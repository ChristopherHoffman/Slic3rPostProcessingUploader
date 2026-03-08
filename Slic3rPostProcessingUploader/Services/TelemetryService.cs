using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

namespace Slic3rPostProcessingUploader.Services;

internal sealed class TelemetryService : IDisposable
{
    private const string ServiceName = "Slic3rPostProcessingUploader";
    private const string ConnectionString = "InstrumentationKey=44698ebf-3363-4d89-b83d-5a0a616b22f5;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=ff0f0688-8e7b-4a59-b446-2969a28faae2";

    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;
    private readonly TracerProvider? _tracerProvider;
    private readonly bool _isEnabled;

    public TelemetryService(bool disableTelemetry = false)
    {
        _isEnabled = !disableTelemetry;

        if (_isEnabled)
        {
            // Set up logging for custom events
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(options =>
                {
                    options.AddAzureMonitorLogExporter(exporterOptions =>
                    {
                        exporterOptions.ConnectionString = ConnectionString;
                    });
                });
            });
            _logger = _loggerFactory.CreateLogger(ServiceName);

            // Set up tracing for HTTP dependency tracking
            _tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ServiceName)
                .AddHttpClientInstrumentation()
                .AddAzureMonitorTraceExporter(options =>
                {
                    options.ConnectionString = ConnectionString;
                })
                .Build();
        }
    }

    public void TrackEvent(string eventName, Dictionary<string, object>? properties = null)
    {
        if (!_isEnabled || _logger == null) return;

        if (properties != null && properties.Count > 0)
        {
            var state = properties.Select(p => new KeyValuePair<string, object?>(p.Key, p.Value)).ToList();
            state.Add(new KeyValuePair<string, object?>("EventName", eventName));

            _logger.Log(LogLevel.Information, 0, state, null, (s, _) => eventName);
        }
        else
        {
            _logger.LogInformation("{EventName}", eventName);
        }
    }

    public void TrackException(Exception exception, string? context = null)
    {
        if (!_isEnabled || _logger == null) return;

        var state = new List<KeyValuePair<string, object?>>
        {
            new("EventName", "Exception"),
            new("ExceptionType", exception.GetType().Name),
            new("ExceptionMessage", exception.Message),
            new("StackTrace", exception.StackTrace ?? "")
        };

        if (!string.IsNullOrEmpty(context))
        {
            state.Add(new("Context", context));
        }

        _logger.Log(LogLevel.Error, 0, state, exception, (s, ex) => $"Exception: {ex?.Message}");
    }

    public void Flush(int timeoutMilliseconds = 10000)
    {
        _tracerProvider?.ForceFlush(timeoutMilliseconds);
    }

    public void Dispose()
    {
        Flush();
        _tracerProvider?.Dispose();
        _loggerFactory?.Dispose();
    }
}
