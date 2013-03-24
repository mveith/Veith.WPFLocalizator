namespace Veith.WPFLocalizator.Model
{
    public class KeyAndValueItem
    {
        public KeyAndValueItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.IsSelectedForLocalization = true;
        }

        public string Key { get; set; }

        public string Value { get; private set; }

        public bool IsSelectedForLocalization { get; set; }

        public static bool operator ==(KeyAndValueItem first, KeyAndValueItem second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(KeyAndValueItem first, KeyAndValueItem second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as KeyAndValueItem;

            return other.Key.Equals(this.Key) && other.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode() ^ this.Value.GetHashCode();
        }
    }
}
