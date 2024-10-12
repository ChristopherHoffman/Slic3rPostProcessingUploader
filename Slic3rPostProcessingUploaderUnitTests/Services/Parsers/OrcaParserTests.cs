using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Slic3rPostProcessingUploader.Services.Parsers;

namespace Slic3rPostProcessingUploaderUnitTests.Services.Parsers
{
    [TestClass]
    public class OrcaParserTests
    {
        [TestMethod]
        public void ShouldReturnAnEmptySettingWhenGcodeIsEmpty()
        {
            var parser = new OrcaParser();
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
            var parser = new OrcaParser();
            var result = parser.ParseGcode(OrcaParserTestGcode.CalibrationCube);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.settings);
            Assert.AreEqual(1319, result.settings.estimated_print_time_seconds);
            Assert.AreEqual(0, result.settings.material_used_mg);
            Assert.AreEqual("Settings:", result.settings.note);
            Assert.AreEqual("OrcaSlicer", result.Slicer);
            Assert.AreEqual("2.2.0-rc", result.CuraVersion);
        }

    }
}
