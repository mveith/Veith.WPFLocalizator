using System.Windows;
using Veith.WPFLocalizator.UserInteraction;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.View
{
    public partial class NameSelectingWindow : Window
    {
        public NameSelectingWindow()
        {
            this.InitializeComponent();

            this.ViewModel = new NameSelectingWindowViewModel();
            this.DataContext = this.ViewModel;
        }

        public NameSelectingWindowViewModel ViewModel { get; private set; }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class NameSelectingWindowOpenener : INameSelectingWindow
    {
        public string GetName(string defaultName)
        {
            var window = new NameSelectingWindow();
            window.ViewModel.SelectedName = defaultName;
            window.UpdateOwner();

            var result = window.ShowDialog();

            if (result == true)
            {
                return window.ViewModel.SelectedName;
            }

            return null;
        }
    }
}