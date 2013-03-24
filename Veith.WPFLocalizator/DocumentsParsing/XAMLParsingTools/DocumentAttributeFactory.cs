using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Veith.WPFLocalizator.Common;

namespace Veith.WPFLocalizator.DocumentsParsing.XAMLParsingTools
{
    public class DocumentAttributeFactory
    {
        public IEnumerable<DocumentAttribute> CreateFromElements(IEnumerable<XElement> elements)
        {
            var elementsWithIndexes = elements.Select((e, i) => new ElementWithIndex() { Element = e, Index = i }).ToArray();
            var notAttributeElements = elementsWithIndexes.Where(e => !IsAttributeElement(e.Element));
            var attributeElements = elementsWithIndexes.Where(e => IsAttributeElement(e.Element));

            var allAttributes = GetAllAttributes(notAttributeElements, attributeElements);

            return allAttributes;
        }

        private static IEnumerable<DocumentAttribute> GetAllAttributes(
            IEnumerable<ElementWithIndex> notAttributeElements,
            IEnumerable<ElementWithIndex> attributeElements)
        {
            var attributes = new List<DocumentAttribute>();
            foreach (var element in notAttributeElements)
            {
                foreach (var attribute in element.Element.Attributes())
                {
                    attributes.Add(new DocumentAttribute()
                    {
                        Name = attribute.Name.LocalName,
                        Value = attribute.Value,
                        Parent = attribute.Parent,
                        ElementIndex = element.Index
                    });
                }
            }

            foreach (var attributeElement in attributeElements)
            {
                attributes.Add(new DocumentAttribute()
                {
                    Name = attributeElement.Element.GetElementNameWithoutParentName(),
                    Value = attributeElement.Element.Value,
                    Parent = attributeElement.Element,
                    ElementIndex = attributeElement.Index
                });
            }

            return attributes;
        }

        private static bool IsAttributeElement(XElement element)
        {
            var parentName = element.GetElementParentNameWithDot();

            return parentName != null ? element.Name.LocalName.Contains(parentName) && !string.IsNullOrEmpty(element.Value) : false;
        }

        private class ElementWithIndex
        {
            public XElement Element { get; set; }

            public int Index { get; set; }
        }
    }
}
