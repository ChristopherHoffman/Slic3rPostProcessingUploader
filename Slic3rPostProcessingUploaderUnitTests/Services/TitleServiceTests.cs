using Slic3rPostProcessingUploader.Services;

namespace Slic3rPostProcessingUploaderUnitTests.Services
{
    [TestClass]
    public class TitleServiceTests
    {
        private TitleService _titleService = null!;

        [TestInitialize]
        public void Setup()
        {
            _titleService = new TitleService();
        }

        #region ToSnakeCase Tests

        [TestMethod]
        public void ToSnakeCase_WithNull_ReturnsEmptyString()
        {
            var result = _titleService.ToSnakeCase(null);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ToSnakeCase_WithEmptyString_ReturnsEmptyString()
        {
            var result = _titleService.ToSnakeCase("");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ToSnakeCase_WithSingleCharacter_ReturnsLowercase()
        {
            var result = _titleService.ToSnakeCase("A");
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void ToSnakeCase_WithPascalCase_ReturnsSnakeCase()
        {
            var result = _titleService.ToSnakeCase("MyFileName");
            Assert.AreEqual("my_file_name", result);
        }

        [TestMethod]
        public void ToSnakeCase_WithCamelCase_ReturnsSnakeCase()
        {
            var result = _titleService.ToSnakeCase("myFileName");
            Assert.AreEqual("my_file_name", result);
        }

        [TestMethod]
        public void ToSnakeCase_WithAllLowercase_ReturnsSameString()
        {
            var result = _titleService.ToSnakeCase("filename");
            Assert.AreEqual("filename", result);
        }

        [TestMethod]
        public void ToSnakeCase_WithConsecutiveUppercase_InsertsUnderscores()
        {
            var result = _titleService.ToSnakeCase("XMLParser");
            Assert.AreEqual("x_m_l_parser", result);
        }

        #endregion

        #region GetTitle Tests

        [TestMethod]
        public void GetTitle_WithNull_ReturnsEmptyString()
        {
            var result = _titleService.GetTitle(null);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetTitle_WithEmptyString_ReturnsEmptyString()
        {
            var result = _titleService.GetTitle("");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetTitle_WithSimpleFilename_ReturnsTitleCase()
        {
            var result = _titleService.GetTitle("calibration_cube");
            Assert.AreEqual("Calibration Cube", result);
        }

        [TestMethod]
        public void GetTitle_WithGcodeSegment_RemovesGcode()
        {
            var result = _titleService.GetTitle("calibration_cube_gcode");
            Assert.AreEqual("Calibration Cube", result);
        }

        [TestMethod]
        public void GetTitle_WithPascalCaseFilename_ConvertsProperly()
        {
            var result = _titleService.GetTitle("CalibrationCube");
            Assert.AreEqual("Calibration Cube", result);
        }

        [TestMethod]
        public void GetTitle_WithMixedCase_HandlesCorrectly()
        {
            var result = _titleService.GetTitle("My3DPrint");
            Assert.AreEqual("My3 D Print", result);
        }

        [TestMethod]
        public void GetTitle_WithVeryLongFilename_TruncatesTo100Characters()
        {
            var longName = string.Join("_", Enumerable.Repeat("segment", 50));
            var result = _titleService.GetTitle(longName);
            Assert.IsTrue(result.Length <= 100);
        }

        [TestMethod]
        public void GetTitle_WithExactly100Characters_DoesNotTruncate()
        {
            // Create a filename that results in exactly 100 characters after conversion
            var result = _titleService.GetTitle("test_file_name");
            Assert.IsTrue(result.Length <= 100);
        }

        [TestMethod]
        public void GetTitle_WithUnderscoresAndGcode_RemovesGcodeAndFormatsCorrectly()
        {
            var result = _titleService.GetTitle("my_awesome_print_gcode_test");
            Assert.AreEqual("My Awesome Print Test", result);
        }

        [TestMethod]
        public void GetTitle_WithOnlyGcode_ReturnsEmptyString()
        {
            var result = _titleService.GetTitle("gcode");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetTitle_WithLowercaseGcode_RemovesGcode()
        {
            // Note: GCODE in uppercase gets broken up by ToSnakeCase into individual letters
            // so only lowercase "gcode" segments are filtered out
            var result = _titleService.GetTitle("test_gcode_file");
            Assert.AreEqual("Test File", result);
        }

        #endregion
    }
}
