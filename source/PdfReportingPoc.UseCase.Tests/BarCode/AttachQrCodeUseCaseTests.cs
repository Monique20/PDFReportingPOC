using System.IO;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using PdfReportingPoc.BarCode;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.UseCase.BarCode;

namespace PdfReportingPoc.UseCase.Tests.BarCode
{
    [TestFixture]
    public class AttachQrCodeUseCaseTests
    {
        [Test]
        public void Execute_GivenRequestWithPdfFileBytesAndQrCodeBytes_ShouldReturnOutputFileBytes()
        {
            //Arrange 
            var request = new AttachQrCodeRequest
            {
                FileBytes = GetFileBytes("BootCampForm-v2.pdf"),
                QrCodeBytes = GetFileBytes("barcode3.png")
            };

            var attachBarCodes = new BarCodeAttachmentOperations();

            var sut = CreateAttachQrCodeUseCase(attachBarCodes);

            //Act
            var actual = sut.Execute(request);

            //Assert
            actual.OutputFileBytes.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void Execute_GivenRequestToAttachEndPoint_ShouldCallAttachMethod()
        {
            //Arrange 
            var request = new AttachQrCodeRequest
            {
                FileBytes = GetFileBytes("BootCampForm-v2.pdf"),
                QrCodeBytes = GetFileBytes("barcode3.png")
            };

            var attachBarCodes = Substitute.For<IBarCodeAttachmentOperations>();

            var sut = CreateAttachQrCodeUseCase(attachBarCodes);

            //Act
            var actual = sut.Execute(request);

            //Assert
            attachBarCodes.Received().Attach(Arg.Any<AttachQrCodeRequest>());
        }

        private IAttachQrCodeUseCase CreateAttachQrCodeUseCase(IBarCodeAttachmentOperations barCodeAttachmentOperations)
        {
            return new AttachQrCodeUseCase(barCodeAttachmentOperations);
        }

        private byte[] GetFileBytes(string name)
        {
            var basePath = TestContext.CurrentContext.TestDirectory;
            var folder = "TestData";
            var path = Path.Combine(basePath, folder, name);
            var fileBytes = File.ReadAllBytes(path);
            return fileBytes;
        }
    }
}
