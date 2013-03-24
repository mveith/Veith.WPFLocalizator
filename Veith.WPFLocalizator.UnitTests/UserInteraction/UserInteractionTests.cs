using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;
using System.Linq;
using Veith.WPFLocalizator.Common;
using System.IO;

namespace Veith.WPFLocalizator.UnitTests.UserInteraction
{
    [TestClass]
    public class UserInteractionTests
    {
        private IUserEditingKeysWindow userEditingKeysWindow;
        private IFileSystemWrapper fileSystemWrapper;
        private ISelectingProjectWindow selectingProjectWindow;
        private INameSelectingWindow nameSelectingWindow;

        private DefaultUserInteraction userInteraction;
        private List<KeyAndValueItem> items;
        private string projectsDirectory;
        private string fileName;

        [TestInitialize]
        public void TestInitialize()
        {
            this.userEditingKeysWindow = Substitute.For<IUserEditingKeysWindow>();
            this.fileSystemWrapper = Substitute.For<IFileSystemWrapper>();
            this.selectingProjectWindow = Substitute.For<ISelectingProjectWindow>();
            this.nameSelectingWindow = Substitute.For<INameSelectingWindow>();

            this.projectsDirectory = "QQ";

            this.userInteraction = new DefaultUserInteraction(
                this.userEditingKeysWindow,
                this.fileSystemWrapper,
                new ProjectsTools(this.projectsDirectory)
                {
                    SelectingProjectWindow = this.selectingProjectWindow,
                    NameSelectingWindow = this.nameSelectingWindow
                });
            this.items = new List<KeyAndValueItem>();
            this.items.Add(new KeyAndValueItem("KEY", "VALUE"));

            this.fileName = "TESTFileName";
        }

        [TestMethod]
        public void IfUseUserEditingKeysThenShowUserEditingKeysWindow()
        {
            this.UserEditKeys();

            this.userEditingKeysWindow.Received().ShowWindow(this.items, this.fileName);
        }

        [TestMethod]
        public void IfSelectingDirectoryThenUseFileSystemWrapper()
        {
            this.userInteraction.SelectDirectory();

            this.fileSystemWrapper.Received().SelectDirectory();
        }

        [TestMethod]
        public void IfSelectingDirectoryThenReturnDirectoryFromFileSystem()
        {
            this.fileSystemWrapper.SelectDirectory().Returns("DIR");

            var selectedDirectory = this.userInteraction.SelectDirectory();

            Assert.AreEqual("DIR", selectedDirectory);
        }

        [TestMethod]
        public void IfSelectingFilesThenUseFileSystemWrapperForSelectingFiles()
        {
            this.fileSystemWrapper.SelectFiles().Returns(new[] { "FILE", "FILE2" });

            var selectedFiles = this.userInteraction.SelectFiles().ToArray();

            this.fileSystemWrapper.Received().SelectFiles();

            Assert.AreEqual("FILE", selectedFiles[0]);
            Assert.AreEqual("FILE2", selectedFiles[1]);
        }

        [TestMethod]
        public void IfSelectingProjectThenGetFilesFromProjectsDirectory()
        {
            this.userInteraction.SelectProject();

            this.fileSystemWrapper.Received().GetFilesWithExtensionInDirectory(this.projectsDirectory, "xml");
        }

        [TestMethod]
        public void IfSelectingProjectThenUseSelectingProjectWindow()
        {
            this.fileSystemWrapper.GetFilesWithExtensionInDirectory(Arg.Any<string>(), Arg.Any<string>()).Returns(new[] { "project.xml" });

            this.userInteraction.SelectProject();

            this.selectingProjectWindow.Received().SelectProject(Arg.Is<IEnumerable<string>>(v => v.Single() == "project.xml"));
        }

        [TestMethod]
        public void IfSelectingProjectThenReturnProjectSelectedInWindow()
        {
            this.selectingProjectWindow.SelectProject(Arg.Any<IEnumerable<string>>()).Returns("project.xml");

            var selectedProject = this.userInteraction.SelectProject();

            Assert.AreEqual("project.xml", selectedProject);
        }

        [TestMethod]
        public void IfSavingProjectThenUseNameSelectingWindowForSelectingName()
        {
            this.userInteraction.SaveProject("QQ", "Actual");

            this.nameSelectingWindow.Received().GetName("Actual");
        }

        [TestMethod]
        public void IfSavingProjectThenSaveSerializedProjectWithNameToProjectsDirectory()
        {
            this.nameSelectingWindow.GetName(Arg.Any<string>()).Returns("NewProject");

            this.userInteraction.SaveProject("QQ", "Actual");

            this.fileSystemWrapper.Received().SaveFileContent(Path.Combine(this.projectsDirectory, "NewProject.xml"), "QQ");
        }

        [TestMethod]
        public void IfSavingNameAndNameIsNullThenCancelSaving()
        {
            this.nameSelectingWindow.GetName(Arg.Any<string>()).Returns(default(string));

            this.userInteraction.SaveProject("QQ", "Actual");

            this.fileSystemWrapper.DidNotReceive().SaveFileContent(Arg.Any<string>(), Arg.Any<string>());
        }

        [TestMethod]
        public void IfSavingProjectThenReturnNewProjectName()
        {
            this.nameSelectingWindow.GetName(Arg.Any<string>()).Returns("Name");

            var projectName = this.userInteraction.SaveProject("QQ", "ZZZ");

            Assert.AreEqual("Name", projectName);
        }

        private void UserEditKeys()
        {
            this.userInteraction.UserEditingKeys(this.items, this.fileName);
        }
    }
}
