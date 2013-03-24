using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public interface IResourcesKeysGenerator
    {
        string CreateKey(string documentName, DocumentPart testPart);
    }
}
