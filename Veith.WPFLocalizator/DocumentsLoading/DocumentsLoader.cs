using System.Collections.Generic;
using System.Linq;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsLoading
{
    public class DocumentsLoader : IDocumentsLoader
    {
        private readonly IFileSystemWrapper fileSystemWrapper;
        private readonly DocumentInfoFactory documentFactory;

        private string selectedExtension;

        public DocumentsLoader(IFileSystemWrapper fileSystemWrapper)
        {
            this.fileSystemWrapper = fileSystemWrapper;
            this.documentFactory = new DocumentInfoFactory(this.fileSystemWrapper);
        }

        public IEnumerable<DocumentInfo> LoadDocumentsInDirectory(string directoryPath, string selectedExtension)
        {
            try
            {
                return this.TryLoadDocumentsInDirectory(directoryPath, selectedExtension);
            }
            catch (System.Exception e)
            {
                throw new DocumentsLoadingException(e);
            }
        }

        private IEnumerable<DocumentInfo> TryLoadDocumentsInDirectory(string directoryPath, string selectedExtension)
        {
            this.selectedExtension = selectedExtension;

            var requestedFilesPaths = this.GetRequestedFilePaths(directoryPath);

            var documents = requestedFilesPaths.Select(filePath => this.CreateDocumentInfoForFile(filePath));

            return documents.ToArray();
        }

        private DocumentInfo CreateDocumentInfoForFile(string filePath)
        {
            return this.documentFactory.CreateDocumentInfoForFile(filePath);
        }

        private IEnumerable<string> GetRequestedFilePaths(string directoryPath)
        {
            var filePaths = this.fileSystemWrapper.GetFilesWithExtensionInDirectory(directoryPath, this.selectedExtension);

            return filePaths;
        }
    }
}