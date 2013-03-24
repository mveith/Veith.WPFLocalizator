using System;

namespace Veith.WPFLocalizator.DocumentsSerializing
{
    [Serializable]
    public class DocumentsSerializingException : Exception
    {
        public DocumentsSerializingException(Exception innerException)
            : base("DocumentsSerializingExceptionMessage".Localize(), innerException)
        {
        }
    }
}