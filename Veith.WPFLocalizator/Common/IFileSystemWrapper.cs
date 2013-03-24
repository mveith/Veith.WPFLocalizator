using System.Collections.Generic;

namespace Veith.WPFLocalizator.Common
{
    public interface IFileSystemWrapper
    {
        IEnumerable<string> GetFilesWithExtensionInDirectory(string directoryPath, string extension);

        string GetFileContent(string filePath);

        void SaveFileContent(string filePath, string content);

        string SelectDirectory();

        IEnumerable<string> SelectFiles();

        void BackupFiles(IEnumerable<string> filePaths, string backupDirectory, string originalDirectory);

        void DeleteDirectory(string directory);

        void RevertAllFiles(string backupDirectory, string originalDirectory);

        void RevertAllFiles(string backupDirectory, IEnumerable<string> originalFilePaths);
    }
}
