using System;
using System.Globalization;
using System.Text;

namespace Veith.WPFLocalizator
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (text == string.Empty)
            {
                return text;
            }

            int charIndex = 0;

            var chars = CreateCharsWithoutDiacritics(text, ref charIndex);

            return CreateStringFromChars(chars, charIndex);
        }

        private static char[] CreateCharsWithoutDiacritics(string text, ref int charIndex)
        {
            char[] chars = new char[text.Length];

            text = text.Normalize(NormalizationForm.FormD);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (IsNotNonSpacingMark(c))
                {
                    chars[charIndex++] = c;
                }
            }
            return chars;
        }

        private static string CreateStringFromChars(char[] chars, int charIndex)
        {
            return new string(chars, 0, charIndex).Normalize(NormalizationForm.FormC);
        }

        private static bool IsNotNonSpacingMark(char c)
        {
            return CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark;
        }
    }
}
