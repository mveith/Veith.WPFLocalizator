using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.Common
{
    [TestClass]
    public class XElementExtensionsTests
    {
        [TestMethod]
        public void IfUpdatingElementFromPartThenSelectAttributeWithLocalNameEqualsWithPartName()
        {
            var element = new XElement("Window");
            element.Add(new XAttribute(XName.Get("Width", "x"), "123"));

            var part = new DocumentPart() { Name = "Width", Value = "555" };

            XElementExtensions.UpdateFromPart(element, part);

            Assert.AreEqual("555", element.FirstAttribute.Value);
        }

        [TestMethod]
        public void IfUpdatingAttributeElementThenSetElementContent()
        {
            
        }
    }
}
