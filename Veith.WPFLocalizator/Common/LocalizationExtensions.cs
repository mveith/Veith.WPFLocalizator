using WPFLocalizeExtension.Extensions;

namespace Veith.WPFLocalizator
{
    public static class LocalizationExtensions
    {
        private const string Prefix = "Veith.WPFLocalizator.Resources:Resources:";

        public static string Localize(this string key)
        {
            if (!key.StartsWith(Prefix))
            {
                key = Prefix + key;
            }

            var localizedValue = string.Empty;

            if (new LocTextExtension(key).ResolveLocalizedValue(out localizedValue))
            {
                return localizedValue;
            }

            return "???";
        }
    }
}
