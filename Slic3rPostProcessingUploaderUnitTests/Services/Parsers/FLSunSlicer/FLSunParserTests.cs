using Slic3rPostProcessingUploader.Services.Parsers.FLSunSlicer;
using Snapshooter.MSTest;


namespace Slic3rPostProcessingUploaderUnitTests.Services.Parsers.FLSunSlicer
{
    [TestClass]
    public class FLSunParserTests
    {
        [TestMethod]
        public void ShouldReturnAnEmptySettingWhenGcodeIsEmpty()
        {
            var parser = new FLSunParser("");
            var result = parser.ParseGcode("");
            Assert.IsNotNull(result);

            Snapshot.Match(result);
        }

        [TestMethod]
        public void ShouldReturnExpectedValuesWhenGivenFullGcode()
        {
            var parser = new FLSunParser("");
            var result = parser.ParseGcode(FLSunParserTestGcode.CalibrationCube);

            Snapshot.Match(result);
        }

        [TestMethod]
        public void ShouldRenderTheExpectedNoteWhenGivenATemplateWithNoReplacements()
        {
            string template = "Settings:";

            var parser = new FLSunParser(template);
            var result = parser.ParseGcode(FLSunParserTestGcode.CalibrationCube);

            Assert.AreEqual("Settings:", result.settings.note);
        }

        [TestMethod]
        public void ShouldRenderTheExpectedNoteWhenGivenATemplateWithASingleReplacement()
        {
            string template = """
                Settings:
                    Layer Height: {{layer_height}}
                """;

            var parser = new FLSunParser(template);
            var result = parser.ParseGcode(FLSunParserTestGcode.CalibrationCube);

            Assert.AreEqual("""
                Settings:
                    Layer Height: 0.2
                """, result.settings.note);
        }

        [TestMethod]
        public void ShouldRenderTheExpectedNoteWhenGivenATemplateWithMultipleReplacements()
        {
            string template = """
                Settings:
                    Layer Height: {{layer_height}}
                    First Layer Height: {{first_layer_height}}
                    Wall Loops: {{wall_loops}}
                    Top Shell Layers: {{top_shell_layers}}
                    Bottom Shell Layers: {{bottom_shell_layers}}
                    Sparse Infill Density: {{sparse_infill_density}}
                """;

            var parser = new FLSunParser(template);
            var result = parser.ParseGcode(FLSunParserTestGcode.CalibrationCube);

            Assert.AreEqual("""
                Settings:
                    Layer Height: 0.2
                    First Layer Height: 0.300
                    Wall Loops: 3
                    Top Shell Layers: 4
                    Bottom Shell Layers: 3
                    Sparse Infill Density: 15%
                """, result.settings.note);
        }

    }
}
