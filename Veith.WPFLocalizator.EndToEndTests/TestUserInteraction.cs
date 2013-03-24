using System.Collections.Generic;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.EndToEndTests
{
    public class TestUserInteraction : IUserInteraction
    {
        private readonly string userEditingPostfix;

        public TestUserInteraction(string userEditingPostfix)
        {
            this.userEditingPostfix = userEditingPostfix;
        }

        public void UserEditingKeys(IEnumerable<KeyAndValueItem> items, string fileName)
        {
            foreach (var item in items)
            {
                item.Key = item.Key + this.userEditingPostfix;
            }
        }

        public string SelectDirectory()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> SelectFiles()
        {
            throw new System.NotImplementedException();
        }

        public void ShowMessage(string message)
        {
            System.Console.WriteLine(message);
        }

        public void ShowErrorMessage(System.Exception exception)
        {
            System.Console.WriteLine(exception.ToString());
        }

        public string SelectProject()
        {
            return this.SelectedProject;
        }

        public string SelectedProject { get; set; }

        public string SaveProject(string serializedConfiguration, string actualName)
        {
            throw new System.NotImplementedException();
        }


        public void SaveLastProject(string serializedConfiguration)
        {
            throw new System.NotImplementedException();
        }
    }
}
