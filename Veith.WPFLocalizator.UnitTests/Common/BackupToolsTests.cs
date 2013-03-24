using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Common;

namespace Veith.WPFLocalizator.UnitTests.Common
{
    [TestClass]
    public class BackupToolsTests
    {
        private BackupTool backup;

        private IFileSystemWrapper fileSystem;

        private string backupDirectory;

        private string directory;
        private string[] resourceFilesPaths;
        private string[] selectedExtensions;

        [TestInitialize]
        public void TestInitialize()
        {
            this.fileSystem = Substitute.For<IFileSystemWrapper>();

            this.selectedExtensions = new[] { "xaml", "cs" };
            this.backupDirectory = "BACKUPDirectory";

            this.backup = new BackupTool(this.fileSystem, new BackupConfiguration(this.backupDirectory, this.selectedExtensions));

            this.directory = "DIRECTORY";
            this.resourceFilesPaths = new[] { "ResFilePath" };
        }

        [TestMethod]
        public void IfBackupThenBackupAllFilesWithExtension()
        {
            this.fileSystem.GetFilesWithExtensionInDirectory(this.directory, this.selectedExtensions.First())
                .Returns(new[] { "XamlFile" });
            this.fileSystem.GetFilesWithExtensionInDirectory(this.directory, this.selectedExtensions.Last())
                .Returns(new[] { "CsFile" });

            this.backup.Backup(this.directory, this.resourceFilesPaths);

            this.fileSystem.Received().BackupFiles(
                Arg.Is<IEnumerable<string>>(paths => paths.First() == "XamlFile" && paths.Last() == "CsFile"),
                Path.Combine(this.backupDirectory, "Documents"),
                this.directory);
        }

        [TestMethod]
        public void IfBackupThenBackupAllResourceDirectories()
        {
            this.backup.Backup(this.directory, this.resourceFilesPaths);

            this.fileSystem.Received().BackupFiles(
                Arg.Is<IEnumerable<string>>(paths => paths.Single() == "ResFilePath"),
                Path.Combine(this.backupDirectory, "Resources"),
                string.Empty);
        }

        [TestMethod]
        public void IfDeletingBackupThenDeleteBackupDirectory()
        {
            this.backup.DeleteBackup();

            this.fileSystem.Received().DeleteDirectory(this.backupDirectory);
        }

        [TestMethod]
        public void IfRevertingThenRevertBackupedDocuments()
        {
            this.backup.Revert(this.directory, this.resourceFilesPaths);

            this.fileSystem.Received().RevertAllFiles(Path.Combine(this.backupDirectory, "Documents"), this.directory);
        }

        [TestMethod]
        public void IfRevertingThenRevertBackupedResources()
        {
            this.backup.Revert(this.directory, this.resourceFilesPaths);

            this.fileSystem.Received().RevertAllFiles(Path.Combine(this.backupDirectory, "Resources"), this.resourceFilesPaths);
        }
    }
}
