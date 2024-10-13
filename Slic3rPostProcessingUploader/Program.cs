// See https://aka.ms/new-console-template for more information
using Slic3rPostProcessingUploader.Services;
using Slic3rPostProcessingUploader.Services.Parsers;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Slic3rPostProcessingUploaderUnitTests")]

ArgumentParser arguments = new(args);

string newPrintUrl;
string apiUrl;

if (arguments.UseLocalDev)
{
    newPrintUrl = "https://localhost:4200/prints/new/cura";
    apiUrl = "https://localhost:5001/api/Cura/settings";
}
else
{
    newPrintUrl = "https://www.3dprintlog.com/prints/new/cura";
    apiUrl = "https://api.3dprintlog.com/api/Cura/settings";
}

// Convert the console to write to a local file

if (!string.IsNullOrEmpty(arguments.DebugPath))
{
    // Check if that path exists and if not create it
    if (!Directory.Exists(arguments.DebugPath))
    {
        _ = Directory.CreateDirectory(arguments.DebugPath);
    }
}

if (!string.IsNullOrEmpty(arguments.DebugPath))
{
    string debugFileName = "slic3r-debug.txt";
    string path = Path.Combine(arguments.DebugPath, debugFileName);

    StreamWriter sw = new(path, true) { AutoFlush = true };
    Console.SetOut(sw);

    Console.WriteLine($"Debugging enabled, logging to {arguments.DebugPath}");
}

// Display a console writeline
Console.WriteLine("Starting the 3D Print Log Uploader");


if (!string.IsNullOrEmpty(arguments.DebugPath))
{
    // Read all the current environment variables and filter to just the ones that start with "SLIC3R"
    IEnumerable<string> slic3rVariables = Environment.GetEnvironmentVariables()
        .Cast<DictionaryEntry>()
        .Where(x => x!.Key!.ToString()!.StartsWith("SLIC3R"))
        .ToDictionary(x => x.Key, x => x.Value)
        .Select(d =>
            string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));

    // Save the filtered environment variables to a file in the current directory
    string envVarFileName = "slic3r-environment-variables.json";
    string path = Path.Combine(arguments.DebugPath, envVarFileName);
    File.WriteAllText(path, "{" + string.Join(",", slic3rVariables) + "}");
}

// Read the contents of the file specified by the first argument
string fileContents = File.ReadAllText(arguments.InputFile);

// Get the output file name from the environment variable SLIC3R_PP_OUTPUT_NAME
string? outputFileName = Environment.GetEnvironmentVariable("SLIC3R_PP_OUTPUT_NAME");

if (!string.IsNullOrEmpty(arguments.DebugPath))
{
    string slicerFileContentsFileName = "slic3r-file-contents.txt";
    string path = Path.Combine(arguments.DebugPath, slicerFileContentsFileName);

    // Save the contents of the file to a new file in the current directory
    File.WriteAllText("C:/tmp/slic3r-file-contents.txt", fileContents);
}

// Dependency Injection


// Parse the contents into a DTO
string template = "";
if (arguments.UseDefaultNoteTemplate)
{
    template = new OrcaDefaultNoteTemplate().getNoteTemplate();
}
else
{
    template = arguments.UseFullNoteTemplate
        ? new OrcaFullNoteTemplate().getNoteTemplate()
        : new NoteTemplateFromFile(arguments.NoteTemplatePath).getNoteTemplate();
}

OrcaParser parser = new(template);
CuraSettingDto dto = parser.ParseGcode(fileContents);


dto.settings.file_name = Path.GetFileName(outputFileName);

// Convert the filename to a human readable name
string fileNameWithoutExt = Path.GetFileNameWithoutExtension(outputFileName);

dto.settings.print_name = GetTitle(fileNameWithoutExt!);

VersionService versionService = new();

dto.PluginVersion = versionService.GetVersion();


if (!string.IsNullOrEmpty(arguments.DebugPath))
{
    string dtoFileName = "slic3r-dto.json";
    string path = Path.Combine(arguments.DebugPath, dtoFileName);

    // Save the DTO to a new file in the current directory
    File.WriteAllText(path, dto.ToJSON());
}

// Make an API request to the 3dprintlog.com website
HttpClient client = new();

// Make a POST request to the 3dprintlog.com website with the contents of the dto
StringContent content = new(dto.ToJSON(), Encoding.UTF8, "application/json");

try
{
    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

    Console.WriteLine($"Response: {response}");

    string responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        if (!string.IsNullOrEmpty(arguments.DebugPath))
        {
            string responseFileName = "3d-print-log-api-response.json";
            string path = Path.Combine(arguments.DebugPath, responseFileName);

            File.WriteAllText(path, responseContent);
        }
        throw new Exception($"Failed to upload to 3dprintlog.com: {response.StatusCode}");
    }

    if (!string.IsNullOrEmpty(arguments.DebugPath))
    {
        string responseFileName = "3d-print-log-api-response.json";
        string path = Path.Combine(arguments.DebugPath, responseFileName);

        // Save the response content to a new file in the current directory
        File.WriteAllText(path, responseContent);
    }


    // Get the newSettingId from the response
    string guid = responseContent.Replace("{", "").Replace("}", "").Split(":")[1].Replace("\"", "");

    // Open the new print page in the browser

    Browser browser = new();

    browser.Open(newPrintUrl + "?cura_version=" + dto.CuraVersion + "&plugin_version=" + dto.PluginVersion + "&settingId=" + guid);
}
catch (Exception e)
{

    Console.WriteLine(e.Message);
}


string GetTitle(string filename)
{
    string snakeCaseFilename = ToSnakeCase(filename);
    string title = string.Join(" ", snakeCaseFilename.Split('_')
        .Where(segment => !segment.Equals("gcode", StringComparison.OrdinalIgnoreCase))
        .Select(CultureInfo.CurrentCulture.TextInfo.ToTitleCase))
        .Trim();

    // Limit to 100 characters
    if (title.Length > 100)
    {
        title = title[..100];
    }

    return title;
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


// Save the response content to a new file in the current directory




// Open a browser window to the 3dprintlog.com website

