using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Slic3rPostProcessingUploader.Services.Parsers;
using Snapshooter.MSTest;


namespace Slic3rPostProcessingUploaderUnitTests.Services.Parsers
{
    [TestClass]
    public class OrcaParserTests
    {
        [TestMethod]
        public void ShouldReturnAnEmptySettingWhenGcodeIsEmpty()
        {
            var parser = new OrcaParser("");
            var result = parser.ParseGcode("");
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.settings);
            Assert.AreEqual(0, result.settings.estimated_print_time_seconds);
            Assert.AreEqual(0, result.settings.material_used_mg);
            Assert.AreEqual("Settings:", result.settings.note);
            Assert.AreEqual("OrcaSlicer", result.Slicer);
            Assert.AreEqual("Unknown", result.CuraVersion);
        }

        [TestMethod]
        public void ShouldReturnExpectedValuesWhenGivenFullGcode()
        {
            var parser = new OrcaParser("");
            var result = parser.ParseGcode(OrcaParserTestGcode.CalibrationCube);

            Snapshot.Match(result);
        }

        [TestMethod]
        public void ShouldRenderTheExpectedNoteWhenGivenATemplateWithNoReplacements()
        {
            string template = "Settings:";

            var parser = new OrcaParser(template);
            var result = parser.ParseGcode(OrcaParserTestGcode.CalibrationCube);

            Assert.AreEqual("Settings:", result.settings.note);
        }

        [TestMethod]
        public void ShouldRenderTheExpectedNoteWhenGivenATemplateWithASingleReplacement()
        {
            string template = """
                Settings:
                    Layer Height: {{layer_height}}
                """;

            var parser = new OrcaParser(template);
            var result = parser.ParseGcode(OrcaParserTestGcode.CalibrationCube);

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

            var parser = new OrcaParser(template);
            var result = parser.ParseGcode(OrcaParserTestGcode.CalibrationCube);

            Assert.AreEqual("""
                Settings:
                    Layer Height: 0.2
                    First Layer Height: 0.200
                    Wall Loops: 3
                    Top Shell Layers: 4
                    Bottom Shell Layers: 3
                    Sparse Infill Density: 15%
                """, result.settings.note);
        }

    }
}
