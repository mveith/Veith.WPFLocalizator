using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public class DefaultResourcesKeysGenerator : IResourcesKeysGenerator
    {
        public string CreateKey(string documentName, DocumentPart testPart)
        {
            return string.Format(
                "{0}{1}{2}",
                documentName,
                GetPartElementIdentifier(testPart),
                testPart.Name);
        }

        private static string GetPartElementIdentifier(DocumentPart testPart)
        {
            return !string.IsNullOrEmpty(testPart.ElementName) ? testPart.ElementName : testPart.ElementyTypeName;
        }
    }
}
