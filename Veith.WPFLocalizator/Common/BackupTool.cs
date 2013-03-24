using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Veith.WPFLocalizator.Common
{
    public class BackupTool : IBackupTool
    {
        private readonly IFileSystemWrapper fileSystem;
        private readonly string documentsBackupDirectory;
        private readonly string resourcesBackupDirectory;
        private readonly string backupDirectory;
        private readonly IEnumerable<string> selectedExtensions;

        private string directory;
        private IEnumerable<string> resourceFilesPaths;

        public BackupTool(IFileSystemWrapper fileSystem, BackupConfiguration configuration)
        {
            this.fileSystem = fileSystem;
            this.backupDirectory = configuration.BackupDirectory;
            this.documentsBackupDirectory = Path.Combine(this.backupDirectory, "Documents");
            this.resourcesBackupDirectory = Path.Combine(this.backupDirectory, "Resources");
            this.selectedExtensions = configuration.SelectedExtensions;
        }

        public void Revert(string directory, IEnumerable<string> resourceFilesPaths)
        {
            this.directory = directory;
            this.resourceFilesPaths = resourceFilesPaths;

            this.fileSystem.RevertAllFiles(this.documentsBackupDirectory, this.directory);
            this.fileSystem.RevertAllFiles(this.resourcesBackupDirectory, this.resourceFilesPaths);
        }

        public void DeleteBackup()
        {
            this.fileSystem.DeleteDirectory(this.backupDirectory);
        }

        public void Backup(string directory, IEnumerable<string> resourceFilesPaths)
        {
            this.directory = directory;
            this.resourceFilesPaths = resourceFilesPaths;

            this.BackupDocuments();

            this.BackupResourceFiles();
        }

        private void BackupResourceFiles()
        {
            this.fileSystem.BackupFiles(this.resourceFilesPaths, this.resourcesBackupDirectory, string.Empty);
        }

        private void BackupDocuments()
        {
            var files = this.selectedExtensions.SelectMany(ex => this.fileSystem.GetFilesWithExtensionInDirectory(this.directory, ex)).ToArray();

            this.fileSystem.BackupFiles(files, this.documentsBackupDirectory, this.directory);
        }
    }

    public class BackupConfiguration
    {
        public BackupConfiguration(string backupDirectory, IEnumerable<string> selectedExtensions)
        {
            this.BackupDirectory = backupDirectory;
            this.SelectedExtensions = selectedExtensions;
        }

        public string BackupDirectory { get; private set; }

        public IEnumerable<string> SelectedExtensions { get; private set; }
    }
}
