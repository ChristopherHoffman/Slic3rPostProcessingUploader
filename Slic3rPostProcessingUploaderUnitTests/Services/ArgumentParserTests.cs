using Slic3rPostProcessingUploader.Services;
using Slic3rPostProcessingUploader.Services.Parsers;
using Snapshooter.MSTest;
using System.IO;

namespace Slic3rPostProcessingUploaderUnitTests.Services
{
    [TestClass]
    public class ArgumentParserTests
    {
        [TestMethod]
        public void ShouldDisplayHelpDocs()
        {
            var parser = new ArgumentParser([]);

            string allConsoleOutput;

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                parser.DisplayHelpDocs();

                allConsoleOutput = sw.ToString();
            }

            Assert.IsNotNull(allConsoleOutput);

            Snapshot.Match(allConsoleOutput);
        }
    }
}
