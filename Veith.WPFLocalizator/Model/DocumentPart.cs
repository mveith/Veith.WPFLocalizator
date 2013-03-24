namespace Veith.WPFLocalizator.Model
{
    [System.Diagnostics.DebuggerDisplay("{Name} - {Value}")]
    public class DocumentPart
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string ElementName { get; set; }

        public string ElementyTypeName { get; set; }

        public int ElementIndex { get; set; }

        public bool IsSelectedForLocalization { get; set; }
    }
}
