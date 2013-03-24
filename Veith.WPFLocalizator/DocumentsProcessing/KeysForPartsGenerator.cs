using System.Collections.Generic;
using System.Linq;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public interface IKeysForPartsGenerator
    {
        void GenerateKeysForParts(IEnumerable<DocumentPart> parts, ParsedDocument document);

        string GetResultKeyForPart(DocumentPart part);
    }

    public class KeysForPartsGenerator : IKeysForPartsGenerator
    {
        private readonly IUserInteraction userInteraction;
        private readonly IResourcesKeysGenerator keysGenerator;

        private ParsedDocument document;
        private Dictionary<DocumentPart, string> keysForParts;

        public KeysForPartsGenerator(
            IUserInteraction userInteraction,
            IResourcesKeysGenerator keysGenerator)
        {
            this.userInteraction = userInteraction;
            this.keysGenerator = keysGenerator;
        }

        public void GenerateKeysForParts(IEnumerable<DocumentPart> parts, ParsedDocument document)
        {
            this.document = document;
            this.keysForParts = new Dictionary<DocumentPart, string>();

            foreach (var part in parts)
            {
                this.FindKeyForPart(part);
            }

            this.UserEditingKeys();
        }

        public string GetResultKeyForPart(DocumentPart part)
        {
            return this.keysForParts[part];
        }

        private void UserEditingKeys()
        {
            var keysAndValues = this.keysForParts.Select(kp => new KeyAndValueItem(kp.Value, kp.Key.Value)).ToArray();

            this.userInteraction.UserEditingKeys(keysAndValues, this.document.Document.FileName);

            var originalKeysForParts = this.keysForParts.ToArray();

            this.keysForParts.Clear();

            for (int i = 0; i < keysAndValues.Length; i++)
            {
                var part = originalKeysForParts[i].Key;
                part.IsSelectedForLocalization = keysAndValues[i].IsSelectedForLocalization;
                this.keysForParts.Add(part, keysAndValues[i].Key);
            }
        }

        private void FindKeyForPart(DocumentPart part)
        {
            var key = this.CreateKeyForPart(part);
            var originalValue = part.Value;

            this.keysForParts.Add(part, key);
        }

        private string CreateKeyForPart(DocumentPart part)
        {
            return this.keysGenerator.CreateKey(this.document.Document.FileName, part);
        }
    }
}
