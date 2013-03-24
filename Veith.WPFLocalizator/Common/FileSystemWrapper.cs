using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Veith.WPFLocalizator.Common
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        private Encoding defaultEncoding = Encoding.UTF8;

        public IEnumerable<string> GetFilesWithExtensionInDirectory(string directoryPath, string extension)
        {
            if (IsFilePath(directoryPath))
            {
                return new[] { directoryPath };
            }

            var allFilesInDirectory = this.GetFilesInDirectoryRecursive(directoryPath);

            var filesWithExtension = allFilesInDirectory.Where(f => Path.GetExtension(f).Replace(".", string.Empty).ToUpper() == extension.ToUpper());

            return filesWithExtension.ToArray();
        }

        public string GetFileContent(string filePath)
        {
            return File.ReadAllText(filePath, this.defaultEncoding);
        }

        public void SaveFileContent(string filePath, string content)
        {
            File.WriteAllText(filePath, content, this.defaultEncoding);
        }

        public IEnumerable<string> SelectFiles()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            var result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileNames;
            }

            return new string[] { };
        }

        public string SelectDirectory()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return null;
        }

        public void BackupFiles(IEnumerable<string> filePaths, string backupDirectory, string originalDirectory)
        {
            if (IsFilePath(originalDirectory))
            {
                originalDirectory = Path.GetDirectoryName(originalDirectory);
            }

            foreach (var filePath in filePaths)
            {
                string relativePath = GetRelativeFilePathInDirectory(filePath, originalDirectory);

                var backupFileDirectoryPath = Path.Combine(backupDirectory, Path.GetDirectoryName(relativePath));

                if (!Directory.Exists(backupFileDirectoryPath))
                {
                    Directory.CreateDirectory(backupFileDirectoryPath);
                }

                File.Copy(filePath, Path.Combine(backupDirectory, relativePath), true);
            }
        }

        public void DeleteDirectory(string directory)
        {
            Directory.Delete(directory, true);
        }

        public void RevertAllFiles(string backupDirectory, string originalDirectory)
        {
            if (IsFilePath(originalDirectory))
            {
                originalDirectory = Path.GetDirectoryName(originalDirectory);
            }

            var files = this.GetFilesInDirectoryRecursive(backupDirectory);

            foreach (var file in files)
            {
                var filePath = GetRelativeFilePathInDirectory(file, backupDirectory);
                var fullFilePath = Path.Combine(originalDirectory, filePath);

                if (!AreFileEquals(file, fullFilePath))
                {
                    File.Copy(file, fullFilePath, true);
                }
            }
        }

        public void RevertAllFiles(string backupDirectory, IEnumerable<string> originalFilePaths)
        {
            var backupedFiles = this.GetFilesInDirectoryRecursive(backupDirectory);

            foreach (var file in originalFilePaths)
            {
                var backupedFile = backupedFiles.First(f => Path.GetFileName(f) == Path.GetFileName(file));

                if (!AreFileEquals(backupedFile, file))
                {
                    File.Copy(backupedFile, file, true);
                }
            }
        }

        private static bool AreFileEquals(string path1, string path2)
        {
            var file1Bytes = File.ReadAllBytes(path1);
            var file2Bytes = File.ReadAllBytes(path2);

            if (file1Bytes.Length != file2Bytes.Length)
            {
                return false;
            }

            for (int i = 0; i < file1Bytes.Length; i++)
            {
                if (file1Bytes[i] != file2Bytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetRelativeFilePathInDirectory(string filePath, string directory)
        {
            if (!directory.EndsWith("\\") && !string.IsNullOrEmpty(directory))
            {
                directory += "\\";
            }

            Uri file = new Uri(filePath);

            string relativePath = Path.GetFileName(filePath);

            if (!string.IsNullOrEmpty(directory))
            {
                Uri dirPath = new Uri(directory);

                relativePath = Uri.UnescapeDataString(dirPath.MakeRelativeUri(file).ToString().Replace('/', Path.DirectorySeparatorChar));
            }

            return relativePath;
        }

        private static bool IsFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var attributes = File.GetAttributes(path);

            return (attributes & FileAttributes.Directory) != FileAttributes.Directory;
        }

        private IEnumerable<string> GetFilesInDirectoryRecursive(string directoryPath)
        {
            var result = new List<string>();

            var directories = Directory.GetDirectories(directoryPath);

            foreach (var subdirectoryPath in directories)
            {
                result.AddRange(this.GetFilesInDirectoryRecursive(subdirectoryPath));
            }

            result.AddRange(Directory.GetFiles(directoryPath));

            return result;
        }
    }
}
