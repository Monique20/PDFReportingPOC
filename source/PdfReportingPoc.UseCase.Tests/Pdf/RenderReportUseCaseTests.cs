using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.FileSystem;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Domain.Report;
using PdfReportingPoc.UseCase.Pdf;

namespace PdfReportingPoc.UseCase.Tests.Pdf
{
    [TestFixture]
    public class RenderReportUseCaseTests
    {
        [Test]
        public void Execute_GivenValidFilePath_LoadFileMethodShouldBeCalled()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                }
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[0]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            fieldsOperator.Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>())
                .Returns(new PdfFieldsOperationsResponse { Output = new byte[0] });
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            barCodeAttacher.Execute(Arg.Any<AttachBarCodeRequest>()).Returns(new AttachBarCodeResponse());
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);


            //Act
            sut.Execute(request);

            //Assert
            fileLoader.Received().LoadFile(Arg.Any<string>());
        }

        [Test]
        public void Execute_GivenValidFileAndFields_ExecuteMethodForPopulatingAndMarkingFieldsReadonlyShouldBeCalled()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                }
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[999]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            fieldsOperator.Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>())
                .Returns(new PdfFieldsOperationsResponse {Output = new byte[999]});
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            barCodeAttacher.Execute(Arg.Any<AttachBarCodeRequest>()).Returns(new AttachBarCodeResponse());
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);


            //Act
            sut.Execute(request);

            //Assert
            fieldsOperator.Received().Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>());
        }

        [Test]
        public void Execute_GivenValidInputData_ExecuteMethodForCreatingQrCodeShouldBeCalled()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                }
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[999]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            fieldsOperator.Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>())
                .Returns(new PdfFieldsOperationsResponse { Output = new byte[999] });
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            barCodeAttacher.Execute(Arg.Any<AttachBarCodeRequest>()).Returns(new AttachBarCodeResponse());
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);


            //Act
            sut.Execute(request);

            //Assert
            barCodeCreater.Received().Execute(Arg.Any<CreateBarCodeRequest>());
            
        }

        [Test]
        public void Execute_GivenValidRequest_ExecuteMethodForAttachingQrCodeShouldBeCalled()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";

            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                }
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[999]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            fieldsOperator.Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>())
                .Returns(new PdfFieldsOperationsResponse { Output = new byte[999] });
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            barCodeAttacher.Execute(Arg.Any<AttachBarCodeRequest>()).Returns(new AttachBarCodeResponse());
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);


            //Act
            sut.Execute(request);

            //Assert
            barCodeAttacher.Received().Execute(Arg.Any<AttachBarCodeRequest>());
        }

        [Test]
        public void Execute_GivenInvalidOrEmptyFile_AllTheMethodsAfterLoadFileShouldNotBeCalled()
        {
            //Arrange
            var fileName = "Me.pdf";

            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                }
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[0]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);

            //Act
            sut.Execute(request);

            //Assert
            fieldsOperator.DidNotReceive().Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>());
            barCodeCreater.DidNotReceive().Execute(Arg.Any<CreateBarCodeRequest>());
            barCodeAttacher.DidNotReceive().Execute(Arg.Any<AttachBarCodeRequest>());
            
        }
        
        [Test]
        public void Execute_GivenValidPassword_ExecuteMethodForPasswordProtectShouldBeCalled()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = "password"
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[999]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            fieldsOperator.Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>())
                .Returns(new PdfFieldsOperationsResponse { Output = new byte[999] });
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            barCodeAttacher.Execute(Arg.Any<AttachBarCodeRequest>()).Returns(new AttachBarCodeResponse());
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);

            //Act
            sut.Execute(request);

            //Assert
            passwordProtecter.Received().PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>());
        }


        [Test]
        public void Execute_GivenInvalidPassword_ExecuteMethodForPasswordProtectShouldNotBeCalled()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var request = new RenderReportRequest
            {
                FileName = fileName,
                ListOfFields = GetValidFieldNames(),
                QrCodeData = new CreateBarCodeRequest
                {
                    Text = Guid.NewGuid().ToString(),
                    CheckSumEnabled = true
                },
                Barcode = new Barcode
                {
                    LowerLeftX = 100,
                    LowerLeftY = 100,
                    UpperRightX = 200,
                    UpperRightY = 200,
                    PageNumber = 1
                },
                Password = ""
            };

            var fileLoader = Substitute.For<IFileSystemProvider>();
            fileLoader.LoadFile(Arg.Any<string>()).Returns(new byte[0]);
            var fieldsOperator = Substitute.For<IPopulatePdfUseCase>();
            fieldsOperator.Execute(Arg.Any<byte[]>(), Arg.Any<List<PdfFields>>())
                .Returns(new PdfFieldsOperationsResponse { Output = new byte[0] });
            var barCodeCreater = Substitute.For<ICreateBarCodeUseCase>();
            barCodeCreater.Execute(Arg.Any<CreateBarCodeRequest>()).Returns(new CreateBarCodeResponse());
            var barCodeAttacher = Substitute.For<IAttachBarCodeUseCase>();
            barCodeAttacher.Execute(Arg.Any<AttachBarCodeRequest>()).Returns(new AttachBarCodeResponse());
            var passwordProtecter = Substitute.For<IPdfOperations>();
            passwordProtecter.PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>()).Returns(new byte[0]);

            var sut = new RenderReportUseCase(fileLoader, fieldsOperator, barCodeCreater, barCodeAttacher, passwordProtecter);

            //Act
            sut.Execute(request);

            //Assert
            passwordProtecter.DidNotReceive().PasswordProtect(Arg.Any<byte[]>(), Arg.Any<string>());
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
