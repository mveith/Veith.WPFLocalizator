using MahApps.Metro.Controls;

namespace Veith.WPFLocalizator.View
{
    public partial class MessageBoxWindow : MetroWindow
    {
        public MessageBoxWindow()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        public string Message { get; set; }

        public static void Show(string title, string message)
        {
            var window = new MessageBoxWindow()
            {
                Title = title,
                Message = message
            };

            window.UpdateOwner();

            window.ShowDialog();
        }
    }
}
