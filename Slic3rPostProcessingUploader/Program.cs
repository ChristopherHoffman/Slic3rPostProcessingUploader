using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slic3rPostProcessingUploader.Services;
using Slic3rPostProcessingUploader.Services.Parsers;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Slic3rPostProcessingUploaderUnitTests")]

try
{

    IServiceCollection services = new ServiceCollection();

    services.AddLogging(loggingBuilder => loggingBuilder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("Category", LogLevel.Information));
    services.AddApplicationInsightsTelemetryWorkerService((ApplicationInsightsServiceOptions options) => options.ConnectionString = "InstrumentationKey=fb08dc7f-aa66-49fc-81fe-f797f75095eb;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=74fff9d3-fe84-4357-9f68-6dace75d665d");


    IServiceProvider serviceProvider = services.BuildServiceProvider();

    // Obtain logger instance from DI.
    ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
    var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();


    ArgumentParser arguments = new(args);

    string newPrintUrl = arguments.UseLocalDev ? "https://localhost:4200/prints/new/cura" : "https://www.3dprintlog.com/prints/new/cura";
    string apiUrl = arguments.UseLocalDev ? "https://localhost:5001/api/Cura/settings" : "https://api.3dprintlog.com/api/Cura/settings";

    SetupDebugging(arguments.DebugPath);

    Console.WriteLine("Starting the 3D Print Log Uploader");

    LogEnvironmentVariables(arguments.DebugPath);

    string fileContents = File.ReadAllText(arguments.InputFile);
    LogFileContents(arguments.DebugPath, fileContents);

    CuraSettingDto dto;

    using (telemetryClient.StartOperation<RequestTelemetry>("parsing"))
    {
        string template = GetTemplate(arguments);
        OrcaParser parser = new(template);
        dto = parser.ParseGcode(fileContents);

        dto.settings.file_name = Path.GetFileName(Environment.GetEnvironmentVariable("SLIC3R_PP_OUTPUT_NAME"));
        dto.settings.print_name = GetTitle(Path.GetFileNameWithoutExtension(dto.settings.file_name));
        dto.PluginVersion = new VersionService().GetVersion();

        // Track the slicer, plugin version, and cura version as events
        telemetryClient.TrackEvent("Parse", new Dictionary<string, string> { { "Slicer", dto.Slicer },
            { "PluginVersion", dto.PluginVersion },
        { "CuraVersion", dto.CuraVersion } });

        LogDto(arguments.DebugPath, dto);

    }

    using (telemetryClient.StartOperation<RequestTelemetry>("uploading"))
    {
        await UploadToApi(apiUrl, dto, arguments.DebugPath, newPrintUrl);
    }

    // Give application insights time to flush before closing
    telemetryClient.Flush();
    Task.Delay(2000).Wait();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

void SetupDebugging(string debugPath)
{
    if (!string.IsNullOrEmpty(debugPath))
    {
        if (!Directory.Exists(debugPath))
        {
            _ = Directory.CreateDirectory(debugPath);
        }

        string debugFileName = "slic3r-debug.txt";
        string path = Path.Combine(debugPath, debugFileName);

        StreamWriter sw = new(path, true) { AutoFlush = true };
        Console.SetOut(sw);

        Console.WriteLine($"Debugging enabled, logging to {debugPath}");
    }
}

void LogEnvironmentVariables(string debugPath)
{
    if (!string.IsNullOrEmpty(debugPath))
    {
        IEnumerable<string> slic3rVariables = Environment.GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .Where(x => x.Key.ToString().StartsWith("SLIC3R"))
            .ToDictionary(x => x.Key, x => x.Value)
            .Select(d => string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));

        string envVarFileName = "slic3r-environment-variables.json";
        string path = Path.Combine(debugPath, envVarFileName);
        File.WriteAllText(path, "{" + string.Join(",", slic3rVariables) + "}");
    }
}

void LogFileContents(string debugPath, string fileContents)
{
    if (!string.IsNullOrEmpty(debugPath))
    {
        string slicerFileContentsFileName = "slic3r-file-contents.txt";
        string path = Path.Combine(debugPath, slicerFileContentsFileName);
        File.WriteAllText(path, fileContents);
    }
}

string GetTemplate(ArgumentParser arguments)
{
    return arguments.UseDefaultNoteTemplate
        ? new OrcaDefaultNoteTemplate().getNoteTemplate()
        : arguments.UseFullNoteTemplate
        ? new OrcaFullNoteTemplate().getNoteTemplate()
        : new NoteTemplateFromFile(arguments.NoteTemplatePath).getNoteTemplate();
}

void LogDto(string debugPath, CuraSettingDto dto)
{
    if (!string.IsNullOrEmpty(debugPath))
    {
        string dtoFileName = "slic3r-dto.json";
        string path = Path.Combine(debugPath, dtoFileName);
        File.WriteAllText(path, dto.ToJSON());
    }
}

async Task UploadToApi(string apiUrl, CuraSettingDto dto, string debugPath, string newPrintUrl)
{
    HttpClient client = new();
    StringContent content = new(dto.ToJSON(), Encoding.UTF8, "application/json");

    try
    {
        HttpResponseMessage response = await client.PostAsync(apiUrl, content);
        Console.WriteLine($"Response: {response}");

        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            LogApiResponse(debugPath, responseContent);
            throw new Exception($"Failed to upload to 3dprintlog.com: {response.StatusCode}");
        }

        LogApiResponse(debugPath, responseContent);

        string guid = responseContent.Replace("{", "").Replace("}", "").Split(":")[1].Replace("\"", "");
        new Browser().Open($"{newPrintUrl}?cura_version={dto.CuraVersion}&plugin_version={dto.PluginVersion}&settingId={guid}");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

void LogApiResponse(string debugPath, string responseContent)
{
    if (!string.IsNullOrEmpty(debugPath))
    {
        string responseFileName = "3d-print-log-api-response.json";
        string path = Path.Combine(debugPath, responseFileName);
        File.WriteAllText(path, responseContent);
    }
}

string GetTitle(string filename)
{
    string snakeCaseFilename = ToSnakeCase(filename);
    string title = string.Join(" ", snakeCaseFilename.Split('_')
        .Where(segment => !segment.Equals("gcode", StringComparison.OrdinalIgnoreCase))
        .Select(CultureInfo.CurrentCulture.TextInfo.ToTitleCase))
        .Trim();

    return title.Length > 100 ? title[..100] : title;
}

string ToSnakeCase(string text)
{
    if (text.Length < 2)
    {
        return text.ToLowerInvariant();
    }
    StringBuilder sb = new();
    _ = sb.Append(char.ToLowerInvariant(text[0]));
    for (int i = 1; i < text.Length; ++i)
    {
        char c = text[i];
        if (char.IsUpper(c))
        {
            _ = sb.Append('_');
            _ = sb.Append(char.ToLowerInvariant(c));
        }
        else
        {
            _ = sb.Append(c);
        }
    }
    return sb.ToString();
}
