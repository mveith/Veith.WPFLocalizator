using System;
using System.Windows;

namespace Veith.WPFLocalizator.EndToEndTests.Resources
{
    public class TestCSharpCode
    {
        public void UsingMessageBox()
        {
            MessageBox.Show("Message box text");
        }

        public void UsingConsole()
        {
            Console.WriteLine("Console write line text");
        }

        public void UsingMethodParameter()
        {
            this.MethodWithParameter("Parameter text");
        }

        private void MethodWithParameter(string parameter)
        {
        }
    }
}
