using Slic3rPostProcessingUploader.Services;
using System.Text.Json;

namespace Slic3rPostProcessingUploaderUnitTests.Services
{
    [TestClass]
    public class ApiResponseTests
    {
        [TestMethod]
        public void Deserialize_WithValidJson_ReturnsNewSettingId()
        {
            var json = """{"newSettingId":"abc-123-def"}""";

            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("abc-123-def", response.NewSettingId);
        }

        [TestMethod]
        public void Deserialize_WithGuidNewSettingId_ReturnsNewSettingId()
        {
            var json = """{"newSettingId":"550e8400-e29b-41d4-a716-446655440000"}""";

            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("550e8400-e29b-41d4-a716-446655440000", response.NewSettingId);
        }

        [TestMethod]
        public void Deserialize_WithExtraFields_IgnoresExtraFields()
        {
            var json = """{"newSettingId":"abc-123","extraField":"ignored","number":42}""";

            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("abc-123", response.NewSettingId);
        }

        [TestMethod]
        public void Deserialize_WithMissingNewSettingId_ReturnsEmptyString()
        {
            var json = """{"otherField":"value"}""";

            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual(string.Empty, response.NewSettingId);
        }

        [TestMethod]
        public void Deserialize_WithNullNewSettingId_ReturnsNull()
        {
            var json = """{"newSettingId":null}""";

            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsNull(response.NewSettingId);
        }

        [TestMethod]
        public void Deserialize_WithEmptyJson_ReturnsDefaultValues()
        {
            var json = """{}""";

            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual(string.Empty, response.NewSettingId);
        }
    }
}
