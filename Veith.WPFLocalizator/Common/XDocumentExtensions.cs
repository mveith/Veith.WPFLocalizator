using System.Text;
using System.Xml.Linq;

namespace Veith.WPFLocalizator.Common
{
    public static class XDocumentExtensions
    {
        public static string ToStringWithDeclaration(this XDocument document)
        {
            var result = new StringBuilder();

            result.AppendLine(document.Declaration.ToString());
            result.Append(document.ToString());

            return result.ToString();
        }
    }
}
