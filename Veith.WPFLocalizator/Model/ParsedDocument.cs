using System.Collections.Generic;
using System.Linq;

namespace Veith.WPFLocalizator.Model
{
    public class ParsedDocument
    {
        public DocumentInfo Document { get; set; }

        public IEnumerable<DocumentPart> Parts { get; set; }

        public bool IsDocumentUpdated()
        {
            return this.Parts != null && this.Parts.Any();
        }
    }
}
