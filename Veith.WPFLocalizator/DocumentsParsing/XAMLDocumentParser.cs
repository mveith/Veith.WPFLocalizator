using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.DocumentsParsing.XAMLParsingTools;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsParsing
{
    public class XAMLDocumentParser : IDocumentParser
    {
        private const string ExpectedExtension = "XAML";
        private readonly DocumentAttributeFactory attributesFactory;

        public XAMLDocumentParser()
        {
            this.attributesFactory = new DocumentAttributeFactory();
        }

        public ParsedDocument ParseDocument(DocumentInfo document)
        {
            if (document.Extension != ExpectedExtension)
            {
                throw new InvalidOperationException("FileHasNotExpectedFormatExceptionMessage".Localize());
            }

            return new ParsedDocument()
            {
                Document = document,
                Parts = this.GetPartsInDocument(document)
            };
        }

        private static DocumentPart CreatePartFromAttribute(DocumentAttribute attribute)
        {
            var nameAttribute = attribute.Parent.Attribute("Name");
            string elementName = nameAttribute != null ? nameAttribute.Value : (string)null;

            return new DocumentPart()
            {
                Name = attribute.Name,
                Value = attribute.Value,
                ElementyTypeName = attribute.Parent.Name.LocalName,
                ElementName = elementName,
                ElementIndex = attribute.ElementIndex
            };
        }

        private IEnumerable<DocumentPart> GetPartsInDocument(DocumentInfo document)
        {
            var attributes = this.GetAttributesInDocument(document);
            var elements = attributes.Select(a => CreatePartFromAttribute(a));

            return elements.ToArray();
        }

        private IEnumerable<DocumentAttribute> GetAttributesInDocument(DocumentInfo document)
        {
            var xml = XDocument.Parse(document.Content);

            var allElements = xml.Root.GetAllElementsInXElement();

            return this.attributesFactory.CreateFromElements(allElements);
        }
    }
}
