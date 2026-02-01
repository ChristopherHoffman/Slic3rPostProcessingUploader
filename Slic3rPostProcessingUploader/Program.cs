using Slic3rPostProcessingUploader.Services;
using Slic3rPostProcessingUploader.Services.Parsers;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

[assembly: InternalsVisibleTo("Slic3rPostProcessingUploaderUnitTests")]

try
{
    ArgumentParser arguments = new(args);

    string newPrintUrl = arguments.UseLocalDev ? "https://localhost:4200/prints/new/cura" : "https://www.3dprintlog.com/prints/new/cura";
    string apiUrl = arguments.UseLocalDev ? "https://localhost:5001/api/Cura/settings" : "https://api.3dprintlog.com/api/Cura/settings";

    using var telemetry = new TelemetryService(arguments.DisableTelemetry);

    if (arguments.DisplayHelp)
    {
        DisplayHelp(arguments);
        return;
    }

    if (arguments.DisplayVersion)
    {
        Console.WriteLine($"Slic3rPostProcessingUploader v{new VersionService().GetVersion()}");
        return;
    }

    SetupDebugging(arguments.DebugPath);

    Console.WriteLine("Starting the 3D Print Log Uploader");

    LogEnvironmentVariables(arguments.DebugPath);

    if (string.IsNullOrEmpty(arguments.InputFile))
    {
        throw new ArgumentException("No input file specified. Please provide a G-code file path as the last argument.");
    }

    string fileContents = File.ReadAllText(arguments.InputFile);
    LogFileContents(arguments.DebugPath, fileContents);

    CuraSettingDto dto;

    using (telemetry.StartOperation("parsing"))
    {
        IGcodeParser parser = ParserFactory.GetParser(arguments, telemetry, fileContents);

        dto = parser.ParseGcode(fileContents);

        var outputName = Environment.GetEnvironmentVariable("SLIC3R_PP_OUTPUT_NAME");
        dto.settings.file_name = outputName != null ? Path.GetFileName(outputName) : Path.GetFileName(arguments.InputFile);
        dto.settings.print_name = new TitleService().GetTitle(Path.GetFileNameWithoutExtension(dto.settings.file_name));
        dto.PluginVersion = new VersionService().GetVersion();

        telemetry.TrackEvent("Parse", new Dictionary<string, string> {
            { "Slicer", dto.Slicer },
            { "PluginVersion", dto.PluginVersion },
            { "CuraVersion", dto.CuraVersion }
        });

        LogDto(arguments.DebugPath, dto);
    }

    using (telemetry.StartOperation("uploading"))
    {
        await UploadToApi(apiUrl, dto, arguments.DebugPath, newPrintUrl);
    }

    telemetry.Flush();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.ToString());
}

void DisplayHelp(ArgumentParser arguments)
{
    arguments.DisplayHelpDocs();

    Console.WriteLine("Press any key to exit");
    Console.ReadKey();
    return;
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
            .Where(x => x.Key.ToString()!.StartsWith("SLIC3R"))
            .ToDictionary(x => x.Key, x => x.Value)
            .Select(d => string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value!)));

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
    using HttpClient client = new();
    using StringContent content = new(dto.ToJSON(), Encoding.UTF8, "application/json");

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

        var apiResponse = JsonSerializer.Deserialize(responseContent, ApiResponseContext.Default.ApiResponse);
        if (apiResponse == null || string.IsNullOrEmpty(apiResponse.NewSettingId))
        {
            throw new Exception($"Invalid API response: missing newSettingId");
        }
        new Browser().Open($"{newPrintUrl}?cura_version={dto.CuraVersion}&plugin_version={dto.PluginVersion}&settingId={apiResponse.NewSettingId}");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
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
