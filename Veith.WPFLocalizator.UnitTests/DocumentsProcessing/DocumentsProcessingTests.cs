using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class DocumentsProcessingTests : DocumentsProcessingBase
    {
        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestMethod]
        public void IfProcessingDocumentThenReplaceAllTextAttributeValueInDocumentParts()
        {
            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            var testPart2 = new DocumentPart() { Name = "Text", Value = "Original value 2" };
            this.parts.Add(testPart);
            this.parts.Add(testPart2);

            this.ProcessDocument();

            Assert.AreNotEqual("Original value", testPart.Value);
            Assert.AreNotEqual("Original value 2", testPart2.Value);
        }

        [TestMethod]
        public void IfProcessingDocumentThenNotReplaceForExampleWidthAttributes()
        {
            var testPart = new DocumentPart() { Name = "Width", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual("Original value", testPart.Value);
        }

        [TestMethod]
        public void IfProcessingDocumentThenReplaceContentAttributeValue()
        {
            var testPart = new DocumentPart() { Name = "Content", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreNotEqual("Original value", testPart.Value);
        }

        [TestMethod]
        public void IfProcessingDocumentThenNewValueStartsWithPrefix()
        {
            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.StartsWith(this.prefix));
        }

        [TestMethod]
        public void IfProcessingDocumentThenNewValueEndsWithSuffix()
        {
            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.EndsWith(this.suffix));
        }

        [TestMethod]
        public void IfProcessingDocumentThenNewValueAfterPreffixContainsNamespace()
        {
            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.StartsWith(this.prefix + " " + this.namespaceValue));
        }

        [TestMethod]
        public void IfProcessingDocumentThenNewValueAfterNamespaceContainsResourceFileName()
        {
            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.Contains(this.namespaceValue + ":" + this.resourceFileName));
        }

        [TestMethod]
        public void ForKeyGeneratingUseResourcesKeysGenerator()
        {
            this.keysGenerator.CreateKey(Arg.Any<string>(), Arg.Any<DocumentPart>()).Returns("AutoResourceKey");

            var testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.EndsWith(this.resourceFileName + ":" + "AutoResourceKey" + this.suffix));
            this.keysGenerator.Received().CreateKey(this.documentName, testPart);
        }

        [TestMethod]
        public void IfProcessingDocumentThenReplaceTitleAttributeValue()
        {
            var testPart = new DocumentPart() { Name = "Title", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreNotEqual("Original value", testPart.Value);
        }

        [TestMethod]
        public void IfProcessingProcessedDocumentThenDoNotReplaceAnchorsToResources()
        {
            var testPart = new DocumentPart() { Name = "Title", Value = "{lng:LocText atd.}" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual("{lng:LocText atd.}", testPart.Value);
        }

        [TestMethod]
        public void IfChangeConfigurationBeforeProcessingThenUseNewConfiguration()
        {
            this.configuration.Prefix = "{lng:NewLocText";

            var testPart = new DocumentPart() { Name = "Title", Value = "VALUE" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreEqual(true, testPart.Value.StartsWith("{lng:NewLocText"));
        }

        [TestMethod]
        public void IfUserKeysEditingThrowsExceptionThenThrowException()
        {
            var testPart = new DocumentPart() { Name = "Header", Value = "Original value" };
            this.parts.Add(testPart);

            var exception = new Exception();

            try
            {
                this.userInteraction.WhenForAnyArgs(ui => ui.UserEditingKeys(Arg.Any<IEnumerable<KeyAndValueItem>>(), Arg.Any<string>()))
                        .Do((x) => { throw exception; });

                this.ProcessDocument();

                Assert.Fail("Sem to nesmí dojít!");
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception, e);
            }
        }

        [TestMethod]
        public void IfProcessingDocumentThenReplaceHeaderAttributeValue()
        {
            var testPart = new DocumentPart() { Name = "Header", Value = "Original value" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            Assert.AreNotEqual("Original value", testPart.Value);
        }

        [TestMethod]
        public void IfProcessedDocumentHasNoStringPartsThenNotShowEditingKeysWindow()
        {
            var testPart = new DocumentPart() { Name = "Header", Value = "{Binding Header}" };
            this.parts.Add(testPart);

            this.ProcessDocument();

            this.userInteraction.DidNotReceive().UserEditingKeys(Arg.Any<IEnumerable<KeyAndValueItem>>(), Arg.Any<string>());
        }
    }
}
