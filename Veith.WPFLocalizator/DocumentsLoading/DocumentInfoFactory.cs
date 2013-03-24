using System.IO;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsLoading
{
    internal class DocumentInfoFactory
    {
        private readonly IFileSystemWrapper fileSystemWrapper;

        private string filePath;

        public DocumentInfoFactory(IFileSystemWrapper fileSystemWrapper)
        {
            this.fileSystemWrapper = fileSystemWrapper;
        }

        public DocumentInfo CreateDocumentInfoForFile(string filePath)
        {
            this.filePath = filePath;

            return new DocumentInfo
            {
                FilePath = filePath,
                Content = this.GetFileContent(),
                Extension = this.GetFileExtension(),
                FileName = this.GetFileName()
            };
        }

        private string GetFileName()
        {
            return Path.GetFileNameWithoutExtension(this.filePath);
        }

        private string GetFileExtension()
        {
            return Path.GetExtension(this.filePath).TrimStart('.').ToUpper();
        }

        private string GetFileContent()
        {
            return this.fileSystemWrapper.GetFileContent(this.filePath);
        }
    }
}
