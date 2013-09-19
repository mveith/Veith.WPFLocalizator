using System.Windows;

namespace Veith.WPFLocalizator
{
    public static class ControlsExtensions
    {
        public static void UpdateOwner(this Window window)
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow != null && window != mainWindow)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.Owner = mainWindow;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
    }
}
