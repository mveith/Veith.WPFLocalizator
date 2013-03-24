using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsSerializing
{
    public interface IDocumentSerializer
    {
        void SerializeDocument(ParsedDocument document);
    }
}
