// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Hello, World!");

// Display a console writeline
Console.WriteLine("Hello, World!");

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
var firstArgument = args.FirstOrDefault();

// Read the contents of the file specified by the first argument
var fileContents = File.ReadAllText(firstArgument!);

// Save the contents of the file to a new file in the current directory
File.WriteAllText("C:/tmp/slic3r-file-contents.txt", fileContents);

// Make an API request to the 3dprintlog.com website
var client = new HttpClient();
var response = await client.GetAsync("https://www.3dprintlog.com");

// Read the response content as a string
var responseContent = await response.Content.ReadAsStringAsync();

// Save the response content to a new file in the current directory
File.WriteAllText("C:/tmp/slic3r-api-response.html", responseContent);

// Open a browser window to the 3dprintlog.com website
OpenBrowser("https://www.3dprintlog.com");

void OpenBrowser(string url)
{
    try
    {
        Process.Start(url);
    }
    catch
    {
        // hack because of this: https://github.com/dotnet/corefx/issues/10361
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            throw;
        }
    }
}

public class StringJson
{
    public string? Value { get; set; }
    public string? Key { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(StringJson))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}


