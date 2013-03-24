using System.Collections.Generic;
using System.Linq;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public class DocumentProcessor : IDocumentProcessor
    {
        private readonly string[] stringPartsNames;
        private readonly IKeysForPartsGenerator keysGenerator;

        private DocumentProcessorConfiguration configuration;
        private ParsedDocument document;
        private IEnumerable<IResourcesDictionary> dictionaries;

        public DocumentProcessor(IKeysForPartsGenerator keysGenerator, string[] stringPartsNames)
        {
            this.keysGenerator = keysGenerator;

            this.stringPartsNames = stringPartsNames;
        }

        public void ProcessDocument(
            ParsedDocument document,
            DocumentProcessorConfiguration configuration,
            IEnumerable<IResourcesDictionary> dictionaries)
        {
            this.document = document;
            this.configuration = configuration;
            this.dictionaries = dictionaries;

            var stringParts = this.GetDocumentStringParts();

            if (stringParts.Any())
            {
                this.keysGenerator.GenerateKeysForParts(stringParts, this.document);

                foreach (var part in stringParts)
                {
                    this.ProcessPart(part);
                }
            }
        }

        private bool IsStringPart(DocumentPart part)
        {
            return this.stringPartsNames.Contains(part.Name) && !part.Value.StartsWith("{");
        }

        private DocumentPart[] GetDocumentStringParts()
        {
            return this.document.Parts.Where(p => this.IsStringPart(p)).ToArray();
        }

        private void ProcessPart(DocumentPart part)
        {
            if (!part.IsSelectedForLocalization)
            {
                return;
            }

            var originalValue = part.Value;
            var key = this.keysGenerator.GetResultKeyForPart(part);

            part.Value = this.GetNewValueForPart(part, key);

            var valueToDictionary = this.configuration.NewValuePrefix + originalValue;

            foreach (var dictionary in this.dictionaries)
            {
                dictionary.Add(key, valueToDictionary);
            }
        }

        private string GetNewValueForPart(DocumentPart part, string key)
        {
            return string.Format(
                "{0} {1}:{2}:{3}{4}",
                this.configuration.Prefix,
                this.configuration.NamespaceValue,
                this.configuration.ResourceFileName,
                key,
                this.configuration.Suffix);
        }
    }
}
