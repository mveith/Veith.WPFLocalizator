using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Veith.WPFLocalizator.EndToEndTests
{
    [TestClass]
    [DeploymentItem(@"Resources\TestXAML.xaml", "Resources")]
    [DeploymentItem(@"Resources\TestCSharpCode.cs", "Resources")]
    [DeploymentItem(@"Resources\Resources.cs-cz.resx", "Resources")]
    [DeploymentItem(@"Resources\Resources.en-gb.resx", "Resources")]
    public class ReplacingStringsInCSharpCodeWithMethodCallTest : EndToEndTestsBase
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
        public void ReplacingStringsInCSharpCodeWithMethodCall()
        {
            mainWindowViewModel.ProcessSelectedDirectoryCommand.Execute(null);

            this.VerifyResult();
        }

        private void VerifyResult()
        {
            var resultText = File.ReadAllText(Path.Combine(this.path, this.cSharpFileName), Encoding.Default);

            VerifyRemovingStringsFromFile(resultText);

            VerifyUsingMethodsCallInFile(resultText);
        }

        private static void VerifyRemovingStringsFromFile(string resultText)
        {
            Assert.IsFalse(resultText.Contains("MessageBox.Show(\"Message box text\");"));
            Assert.IsFalse(resultText.Contains("Console.WriteLine(\"Console write line text\");"));
            Assert.IsFalse(resultText.Contains("this.MethodWithParameter(\"Parameter text\");"));
        }

        private void VerifyUsingMethodsCallInFile(string resultText)
        {
            Assert.IsFalse(resultText.Contains("MessageBox.Show(Localization.GetText(\"TestCSharpCode1\"));"));
            Assert.IsFalse(resultText.Contains("Console.WriteLine(Localization.GetText(\"TestCSharpCode2\"));"));
            Assert.IsFalse(resultText.Contains("this.MethodWithParameter(Localization.GetText(\"TestCSharpCode3\"));"));
        }
    }
}
