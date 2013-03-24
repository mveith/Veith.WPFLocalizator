namespace Veith.WPFLocalizator.DocumentsParsing.XAMLParsingTools
{
    public class DocumentAttribute
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public System.Xml.Linq.XElement Parent { get; set; }

        public int ElementIndex { get; set; }
    }
}
