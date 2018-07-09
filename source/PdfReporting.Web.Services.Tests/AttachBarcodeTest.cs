using System;
using System.IO;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using PdfReportingPoc.BarCode;
using PdfReportingPoc.Domain.BarCode;

namespace PdfReporting.Web.Services.Tests
{
    // todo : this belongs in the folder BarCode
    [TestFixture]
    public class AttachBarcodeTest
    {
        [Test]
        public void WhenTheresRequestToAttachEndPoint_ShouldReturnAttachmentOfPDFWithBarcode()
        {
            //Arrange 
            var createBarcodes = new BarCodes();
            var browser = CreateBrowser(createBarcodes);

            //Act
            var result = browser.Get($"/api/AttachQRBarcode", with =>
            {
                with.HttpRequest();
            });

            var pdfPath = AppDomain.CurrentDomain.BaseDirectory+ "Document\\Boot camp form - v2.pdf";
            var file = File.ReadAllBytes(pdfPath);

            var actual = JsonConvert.DeserializeObject<AttachBarcodeResponse>(result.Result.Body.AsString());

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
            Assert.AreNotEqual(file, actual.PdfWithBarcode);
        }

        public static Browser CreateBrowser(IBarCodes createBarcodes)
        {
            var browser = new Browser(with =>
            {
                with.Module<AttachBarCodeModule>();
                with.Dependency(createBarcodes);
            });
            return browser;
        }
    }
}
