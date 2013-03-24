using System;

namespace Veith.WPFLocalizator.UserInteraction
{
    [Serializable]
    public class UserAbortException : Exception
    {
        public UserAbortException()
            : base("UserAbortExceptionMessage".Localize())
        {
        }
    }
}
