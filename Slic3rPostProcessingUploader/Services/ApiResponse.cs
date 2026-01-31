using System.Text.Json.Serialization;

namespace Slic3rPostProcessingUploader.Services
{
    internal class ApiResponse
    {
        [JsonPropertyName("newSettingId")]
        public string NewSettingId { get; set; } = string.Empty;
    }
}
