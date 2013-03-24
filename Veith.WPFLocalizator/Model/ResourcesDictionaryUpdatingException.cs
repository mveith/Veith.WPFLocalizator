using System;

namespace Veith.WPFLocalizator.Model
{
    [Serializable]
    public class ResourcesDictionaryUpdatingException : Exception
    {
        public ResourcesDictionaryUpdatingException(Exception innerException)
            : base("ResourcesDictionaryUpdatingExceptionMessage".Localize(), innerException)
        {
        }
    }
}
