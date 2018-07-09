using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using PdfReportingPoc.Fields;
using System.Collections.Generic;
using System.IO;
using PdfReportingPoc.Domain.Pdf;

namespace PdfReportingPoc.Data.Tests.Fields
{
    [TestFixture]
    public class PdfOperationsTests
    {
        [Test]
        public void MarkFieldsReadOnly_GivenNullOrEmptyFields_ShouldReturnNoFieldsToMark()
        {
            //Arrange
            var fields = EmptyOrNullFieldName();
            var filename = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(filename);

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.MarkFieldsReadOnly(fileBytes, fields);

            //Assert
            var expected = new byte[0];
            var expectedMessage = $"{""} cannot be found";
            Assert.AreEqual(expected, actual.Output);
            Assert.AreEqual(expectedMessage, actual.ErrorMessage);
        }

        [Test]
        public void MarkFieldsReadOnly_GivenInvalidFields_ShouldReturnFieldDoesNotExist()
        {
            //Arrange
            var fields = NonExistingField();
            var filename = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(filename);

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.MarkFieldsReadOnly(fileBytes, fields);

            //Assert
            var expected = new byte[0];
            var expectedMessage = $"{"Firstame"} cannot be found";
            Assert.AreEqual(expected, actual.Output);
            Assert.AreEqual(expectedMessage, actual.ErrorMessage);
        }

        [Test]
        public void MarkFieldsReadOnly_GivenValidFilenameAndFields_ShouldReturnNewFileWithSpecifiedFieldsMarkedAsReadOnly()
        {
            //Arrange
            var fields = GetValidFieldNames();
            var filename = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(filename);

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.MarkFieldsReadOnly(fileBytes, fields);

            //Assert
            var expected = fileBytes.Length;
            actual.Output.Length.Should().BeLessThan(expected);
        }

        [Test]
        public void MarkFieldsReadOnly_GivenMultipleFieldValues_ShouldMarkCellFieldsAsReadOnly()
        {
            //Arrange
            var fields = GetFieldValuesForCellForm();
            var filename = "multi-cell.pdf";
            var fileBytes = GetFileBytes(filename);

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.MarkFieldsReadOnly(fileBytes, fields);

            //Assert
            var expected = fileBytes.Length;
            actual.Output.Length.Should().BeLessThan(expected);
        }

        [Test]
        public void PopulatePdfFields_GivenAnExistingPdfAndFieldsToPopulate_ShouldPopulatedFormFieldsWithNewValues()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(fileName);
            var listOfFields = GetValidFieldNames();

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PopulateFieldValues(fileBytes, listOfFields);

            //Assert
            var expected = fileBytes.Length;
            actual.Output.Length.Should().BeGreaterThan(expected);

        }

        [Test]
        public void PopulatePdfFields_GivenMultipleFieldValues_ShouldPopulatedFormFieldsInCellsAndCheckboxes()
        {
            //Arrange
            var fileName = "multi-cell.pdf";
            var fileBytes = GetFileBytes(fileName);
            var listOfFields = GetFieldValuesForCellForm();

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PopulateFieldValues(fileBytes, listOfFields);

            //Assert
            var expected = fileBytes.Length;
            actual.Output.Length.Should().BeLessThan(expected);

        }

        [Test]
        public void PopulatePdfFields_GivenAnExistingPdfAndInvalidFieldName_ShouldReturnZeroBytes()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(fileName);
            var listOfFields = EmptyOrNullFieldName();

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PopulateFieldValues(fileBytes, listOfFields);

            //Assert
            var expected = new byte[0];
            var expectedMessage = $"{""} cannot be found";
            Assert.AreEqual(expected, actual.Output);
            Assert.AreEqual(expectedMessage, actual.ErrorMessage);

        }

        [Test]
        public void PopulatePdfFields_GivenAnExistingPdfAndNonExistingField_ShouldReturnBeTheSameAsOriginalPdf()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(fileName);
            var listOfFields = NonExistingField();

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PopulateFieldValues(fileBytes, listOfFields);

            //Assert
            actual.Output.Length.Should().IsSameOrEqualTo(fileBytes);

        }

        [Test]
        public void PopulatePdfFields_GivenFormFieldValueGreaterThanMaxLengthOfField_ShouldReturnErrorMessage()
        {
            //Arrange
            var fileName = "multi-cell.pdf";
            var fileBytes = GetFileBytes(fileName);
            var listOfFields = new List<PdfFields>
            {
                    new PdfFields
                    {
                        FieldName = "Text1",
                        FieldValue = "123456789123456789"
                    }
            };

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PopulateFieldValues(fileBytes, listOfFields);
      
            //Assert
            var expectedBytes = new byte[0];
            var expectedMessage = "Text1 has exceeded its maximum value by 2";
            actual.Output.Length.Should().Be(expectedBytes.Length);
            actual.ErrorMessage.Should().Be(expectedMessage);

        }

        [Test]
        public void PasswordProtect_GivenValidPassword_ShouldEncryptFile()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(fileName);
            var password = "user";

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PasswordProtect(fileBytes, password);

            //Assert
            actual.Length.Should().BeLessThan(fileBytes.Length);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void PasswordProtect_GivenEmptyOrNullPassword_ShouldReturnFalse(string password)
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var fileBytes = GetFileBytes(fileName);

            var sut = CreatePdfOperations();

            //Act
            var actual = sut.PasswordProtect(fileBytes, password);

            //Assert
            actual.Length.Should().Be(0);
        }

        private static PdfOperations CreatePdfOperations()
        {
            return new PdfOperations();
        }

        private static List<PdfFields> GetValidFieldNames()
        {
            return new List<PdfFields>
            {
                new PdfFields
                {
                    FieldName="FirstName",
                    FieldValue="Bob"
                },
                new PdfFields
                {
                    FieldName="Surname",
                    FieldValue="Smith"
                },
                new PdfFields
                {
                    FieldName="Tax",
                    FieldValue="1500"
                },
                new PdfFields
                {
                    FieldName="DateOfBirth",
                    FieldValue="1975-05-08"
                },
                new PdfFields
                {
                    FieldName="AccommodationCost",
                    FieldValue="8200"
                },
                new PdfFields
                {
                    FieldName="GrossSalary",
                    FieldValue="80000"
                },
                new PdfFields
                {
                    FieldName="CellPhoneCost",
                    FieldValue="500"
                },
                new PdfFields
                {
                    FieldName="CreditCardCost",
                    FieldValue="500"
                },
                new PdfFields
                {
                    FieldName="OtherDebtCost",
                    FieldValue="4000"
                }
            };
        }

        private static List<PdfFields> GetFieldValuesForCellForm()
        {
            return new List<PdfFields>
            {
                    new PdfFields
                    {
                        FieldName = "Text3",
                        FieldValue = "Smith"
                    },
                    new PdfFields
                    {
                        FieldName = "Text1",
                        FieldValue = "Bob"
                    },
                    new PdfFields
                    {
                        FieldName = "Text2",
                        FieldValue = "25"
                    },
                    new PdfFields
                    {
                        FieldName = "Text4",
                        FieldValue = "05"
                    },
                    new PdfFields
                    {
                        FieldName = "Text5",
                        FieldValue = "1975"
                    },
                    new PdfFields
                    {
                        FieldName = "Text6",
                        FieldValue = "750525023008"
                    },
                    new PdfFields
                    {
                        FieldName = "Text7",
                        FieldValue = "Durban"
                    },
                    new PdfFields
                    {
                        FieldName = "Text8",
                        FieldValue = "586324790215"
                    },
                    new PdfFields
                    {
                        FieldName = "Text9",
                        FieldValue = "La Lucia"
                    },
                    new PdfFields
                    {
                        FieldName = "Text10",
                        FieldValue = "750525023008"
                    },
                    new PdfFields
                    {
                        FieldName = "Text11",
                        FieldValue = "Absa"
                    },
                    new PdfFields
                    {
                        FieldName = "Text12",
                        FieldValue = "La Lucia"
                    },
                    new PdfFields
                    {
                        FieldName = "Text13",
                        FieldValue = "400208218"
                    },
                    new PdfFields
                    {
                        FieldName = "Text14",
                        FieldValue = "Savings"
                    },
                    new PdfFields
                    {
                        FieldName = "Text15",
                        FieldValue = "UKZN"
                    },
                    new PdfFields
                    {
                        FieldName = "Text16",
                        FieldValue = "2015"
                    },
                    new PdfFields
                    {
                        FieldName = "Text17",
                        FieldValue = "Information Technology"
                    },
                    new PdfFields
                    {
                        FieldName="Check Box18",
                        FieldValue= "true"
                    },
                    new PdfFields
                    {
                        FieldName="Check Box20",
                        FieldValue= "true"
                    }
            };
        }

        private static List<PdfFields> NonExistingField()
        {
            return new List<PdfFields>
            {
                new PdfFields
                {
                    FieldName = "Firstame",
                    FieldValue = "Sindisiwe"
                }
            };
        }

        private static List<PdfFields> EmptyOrNullFieldName()
        {
            return new List<PdfFields>
            {
                new PdfFields
                {
                    FieldName = "",
                    FieldValue = "BlahBhah"
                },
                new PdfFields
                {
                    FieldName = " ",
                    FieldValue = "BlahBhah"
                },
                new PdfFields
                {
                    FieldName = null,
                    FieldValue = "BlahBhah"
                }
            };
        }

        private static byte[] GetFileBytes(string filename)
        {
            var baseDirectory = TestContext.CurrentContext.TestDirectory + "\\TestData\\";
            var currentFilePath = Path.Combine(baseDirectory, filename);
            var fileBytes = File.ReadAllBytes(currentFilePath);

            return fileBytes;
        }
    }
}
