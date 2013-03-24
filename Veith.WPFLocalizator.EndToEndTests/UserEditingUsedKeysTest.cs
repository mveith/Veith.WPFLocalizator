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
    public class UserEditingUsedKeysTest : EndToEndTestsBase
    {
        [TestInitialize]
        public new void Initialize()
        {
            this.userEditingPostfix = "EDITED";

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
        public void UserEditingUsedKeys()
        {
            mainWindowViewModel.ProcessSelectedDirectoryCommand.Execute(null);

            this.VerifyUsingEditedKeysInResourceFiles();
            this.VerifyReplacingStringsWithEditedKeysAnchors();
        }

        private void VerifyReplacingStringsWithEditedKeysAnchors()
        {
            var resultText = File.ReadAllText(Path.Combine(this.path, this.fileName), Encoding.Default);

            Assert.IsTrue(this.IsKeyUsedInFile(resultText, "Title=\"{0}\">", "TestXAMLWindowTitle" + this.userEditingPostfix));
            Assert.IsTrue(this.IsKeyUsedInFile(resultText, "<TextBlock Name=\"TestTextBlock\" Text=\"{0}\" />", "TestXAMLTestTextBlockText" + this.userEditingPostfix));
            Assert.IsTrue(this.IsKeyUsedInFile(resultText, "<Button Content=\"{0}\"", "TestXAMLButtonContent" + this.userEditingPostfix));
        }

        private void VerifyUsingEditedKeysInResourceFiles()
        {
            foreach (var filePath in this.resourceFilesPaths)
            {
                var content = File.ReadAllText(Path.Combine(this.path, filePath), Encoding.Default);

                var xml = XDocument.Parse(content);

                var dataElements = xml.Root.Elements("data");

                Assert.AreEqual("Titulek okna", GetAtributeWithKeyValue(dataElements, "TestXAMLWindowTitle" + this.userEditingPostfix));
                Assert.AreEqual("Vnitřní text", GetAtributeWithKeyValue(dataElements, "TestXAMLTestTextBlockText" + this.userEditingPostfix));
                Assert.AreEqual("Text na tlačítku", GetAtributeWithKeyValue(dataElements, "TestXAMLButtonContent" + this.userEditingPostfix));
            }
        }
    }
}
