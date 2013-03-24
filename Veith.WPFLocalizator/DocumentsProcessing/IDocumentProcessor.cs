using System.Collections.Generic;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public interface IDocumentProcessor
    {
        void ProcessDocument(
            ParsedDocument document, 
            DocumentProcessorConfiguration configuration,
            IEnumerable<IResourcesDictionary> dictionaries);
    }
}
