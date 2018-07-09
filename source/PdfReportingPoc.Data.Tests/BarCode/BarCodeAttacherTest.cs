using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using PdfReportingPoc.BarCode;
using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.Data.Tests.BarCode
{
    [TestFixture]
    public class BarCodeAttachmentOperationTests 
    {
        [Test]
        public void Attach_GivenPdfAndQrCode_ShouldReturnFileWithAttachedQrCode()
        {
            //Arrange
            var request = GetRequestInput();

            var sut = GetBarCodeAttachmentOperations();

            //Act
            var actual = sut.Attach(request);

            //Assert
            actual.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void Attach_GivenFileAndQrCode_OutputFileSizeShouldBeGreaterThanOriginalFileSize()
        {
            //Arrange
            var request = GetRequestInput();

            var sut = GetBarCodeAttachmentOperations();
            
            //Act
            var actual = sut.Attach(request);

            //Assert
            actual.Length.Should().BeGreaterThan(request.FileBytes.Length);
        }

        private static BarCodeAttachmentOperations GetBarCodeAttachmentOperations()
        {
            return new BarCodeAttachmentOperations();
        }

        private static AttachQrCodeRequest GetRequestInput()
        {
            var pdfPath = AppDomain.CurrentDomain.BaseDirectory + "TestData\\BootCampForm-v2.pdf";
            var qrBarCodePath = AppDomain.CurrentDomain.BaseDirectory + "TestData\\barcode3.png";

            var pdfBytes = File.ReadAllBytes(pdfPath);
            var qrBarCodeBytes = File.ReadAllBytes(qrBarCodePath);

            return new AttachQrCodeRequest
            {
                FileBytes = pdfBytes,
                QrCodeBytes = qrBarCodeBytes,
                PageNumber = 1,
                LowerLeftX = 100f,
                LowerLeftY = 100f,
                UpperRightX = 200f,
                UpperRightY = 200f
            };
        }

    }
}
