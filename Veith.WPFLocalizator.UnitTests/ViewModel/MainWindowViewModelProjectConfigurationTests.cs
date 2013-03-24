using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.DocumentsProcessing;

namespace Veith.WPFLocalizator.UnitTests.ViewModel
{
    [TestClass]
    public class MainWindowViewModelProjectConfigurationTests : MainWindowViewModelTestsBase
    {
        private string selectedProjectName;
        private DocumentProcessorConfiguration configurationForProject;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();

            this.selectedProjectName = "TestProjectName";

            this.userInteraction.SelectProject().Returns(x => this.selectedProjectName);

            this.configurationForProject = new DocumentProcessorConfiguration()
            {
                NamespaceValue = "NSTest",
                NewValuePrefix = "NewValPrefTest",
                Prefix = "PrefTest",
                ResourceFileName = "ResTest",
                Suffix = "SufTest"
            };

            this.configurationFactory.CreateConfigurationForProject(Arg.Any<string>()).Returns(this.configurationForProject);
        }

        [TestMethod]
        public void IfOpeningProjectConfigurationThenUseUserInteractionToSelectProjectName()
        {
            this.OpenProject();

            this.userInteraction.Received().SelectProject();
        }

        [TestMethod]
        public void IfOpeningProjectThenUseConfigurationFactoryForSelectedProject()
        {
            this.OpenProject();

            this.configurationFactory.Received().CreateConfigurationForProject(this.selectedProjectName);
        }

        [TestMethod]
        public void IfOpeningProjectThenUseNewConfiguration()
        {
            this.OpenProject();

            Assert.AreEqual("NSTest", this.viewModel.Configuration.NamespaceValue);
            Assert.AreEqual("NewValPrefTest", this.viewModel.Configuration.NewValuePrefix);
            Assert.AreEqual("PrefTest", this.viewModel.Configuration.Prefix);
            Assert.AreEqual("ResTest", this.viewModel.Configuration.ResourceFileName);
            Assert.AreEqual("SufTest", this.viewModel.Configuration.Suffix);
        }

        [TestMethod]
        public void IfUserCancelSelectingThenDoNothing()
        {
            this.userInteraction.SelectProject().Returns(default(string));

            this.OpenProject();

            this.configurationFactory.DidNotReceive().CreateConfigurationForProject(Arg.Any<string>());
            Assert.AreEqual(this.configuration, this.viewModel.Configuration.OriginalConfiguration);
        }

        [TestMethod]
        public void IfSavingProjectThenUseConfigurationFactoryToSerializeConfigurationToXmlString()
        {
            this.SaveProject();

            this.configurationFactory.Received().Serialize(this.configuration);
        }

        [TestMethod]
        public void IfSavingProjectThenSaveSerializedProject()
        {
            this.configurationFactory.Serialize(Arg.Any<DocumentProcessorConfiguration>()).Returns("QQQ");

            this.SaveProject();

            this.userInteraction.Received().SaveProject("QQQ", null);
        }

        [TestMethod]
        public void IfOpeningProjectThenSetSelectedProjectNameToOpeningProjectName()
        {
            this.selectedProjectName = "C:/Directory/project.xml";

            this.OpenProject();

            Assert.AreEqual("project", this.viewModel.SelectedProjectName);
        }

        [TestMethod]
        public void IfSavingProjectAndExistingProjectIsOpenedThenUseActualProjectName()
        {
            this.OpenProject();

            this.SaveProject();

            this.userInteraction.Received().SaveProject(Arg.Any<string>(), this.selectedProjectName);
        }

        [TestMethod]
        public void AfterSavingProjectChangeSelectedProjectNameToNewProjectName()
        {
            this.userInteraction.SaveProject(Arg.Any<string>(), Arg.Any<string>()).Returns("NewProject");

            this.SaveProject();

            Assert.AreEqual("NewProject", this.viewModel.SelectedProjectName);
        }

        [TestMethod]
        public void IfSavingIsCanceledThenDoNotSetNewProjectName()
        {
            this.userInteraction.SaveProject(Arg.Any<string>(), Arg.Any<string>()).Returns(default(string));

            this.OpenProject();

            this.SaveProject();

            Assert.AreEqual(this.selectedProjectName, this.viewModel.SelectedProjectName);

        }

        private void OpenProject()
        {
            this.viewModel.OpenProjectCommand.Execute(null);
        }

        private void SaveProject()
        {
            this.viewModel.SaveProjectCommand.Execute(null);
        }
    }
}
