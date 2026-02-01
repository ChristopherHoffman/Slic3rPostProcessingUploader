using System.Diagnostics;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Slic3rPostProcessingUploader.Services;

internal sealed class TelemetryService : IDisposable
{
    private const string ServiceName = "Slic3rPostProcessingUploader";
    private const string ConnectionString = "InstrumentationKey=44698ebf-3363-4d89-b83d-5a0a616b22f5;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=ff0f0688-8e7b-4a59-b446-2969a28faae2";

    private static readonly ActivitySource ActivitySource = new(ServiceName);
    private readonly TracerProvider? _tracerProvider;
    private readonly bool _isEnabled;

    public TelemetryService(bool disableTelemetry = false)
    {
        _isEnabled = !disableTelemetry;

        if (_isEnabled)
        {
            _tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ServiceName)
                .AddAzureMonitorTraceExporter(options =>
                {
                    options.ConnectionString = ConnectionString;
                })
                .Build();
        }
    }

    public Activity? StartOperation(string operationName)
    {
        if (!_isEnabled) return null;
        return ActivitySource.StartActivity(operationName, ActivityKind.Internal);
    }

    public void TrackEvent(string eventName, Dictionary<string, string>? properties = null)
    {
        if (!_isEnabled) return;

        using var activity = ActivitySource.StartActivity(eventName, ActivityKind.Internal);
        if (activity != null && properties != null)
        {
            foreach (var property in properties)
            {
                activity.SetTag(property.Key, property.Value);
            }
        }
    }

    public void Flush()
    {
        _tracerProvider?.ForceFlush();
    }

    public void Dispose()
    {
        _tracerProvider?.Dispose();
    }
}
