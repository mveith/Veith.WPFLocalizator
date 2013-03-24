using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.Common
{
    public static class XElementExtensions
    {
        public static void UpdateFromPart(this XElement element, DocumentPart part)
        {
            var attributeForPart = GetAttributeWithLocalName(element, part.Name);

            if (attributeForPart != null)
            {
                attributeForPart.SetValue(part.Value);
            }
            else if (element.GetElementNameWithoutParentName() == part.Name)
            {
                element.SetValue(part.Value);
            }
        }

        public static string GetElementParentNameWithDot(this XElement attributeElement)
        {
            return attributeElement.Parent != null ? attributeElement.Parent.Name.LocalName + "." : null;
        }

        public static string GetElementNameWithoutParentName(this XElement attributeElement)
        {
            var parent = attributeElement.GetElementParentNameWithDot();

            if (string.IsNullOrEmpty(parent))
            {
                return default(string);
            }

            return attributeElement.Name.LocalName.Replace(attributeElement.GetElementParentNameWithDot(), string.Empty);
        }

        public static IEnumerable<XElement> GetAllElementsInXElement(this XElement element)
        {
            var elements = new List<XElement>();

            elements.Add(element);
            foreach (var child in element.Elements())
            {
                elements.AddRange(GetAllElementsInXElement(child));
            }

            return elements;
        }

        private static XAttribute GetAttributeWithLocalName(XElement element, string name)
        {
            return element.Attributes().FirstOrDefault(a => a.Name.LocalName == name);
        }
    }
}
