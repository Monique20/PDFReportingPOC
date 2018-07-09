using System;
using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.Testing;
using NSubstitute;
using NUnit.Framework;
using PdfReporting.Web.Services.Pdf;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Domain.Report;

namespace PdfReporting.Web.Services.Tests.Pdf
{
    // todo : ensure each test follows the 3's State, Services, Sut
    [TestFixture]
    public class RenderReportTests
    {
        [Test]
        public void ExecuteRenderReportEndPoint_GivenValidData_ShouldReturnStatusCode200()
        {
            // Arrange
            var request = new RenderReportRequest()
            {
                FileName = "BootCampForm-v2.pdf",
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateQrCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                attachQrCodeRequest = new AttachQrCodeRequest
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = "password"
            };

            var renderReportSubstitute = Substitute.For<IRenderReportUseCase>();
            renderReportSubstitute.Execute(Arg.Any<RenderReportRequest>()).Returns(new byte[999]);

            var browser = CreateBrowser(renderReportSubstitute);

            // Act
            var result = browser.Post("/api/RenderReport", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.JsonBody(request);

            });

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);   
        }

        [Test]
        public void ExecuteRenderReportEndPoint_GivenValidData_ShouldReturnFileBytesThatAreGreaterThan0()
        {
            // Arrange
            var request = new RenderReportRequest()
            {
                FileName = "BootCampForm-v2.pdf",
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateQrCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                attachQrCodeRequest = new AttachQrCodeRequest
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = "password"
            };
            var fileName = "BootCampForm-v2.pdf";

            var renderReportSubstitute = Substitute.For<IRenderReportUseCase>();
            var fileBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", fileName));
            renderReportSubstitute.Execute(Arg.Any<RenderReportRequest>()).Returns(fileBytes);

            var browser = CreateBrowser(renderReportSubstitute);

            // Act
            var result = browser.Post("/api/RenderReport", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.JsonBody(request);

            });

            // Assert
            var expected = 94853;
            var stream = result.Result.Body.AsStream();
            Assert.AreEqual(expected, stream.Length);

        }

        [Test]
        public void ExecuteRenderReportEndPoint_GivenValidData_ExecuteMethodShouldBeCalled()
        {
            // Arrange
            var listOfFields = GetValidFieldNames();
            var request = new RenderReportRequest()
            {
                FileName = "BootCampForm-v2.pdf",
                ListOfFields = listOfFields,
                QrCodeData = new CreateQrCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                attachQrCodeRequest = new AttachQrCodeRequest
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = "password"
            };

            var renderReportSubstitute = Substitute.For<IRenderReportUseCase>();

            var browser = CreateBrowser(renderReportSubstitute);

            // Act
            browser.Post("/api/RenderReport", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.JsonBody(request);

            });         

            // Assert
            renderReportSubstitute.Received().Execute(Arg.Any<RenderReportRequest>());
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ExecuteRenderReportEndPoint_GivenEmptyOrNullFileName_ShouldReturnStatusCode400(string fileName)
        {
            // Arrange
            var listOfFields = GetValidFieldNames();
            var request = new RenderReportRequest()
            {
                FileName = fileName,
                ListOfFields = listOfFields,
                QrCodeData = new CreateQrCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                attachQrCodeRequest = new AttachQrCodeRequest
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = "password"
            };

            var renderReportSubstitute = Substitute.For<IRenderReportUseCase>();

            var browser = CreateBrowser(renderReportSubstitute);

            // Act
            var result = browser.Post("/api/RenderReport", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.JsonBody(request);

            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Result.StatusCode);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ExecuteRenderReportEndPoint_GivenEmptyOrNullFileName_ExecuteMethodShouldNotBeCalled(string fileName)
        {
            // Arrange
            var listOfFields = GetValidFieldNames();
            var request = new RenderReportRequest()
            {
                FileName = fileName,
                ListOfFields = listOfFields,
                QrCodeData = new CreateQrCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                }
            };

            var renderReportSubstitute = Substitute.For<IRenderReportUseCase>();

            var browser = CreateBrowser(renderReportSubstitute);

            // Act
            browser.Post("/api/RenderReport", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.JsonBody(request);

            });

            // Assert
            renderReportSubstitute.DidNotReceive().Execute(Arg.Any<RenderReportRequest>());
        }

        [Test]
        public void ExecuteRenderReportEndPoint_GivenInvalidFileName_ShouldReturnStatusCode404()
        {
            // Arrange
            var listOfFields = GetValidFieldNames();
            var request = new RenderReportRequest()
            {
                FileName = "me.pdf",
                ListOfFields = listOfFields,
                QrCodeData = new CreateQrCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                attachQrCodeRequest = new AttachQrCodeRequest
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = "password"
            };

            var renderReportSubstitute = Substitute.For<IRenderReportUseCase>();

            var browser = CreateBrowser(renderReportSubstitute);

            // Act
            var result = browser.Post("/api/RenderReport", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.JsonBody(request);

            });

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.Result.StatusCode);
        }

        public static Browser CreateBrowser(IRenderReportUseCase renderReport)
        {
            var browser = new Browser(with =>
            {
                with.Module<RenderReportModule>();
                with.Dependency(renderReport);
            });
            return browser;
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


    }
}
