using System;
using System.Collections.Generic;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UserInteraction
{
    public interface IUserInteraction
    {
        void UserEditingKeys(IEnumerable<KeyAndValueItem> items, string fileName);

        string SelectDirectory();

        IEnumerable<string> SelectFiles();

        void ShowMessage(string message);

        void ShowErrorMessage(Exception exception);

        string SelectProject();

        string SaveProject(string serializedConfiguration, string actualProjectName);

        void SaveLastProject(string serializedConfiguration);
    }
}