using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsParsing
{
    public interface IDocumentParser
    {
        ParsedDocument ParseDocument(DocumentInfo document);
    }
}
