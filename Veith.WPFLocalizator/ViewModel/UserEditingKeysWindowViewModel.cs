using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Veith.WPFLocalizator.Infrastructure;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.ViewModel
{
    public class UserEditingKeysWindowViewModel
    {
        private string[] originalKeys;
        private bool isCanceled;

        public UserEditingKeysWindowViewModel(IEnumerable<KeyAndValueItem> items, string fileName)
        {
            this.SaveKeysCommand = new RelayCommand(() => this.Close(), () => this.AreKeysValid());
            this.RevertCommand = new RelayCommand(() => this.Revert(), () => this.ExistsChanges());
            this.CancelCommand = new RelayCommand(() => this.Cancel());

            this.Items = items;
            this.FileName = fileName;
            this.originalKeys = items.Select(i => i.Key).ToArray();
        }

        public ICommand SaveKeysCommand { get; set; }

        public ICommand RevertCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public IEnumerable<KeyAndValueItem> Items { get; set; }

        public string FileName { get; private set; }

        public Action Close { get; set; }

        public bool IsValid
        {
            get
            {
                return !this.isCanceled && this.AreKeysValid();
            }
        }

        private bool ExistsChanges()
        {
            var items = this.Items.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Key != this.originalKeys[i])
                {
                    return true;
                }
            }

            return false;
        }

        private void Revert()
        {
            var items = this.Items.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Key = this.originalKeys[i];
            }
        }

        private bool AreKeysValid()
        {
            var groupedKeys = this.Items.Where(i => i.IsSelectedForLocalization).GroupBy(i => i.Key);

            var allKeysSingle = groupedKeys.All(g => g.Count() == 1);

            return allKeysSingle;
        }

        private void Cancel()
        {
            this.isCanceled = true;

            this.Close();
        }
    }
}
