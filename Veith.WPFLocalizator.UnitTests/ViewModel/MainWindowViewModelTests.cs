using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.DocumentsProcessing;

namespace Veith.WPFLocalizator.UnitTests.ViewModel
{
    [TestClass]
    public class MainWindowViewModelTests : MainWindowViewModelTestsBase
    {
        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();

            this.userInteraction.SelectFiles().Returns(new[] { "TEST PATH" });
        }

        [TestMethod]
        public void IfProcessingSelectedDirectoryThenUseDirectoriesProcessor()
        {
            this.ProcessSelectedDirectory();

            this.processor.Received().ProcessDirectory(this.directoryPath, this.configuration);
        }

        [TestMethod]
        public void AfterProcessingDirectoryShowMessageAboutSuccess()
        {
            this.ProcessSelectedDirectory();

            this.userInteraction.Received().ShowMessage(Arg.Any<string>());
        }

        [TestMethod]
        public void IfProcessingDirectoryThrowsExceptionThenShowErrorMessageToUser()
        {
            var exception = new System.Exception();
            this.processor.When(x => x.ProcessDirectory(Arg.Any<string>(), Arg.Any<DocumentProcessorConfiguration>()))
                .Do(x => { throw exception; });

            this.ProcessSelectedDirectory();

            this.userInteraction.Received().ShowErrorMessage(exception);
        }

        [TestMethod]
        public void IfSelectingDirectoryThenUseUserInteractionSelectDirectory()
        {
            this.SelectDirectory();

            this.userInteraction.Received().SelectDirectory();
        }

        [TestMethod]
        public void IfSelectingDirectoryThenUseUserSelectedDirectoryAsSelectedDirectory()
        {
            this.userInteraction.SelectDirectory().Returns("SELECTED DIRECTORY");

            this.SelectDirectory();

            Assert.AreEqual("SELECTED DIRECTORY", this.viewModel.SelectedDirectory);
        }

        [TestMethod]
        public void IfChangingSelectedDirectoryThenRaisePropertyChangedEvent()
        {
            var raised = false;

            this.viewModel.PropertyChanged += (s, ea) => raised = ea.PropertyName == "SelectedDirectory";

            this.viewModel.SelectedDirectory = "NEW_SELECTION";

            Assert.AreEqual(true, raised);
        }

        [TestMethod]
        public void IfAddingDictionaryThenUseUserInteractionSelectFiles()
        {
            this.AddDictionary();

            this.userInteraction.Received().SelectFiles();
        }

        [TestMethod]
        public void IfAddingDictionaryThenAddSelectedFilePathToDictionariesList()
        {
            this.userInteraction.SelectFiles().Returns(new[] { "FILE", "FILE2" });

            this.AddDictionary();

            Assert.AreEqual("FILE", this.viewModel.ResourceFilesPaths.First());
            Assert.AreEqual("FILE2", this.viewModel.ResourceFilesPaths.Last());
        }

        [TestMethod]
        public void ConfigurationsReturnsDocumentProcessorConfiguration()
        {
            Assert.AreEqual(this.configuration.Prefix, this.viewModel.Configuration.Prefix);
            Assert.AreEqual(this.configuration.Suffix, this.viewModel.Configuration.Suffix);
            Assert.AreEqual(this.configuration.NamespaceValue, this.viewModel.Configuration.NamespaceValue);
            Assert.AreEqual(this.configuration.ResourceFileName, this.viewModel.Configuration.ResourceFileName);
            Assert.AreEqual(true, !string.IsNullOrEmpty(this.viewModel.Configuration.Sample));
        }

        [TestMethod]
        public void IfUserSelectNullDirectoryPathThenNotAddDirectory()
        {
            this.userInteraction.SelectFiles().Returns(new string[] { null });

            this.AddDictionary();

            Assert.AreEqual(0, this.viewModel.ResourceFilesPaths.Count);
        }

        [TestMethod]
        public void IfRemovingResourceFileThenRemoveSelectedResourceFileFromPaths()
        {
            this.AddDictionary();

            this.viewModel.SelectedResourceFile = this.viewModel.ResourceFilesPaths.First();

            this.RemoveDictionary();

            Assert.AreEqual(0, this.viewModel.ResourceFilesPaths.Count);
        }

        [TestMethod]
        public void IfSelectedResourceFileIsNotInResourceFilePathsThenCommandCannotBeExecuted()
        {
            this.AddDictionary();

            this.viewModel.SelectedResourceFile = "INVALID PATH";

            this.RemoveDictionary();

            Assert.AreEqual(false, this.viewModel.RemoveResourcesDictionaryCommand.CanExecute(null));
        }

        private void SelectDirectory()
        {
            this.viewModel.SelectDirectoryCommand.Execute(null);
        }

        private void AddDictionary()
        {
            this.viewModel.AddResourcesDictionariesCommand.Execute(null);
        }

        private void RemoveDictionary()
        {
            this.viewModel.RemoveResourcesDictionaryCommand.Execute(null);
        }

        private void ProcessSelectedDirectory()
        {
            this.viewModel.ProcessSelectedDirectoryCommand.Execute(null);
        }
    }
}
