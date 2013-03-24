using System.Collections.Generic;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsLoading
{
    public interface IDocumentsLoader
    {
        IEnumerable<DocumentInfo> LoadDocumentsInDirectory(string directoryPath, string selectedExtension);
    }
}
