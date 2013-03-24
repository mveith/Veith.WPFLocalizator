using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.DocumentsLoading;
using Veith.WPFLocalizator.DocumentsParsing;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.DocumentsSerializing;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.EndToEndTests
{
    [TestClass]
    [DeploymentItem(@"Resources\TestXAML.xaml", "Resources")]
    [DeploymentItem(@"Resources\Resources.cs-cz.resx", "Resources")]
    [DeploymentItem(@"Resources\Resources.en-gb.resx", "Resources")]
    public class ReplacingStringsInXAMLWithResourcesAnchorsTest : EndToEndTestsBase
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
        public void ReplaceStringsInXAMLWithResourcesAnchors()
        {
            mainWindowViewModel.ProcessSelectedDirectoryCommand.Execute(null);

            this.VerifyResult();
        }

        private void VerifyResult()
        {
            var resultText = File.ReadAllText(Path.Combine(this.path, this.fileName), Encoding.Default);

            VerifyRemovingStringsFromFile(resultText);

            VerifyUsingKeysInFile(resultText);
        }

        private static void VerifyRemovingStringsFromFile(string resultText)
        {
            Assert.IsFalse(resultText.Contains("Title=\"Titulek okna\">"));
            Assert.IsFalse(resultText.Contains("<TextBlock Name=\"TestTextBlock\" Text=\"Vnitřní text\" />"));
            Assert.IsFalse(resultText.Contains("<Button Content=\"Text na tlačítku\""));
        }

        private void VerifyUsingKeysInFile(string resultText)
        {
            Assert.IsTrue(this.IsKeyUsedInFile(resultText, "Title=\"{0}\">", "TestXAMLWindowTitle"));
            Assert.IsTrue(this.IsKeyUsedInFile(resultText, "<TextBlock Name=\"TestTextBlock\" Text=\"{0}\" />", "TestXAMLTestTextBlockText"));
            Assert.IsTrue(this.IsKeyUsedInFile(resultText, "<Button Content=\"{0}\"", "TestXAMLButtonContent"));
        }
    }
}
