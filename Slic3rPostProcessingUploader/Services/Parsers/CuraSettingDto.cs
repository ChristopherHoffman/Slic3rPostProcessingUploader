using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Slic3rPostProcessingUploader.Services.Parsers
{
    internal class CuraSettingDto
    {
        public string Slicer { get; set; }
        public string CuraVersion { get; set; }
        public string PluginVersion { get; set; }

        public CuraSettings settings { get; set; }

        // Create a function called ToJSON that will return a JSON string representation of the object
        public string ToJSON()
        {
            // Serialize into json
            var serializerContext = JsonContext.Default;

            return JsonSerializer.Serialize(this, JsonContext.Default.CuraSettingDto);
        }
    }

    public class CuraSettings
    {
        public string note { get; set; }
        public string print_name { get; set; }
        public string time_stamp { get; set; }
        public int estimated_print_time_seconds { get; set; }
        public int? material_used_mg { get; set; }

        /// <summary>
        /// Base64 encoded image
        /// </summary>
        public string? Snapshot { get; set; }
        public string? file_name { get; set; }

        public List<PrintFilamentSummaryDto>? filamentUsage { get; set; }


    }

    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
    [JsonSerializable(typeof(CuraSettingDto))]
    partial class JsonContext : JsonSerializerContext
    {
    }
}
    