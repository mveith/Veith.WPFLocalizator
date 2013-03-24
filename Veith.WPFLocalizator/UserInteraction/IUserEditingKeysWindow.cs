using System.Collections.Generic;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UserInteraction
{
    public interface IUserEditingKeysWindow
    {
        void ShowWindow(IEnumerable<KeyAndValueItem> items, string fileName);
    }
}
