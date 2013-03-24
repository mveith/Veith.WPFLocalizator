using System;
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
    [DeploymentItem(@"Resources\TestProject.xml", "Resources")]
    public class UsingProjectsConfigurationTest : EndToEndTestsBase
    {
        [TestInitialize]
        public new void Initialize()
        {
            base.Initialize();

            File.Copy(Path.Combine("Resources", "TestProject.xml"), Path.Combine(this.path, "TestProject.xml"));
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
        public void UsingProjectsConfiguration()
        {
            this.userInteraction.SelectedProject = Path.Combine(this.path, "TestProject.xml");

            this.mainWindowViewModel.OpenProjectCommand.Execute(null);

            mainWindowViewModel.ProcessSelectedDirectoryCommand.Execute(null);

            var resultText = File.ReadAllText(Path.Combine(this.path, this.fileName), Encoding.Default);

            VerifyUsingAnchorsForProjectConfigurationInFile(resultText);
        }

        private void VerifyUsingAnchorsForProjectConfigurationInFile(string resultText)
        {
            Assert.IsTrue(this.IsKeyForProjectUsedInFile(resultText, "Title=\"{0}\">", "TestXAMLWindowTitle"));
            Assert.IsTrue(this.IsKeyForProjectUsedInFile(resultText, "<TextBlock Name=\"TestTextBlock\" Text=\"{0}\" />", "TestXAMLTestTextBlockText"));
            Assert.IsTrue(this.IsKeyForProjectUsedInFile(resultText, "<Button Content=\"{0}\"", "TestXAMLButtonContent"));
        }

        protected bool IsKeyForProjectUsedInFile(string resultText, string pattern, string key)
        {
            return resultText.Contains(string.Format(pattern, GetContentForKey(key, "ProjectPrefix", "MyNamespaceProject", "ResourcesInProject", "ProjectSuffix")));
        }

    }
}
