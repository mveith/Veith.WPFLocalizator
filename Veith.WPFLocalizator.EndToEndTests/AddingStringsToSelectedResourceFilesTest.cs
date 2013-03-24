using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Veith.WPFLocalizator.EndToEndTests
{
    [TestClass]
    [DeploymentItem(@"Resources\TestXAML.xaml", "Resources")]
    [DeploymentItem(@"Resources\Resources.cs-cz.resx", "Resources")]
    [DeploymentItem(@"Resources\Resources.en-gb.resx", "Resources")]
    public class AddingStringsToSelectedResourceFilesTest : EndToEndTestsBase
    {
        [TestInitialize]
        public new void Initialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
#if DEBUG
        [Ignore]
#endif
        public void AddingStringsToSelectedResourceFiles()
        {
            mainWindowViewModel.ProcessSelectedDirectoryCommand.Execute(null);

            foreach (var filePath in this.resourceFilesPaths)
            {
                VerifyFillingResourcesFile(filePath);
            }
        }

        private void VerifyFillingResourcesFile(string filePath)
        {
            var content = File.ReadAllText(Path.Combine(this.path, filePath), Encoding.Default);

            var xml = XDocument.Parse(content);

            var dataElements = xml.Root.Elements("data");

            Assert.AreEqual("Titulek okna", GetAtributeWithKeyValue(dataElements, "TestXAMLWindowTitle"));
            Assert.AreEqual("Vnitřní text", GetAtributeWithKeyValue(dataElements, "TestXAMLTestTextBlockText"));
            Assert.AreEqual("Text na tlačítku", GetAtributeWithKeyValue(dataElements, "TestXAMLButtonContent"));
        }
    }
}
