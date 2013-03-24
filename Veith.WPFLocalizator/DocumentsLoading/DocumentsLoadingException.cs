using System;

namespace Veith.WPFLocalizator.DocumentsLoading
{
    [Serializable]
    public class DocumentsLoadingException : Exception
    {
        public DocumentsLoadingException(Exception innerException)
            : base("DocumentsLoadingExceptionMessage".Localize(), innerException)
        {
        }
    }
}
