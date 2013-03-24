using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class UserInteractionInProcessingDocument : DocumentsProcessingBase
    {
        private IResourcesDictionary dictionary;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();

            this.dictionary = Substitute.For<IResourcesDictionary>();
            this.dictionaries.Add(this.dictionary);

            this.keysGenerator.CreateKey(Arg.Any<string>(), Arg.Any<DocumentPart>()).Returns("KEY");
        }

        [TestMethod]
        public void IfProcessingDocumentThenUseUserInteractionKeysEditing()
        {
            this.parts.Add(new DocumentPart() { Name = "Text", Value = "Original value" });

            this.ProcessDocument();

            this.userInteraction.Received()
                .UserEditingKeys(Arg.Is<IEnumerable<KeyAndValueItem>>(i =>
                    i.Single() == new KeyAndValueItem("KEY", "Original value")), Arg.Any<string>());
        }

        [TestMethod]
        public void IfProcessingDocumentThenUseUserInteractionForAllPartsAtOnce()
        {
            this.parts.Add(new DocumentPart() { Name = "Text", Value = "Original value" });
            this.parts.Add(new DocumentPart() { Name = "Text", Value = "Original value 2" });

            this.ProcessDocument();

            this.userInteraction.Received(1)
                .UserEditingKeys(Arg.Is<IEnumerable<KeyAndValueItem>>(i => VerifyUserEditingAllPartsAtOnce(i)), Arg.Any<string>());
        }

        [TestMethod]
        public void IfProcessingDocumentThenUseUserEditedKeysInParts()
        {
            this.userInteraction.UserEditingKeys(Arg.Do<IEnumerable<KeyAndValueItem>>(i => i.First().Key = "EDITED_KEY"), Arg.Any<string>());

            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.EndsWith(this.resourceFileName + ":" + "EDITED_KEY" + this.suffix));
        }

        [TestMethod]
        public void IfUserSelectPartForLocalizingThenPartIsSelectedForLocalizing()
        {
            this.userInteraction.UserEditingKeys(Arg.Do<IEnumerable<KeyAndValueItem>>(i =>
            {
                i.First().IsSelectedForLocalization = true;
                i.Last().IsSelectedForLocalization = false;
            }), Arg.Any<string>());

            var testPart1 = new DocumentPart() { Name = "Text", Value = "Original value" };
            var testPart2 = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart1);
            this.parts.Add(testPart2);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart1.IsSelectedForLocalization);
            Assert.AreEqual(false, testPart2.IsSelectedForLocalization);
        }

        [TestMethod]
        public void IfPartIsNotSelectedThenDoNotInsertPartToDictionary()
        {
            this.userInteraction.UserEditingKeys(Arg.Do<IEnumerable<KeyAndValueItem>>(i =>
            {
                i.First().IsSelectedForLocalization = false;
            }), Arg.Any<string>());

            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            this.dictionary.DidNotReceive().Add(Arg.Any<string>(), Arg.Any<string>());
        }

        [TestMethod]
        public void IfUsingUserInteractionThenPassFileNameToWindow()
        {
            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            this.userInteraction.Received(1)
                .UserEditingKeys(Arg.Any<IEnumerable<KeyAndValueItem>>(), this.documentName);
        }

        private static bool VerifyUserEditingAllPartsAtOnce(IEnumerable<KeyAndValueItem> items)
        {
            var firstAreEquals = items.First() == new KeyAndValueItem("KEY", "Original value");
            var secondAreEquals = items.Last() == new KeyAndValueItem("KEY", "Original value 2");

            return firstAreEquals && secondAreEquals;
        }
    }
}
