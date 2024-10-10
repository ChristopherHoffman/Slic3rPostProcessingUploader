// See https://aka.ms/new-console-template for more information
using Slic3rPostProcessingUploader.Services;
using Slic3rPostProcessingUploader.Services.Parsers;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

// string newPrintUrl = "https://www.3dprintlog.com/prints/new/cura";
// string apiUrl = "https://api.3dprintlog.com/api/Cura/settings";

string newPrintUrl = "https://localhost:4200/prints/new/cura";
string apiUrl = "https://localhost:5001/api/Cura/settings";

// Convert the console to write to a local file
var sw = new System.IO.StreamWriter("C:/tmp/slic3r-console-output.txt", true) { AutoFlush = true };
Console.SetOut(sw);

// Display a console writeline
Console.WriteLine("Starting the 3D Print Log Uploader");

// Read all the current environment variables and filter to just the ones that start with "SLIC3R"
var slic3rVariables = Environment.GetEnvironmentVariables()
    .Cast<DictionaryEntry>()
    .Where(x => x!.Key!.ToString()!.StartsWith("SLIC3R"))
    .ToDictionary(x => x.Key, x => x.Value)
    .Select(d =>
        string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));

// Save the filtered environment variables to a file in the current directory
File.WriteAllText("C:/tmp/slic3r-environment-variables.json", "{" + string.Join(",", slic3rVariables) + "}");

// Now get the string value of the first argument passed to the program
var tempFileName = args.FirstOrDefault();

// Read the contents of the file specified by the first argument
var fileContents = File.ReadAllText(tempFileName!);

// Get the output file name from the environment variable SLIC3R_PP_OUTPUT_NAME
var outputFileName = Environment.GetEnvironmentVariable("SLIC3R_PP_OUTPUT_NAME");


// Save the contents of the file to a new file in the current directory
File.WriteAllText("C:/tmp/slic3r-file-contents.txt", fileContents);

// Dependency Injection



// Parse the contents into a DTO
var parser = new OrcaParser();
var dto = parser.ParseGcode(fileContents);


dto.settings.file_name = Path.GetFileName(outputFileName);

// Convert the filename to a human readable name
var fileNameWithoutExt = Path.GetFileNameWithoutExtension(outputFileName);

dto.settings.print_name = GetTitle(fileNameWithoutExt);

var versionService = new VersionService();

dto.PluginVersion = versionService.GetVersion();


// Save the DTO to a new file in the current directory
File.WriteAllText("C:/tmp/slic3r-dto.json", dto.ToJSON());

// Make an API request to the 3dprintlog.com website
var client = new HttpClient();

// Make a POST request to the 3dprintlog.com website with the contents of the dto
var content = new StringContent(dto.ToJSON(), Encoding.UTF8, "application/json");

try
{
    var response = await client.PostAsync(apiUrl, content);

    Console.WriteLine($"Response: {response.ToString()}");

    var responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        File.WriteAllText("C:/tmp/3d-print-log-api-response.json", responseContent); 
        throw new Exception($"Failed to upload to 3dprintlog.com: {response.StatusCode}");
    }

    File.WriteAllText("C:/tmp/3d-print-log-api-response.json", responseContent);


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
    var snakeCaseFilename = ToSnakeCase(filename);
    var title = string.Join(" ", snakeCaseFilename.Split('_')
        .Where(segment => !segment.Equals("gcode", StringComparison.OrdinalIgnoreCase))
        .Select(s => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)))
        .Trim();

    // Limit to 100 characters
    if (title.Length > 100)
    {
        title = title.Substring(0, 100);
    }

    return title;
}

string ToSnakeCase(string text)
{
    if (text.Length < 2)
    {
        return text.ToLowerInvariant();
    }
    var sb = new StringBuilder();
    sb.Append(char.ToLowerInvariant(text[0]));
    for (int i = 1; i < text.Length; ++i)
    {
        char c = text[i];
        if (char.IsUpper(c))
        {
            sb.Append('_');
            sb.Append(char.ToLowerInvariant(c));
        }
        else
        {
            sb.Append(c);
        }
    }
    return sb.ToString();
}


// Save the response content to a new file in the current directory




// Open a browser window to the 3dprintlog.com website

