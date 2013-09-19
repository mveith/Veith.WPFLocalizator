using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.UnitTests.ViewModel
{
    [TestClass]
    public class UserEditingKeysWindowViewModelTests
    {
        private List<KeyAndValueItem> items;
        private UserEditingKeysWindowViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            this.items = new List<KeyAndValueItem>();

            this.CreateViewModel();
        }

        [TestMethod]
        public void IfKeysAreDuplicatedThenSaveKeysCommandCannotBeExecuted()
        {
            this.items.Add(new KeyAndValueItem("KEY", "VALUE1"));
            this.items.Add(new KeyAndValueItem("KEY", "VALUE2"));

            Assert.AreEqual(false, this.viewModel.SaveKeysCommand.CanExecute(null));
        }

        [TestMethod]
        public void IfKeysAreNotDuplicatedThenSaveKeysCommandCanBeExecuted()
        {
            this.items.Add(new KeyAndValueItem("KEY1", "VALUE1"));
            this.items.Add(new KeyAndValueItem("KEY2", "VALUE2"));

            Assert.AreEqual(true, this.viewModel.SaveKeysCommand.CanExecute(null));
        }

        [TestMethod]
        public void SaveKeysCommandExecutionCallCloseAction()
        {
            var closeActionCalled = false;

            this.viewModel.Close = () => closeActionCalled = true;

            this.viewModel.SaveKeysCommand.Execute(null);

            Assert.AreEqual(true, closeActionCalled);
        }

        [TestMethod]
        public void IfKeysAreNotChangedThenCancelCommandCannotBeExecuted()
        {
            this.items.Add(new KeyAndValueItem("KEY", "VALUE"));

            this.CreateViewModel();

            Assert.AreEqual(false, this.viewModel.RevertCommand.CanExecute(null));
        }

        [TestMethod]
        public void IfKeysAreChangedThenCancelCommandCanBeExecuted()
        {
            this.items.Add(new KeyAndValueItem("KEY", "VALUE"));

            this.CreateViewModel();

            this.viewModel.Items.First().Key = "NEWKEY";

            Assert.AreEqual(true, this.viewModel.RevertCommand.CanExecute(null));
        }

        [TestMethod]
        public void AfterRevertCommandExecutionRevertKeysToOriginalValues()
        {
            this.items.Add(new KeyAndValueItem("KEY", "VALUE"));

            this.CreateViewModel();

            this.viewModel.Items.First().Key = "NEWKEY";

            this.viewModel.RevertCommand.Execute(null);

            Assert.AreEqual("KEY", this.viewModel.Items.First().Key);
        }

        [TestMethod]
        public void IfKeysAreNotDuplicatedThenCanSave()
        {
            this.items.Add(new KeyAndValueItem("KEY1", "VALUE1"));
            this.items.Add(new KeyAndValueItem("KEY2", "VALUE2"));

            Assert.AreEqual(true, this.viewModel.SaveKeysCommand.CanExecute(null));
        }

        [TestMethod]
        public void CancelCommandExecutionCallCloseAction()
        {
            var closeActionCalled = false;

            this.viewModel.Close = () => closeActionCalled = true;

            this.viewModel.CancelCommand.Execute(null);

            Assert.AreEqual(true, closeActionCalled);
        }

        [TestMethod]
        public void IfKeysAreInvalidButAreNotSelectedForLocalizationThenCanSave()
        {
            this.items.Add(new KeyAndValueItem("KEY1", "VALUE1"));
            this.items.Add(new KeyAndValueItem("KEY1", "VALUE2") { IsSelectedForLocalization = false });

            Assert.AreEqual(true, this.viewModel.SaveKeysCommand.CanExecute(null));
        }

        [TestMethod]
        public void IfSaveThenIsValidIsTrue()
        {
            this.viewModel.SaveKeysCommand.Execute(null);

            Assert.AreEqual(true, this.viewModel.IsValid);
        }

        [TestMethod]
        public void IfIsNotSavedThenIsValidIsFalse()
        {
            Assert.AreEqual(false, this.viewModel.IsValid);
        }

        private void CreateViewModel()
        {
            this.viewModel = new UserEditingKeysWindowViewModel(this.items, "FileName") { Close = () => { } };
        }
    }
}
