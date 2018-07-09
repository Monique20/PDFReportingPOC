using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Fields;
using PdfReportingPoc.UseCase.Pdf;

namespace PdfReportingPoc.UseCase.Tests.Pdf
{
    [TestFixture]
    public class PopulatePdfUseCaseTests
    {
        [Test]
        public void CreateFieldsUsecase_GivenValidFields_ShouldExcutePopulateAndReadOnly()
        {
            //Arrange
            var response = new PdfFieldsOperationsResponse
            {
                Output = new byte[512]
            };         
            var templateByte = new byte[362];
            var inputData = new List<PdfFields>
            {
                new PdfFields
                {
                    FieldName = "Surname",
                    FieldValue = "Tembe"
                }
            };

            var fieldsSubstitute = Substitute.For<IPdfOperations>();
            fieldsSubstitute.PopulateFieldValues(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>()).Returns(response);
            fieldsSubstitute.MarkFieldsReadOnly(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>()).Returns(response);

            var sut = new PopulatePdfUseCase(fieldsSubstitute);
            //Act
            var actual = sut.Execute(templateByte, inputData);
            //Assert
            fieldsSubstitute.Received().PopulateFieldValues(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>());
            fieldsSubstitute.Received().MarkFieldsReadOnly(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>());

        }

        [Test]
        public void CreateFieldsUsecase_GivenInvalidFields_ShouldNotExcutePopulateAndReadOnly()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var baseDirectory = TestContext.CurrentContext.TestDirectory + "\\TestData\\";
            var currentFilePath = Path.Combine(baseDirectory, fileName);
            var templateByte = File.ReadAllBytes(currentFilePath);
            var inputData = new List<PdfFields>
            {
                new PdfFields
                {
                    FieldName = "Firstname",
                    FieldValue = "Tembe"
                }
            };

            var pdfOperations = new PdfOperations();

            var sut = new PopulatePdfUseCase(pdfOperations);
            //Act
            var actual = sut.Execute(templateByte, inputData);
            //Assert
            actual.Output.Length.Should().Be(0);

        }
    }
}
