using System.Collections.Generic;
using MahApps.Metro.Controls;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.View
{
    public partial class UserEditingKeysWindow : MetroWindow
    {
        public UserEditingKeysWindow(UserEditingKeysWindowViewModel viewModel)
        {
            this.InitializeComponent();

            viewModel.Close = () => this.Close();

            this.DataContext = viewModel;
        }
    }

    public class UserEditingKeysWindowOpener : IUserEditingKeysWindow
    {
        public void ShowWindow(IEnumerable<KeyAndValueItem> items, string fileName)
        {
            var viewModel = new UserEditingKeysWindowViewModel(items, fileName);

            var window = new UserEditingKeysWindow(viewModel);
            window.UpdateOwner();

            window.ShowDialog();

            if (!viewModel.IsValid)
            {
                throw new UserAbortException();
            }
        }
    }
}
