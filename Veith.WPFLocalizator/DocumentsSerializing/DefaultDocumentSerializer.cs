using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsSerializing
{
    public class DefaultDocumentSerializer : IDocumentSerializer
    {
        private IFileSystemWrapper fileSystemWrapper;

        public DefaultDocumentSerializer(IFileSystemWrapper fileSystemWrapper)
        {
            this.fileSystemWrapper = fileSystemWrapper;
        }

        public void SerializeDocument(ParsedDocument document)
        {
            try
            {
                this.TrySerializeDocument(document);
            }
            catch (System.Exception e)
            {
                throw new DocumentsSerializingException(e);
            }
        }

        private static string GetContentFromUpdatedDocument(ParsedDocument document)
        {
            var content = document.Document.Content;

            var xml = XDocument.Parse(content);
            var elements = xml.Root.GetAllElementsInXElement();

            foreach (var part in document.Parts.Where(p => p.IsSelectedForLocalization))
            {
                UpdateElementsForPart(part, elements);
            }

            return xml.ToString();
        }

        private static void UpdateElementsForPart(DocumentPart part, IEnumerable<XElement> elements)
        {
            var elementsForPart = elements.Where((e, i) => i == part.ElementIndex).ToArray();

            foreach (var element in elementsForPart)
            {
                element.UpdateFromPart(part);
            }
        }

        private void TrySerializeDocument(ParsedDocument document)
        {
            if (!document.Parts.Any(p => p.IsSelectedForLocalization))
            {
                return;
            }

            var content = document.Document.Content;

            if (document.IsDocumentUpdated())
            {
                content = GetContentFromUpdatedDocument(document);
            }

            this.fileSystemWrapper.SaveFileContent(document.Document.FilePath, content);
        }
    }
}