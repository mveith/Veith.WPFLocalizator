using System.Globalization;
using System.Linq;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public class ResourcesKeysFromPartValueGenerator : IResourcesKeysGenerator
    {
        public string CreateKey(string documentName, DocumentPart part)
        {
            return documentName + GetValueKeyPart(part);
        }

        private static string GetValueKeyPart(DocumentPart part)
        {
            var valueWithCapitalizedFirstLetters = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(part.Value.RemoveDiacritics());

            var valuewWithoutNonLettersSymbols = new string(valueWithCapitalizedFirstLetters.Where(c => char.IsLetter(c)).ToArray());

            var cleanValue = valuewWithoutNonLettersSymbols.Replace(" ", string.Empty);
            return cleanValue;
        }

    }
}
