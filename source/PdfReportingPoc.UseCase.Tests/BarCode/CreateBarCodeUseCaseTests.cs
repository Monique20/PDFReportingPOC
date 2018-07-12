using NSubstitute;
using NUnit.Framework;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.UseCase.BarCode;
using System;

namespace PdfReportingPoc.UseCase.Tests.BarCode
{
    
    [TestFixture]
    public class CreateBarCodeUseCaseTests
    {
        [Test]
        public void Execute_GivenValidInputData_CreateMethodForCreatingQrCodeShouldBeCalled()
        {
            //Arrange
            var inputData = new CreateBarCodeRequest
            {
                Text = Guid.NewGuid().ToString(),
                CheckSumEnabled = true
            };

            var barCodeSubtitute = Substitute.For<IBarCodes>();
            var sut = new CreateBarCodeUseCase(barCodeSubtitute);

            //Act
            var actual = sut.Execute(inputData);

            //Assert
            barCodeSubtitute.Received()
                            .With_Text(Arg.Any<string>())
                            .With_Default_Resolution()
                            .With_Default_Dimension()
                            .Of_Type_QR_Code(Arg.Any<bool>())
                            .As_Png()
                            .Create();
        }
    }
}
