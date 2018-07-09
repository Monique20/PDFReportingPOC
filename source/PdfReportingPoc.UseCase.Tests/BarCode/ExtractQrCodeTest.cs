using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.UseCase.BarCode;

namespace PdfReportingPoc.UseCase.Tests.BarCode
{
    [TestFixture]
    public class ExtractQrCodeTest
    {
        [Test]
        public void Execute_GivenImageAndCheckSum_ShouldExtractAndReturnATextFromQrCode()
        {
            //Arrange 
            var qrCodePath = AppDomain.CurrentDomain.BaseDirectory + "TestData\\barcode3.png";
            var inputData = new ExtractQrCodeRequest
            {
                Image = File.ReadAllBytes(qrCodePath),
                CheckSumEnabled = true
            };

            var barCode = Substitute.For<IBarCodes>();
            var sut = new ExtractQrCodeUseCase(barCode);

            //Act
            var actual = sut.Execute(inputData);

            //Assert
            barCode.Received().With_Image(Arg.Any<byte[]>())
                              .Of_Type_QR_Code(Arg.Any<bool>())
                              .As_Png().Extract_Text();
        }
    }
}
