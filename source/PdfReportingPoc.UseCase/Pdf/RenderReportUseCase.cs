using System.Configuration;
using System.IO;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.FileSystem;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Domain.Report;

namespace PdfReportingPoc.UseCase.Pdf
{
    public class RenderReportUseCase : IRenderReportUseCase
    {
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly IPopulatePdfUseCase _pdfFieldsOperations;
        private readonly ICreateBarCodeUseCase _createBarCodeUseCase;
        private readonly IAttachQrCodeUseCase _attachQrCodeUseCase;
        private readonly IPdfOperations _passwordProtectOperation;

        public RenderReportUseCase(IFileSystemProvider fileSystemProvider, IPopulatePdfUseCase pdfFieldsOperations, ICreateBarCodeUseCase createBarCodeUseCase, IAttachQrCodeUseCase attachQrCodeUseCase, IPdfOperations passwordProtectOperation)
        {
            _fileSystemProvider = fileSystemProvider;
            _pdfFieldsOperations = pdfFieldsOperations;
            _createBarCodeUseCase = createBarCodeUseCase;
            _attachQrCodeUseCase = attachQrCodeUseCase;
            _passwordProtectOperation = passwordProtectOperation;
        }

        public byte[] Execute(RenderReportRequest request )
        {
            var path = GetFile(request.FileName);
            var fileLoader= _fileSystemProvider.LoadFile(path);
            if (fileLoader.Length == 0)
            {
                return new byte[0];
            }
            var fieldsOperator = _pdfFieldsOperations.Execute(fileLoader,request.ListOfFields);
            var barCodeCreator = _createBarCodeUseCase.Execute(request.QrCodeData);

            var attachQrCodeRequest = request.attachQrCodeRequest;
            attachQrCodeRequest.FileBytes = fieldsOperator.Output;
            attachQrCodeRequest.QrCodeBytes = barCodeCreator.BarCode;

            var barCodeAttacher = _attachQrCodeUseCase.Execute(attachQrCodeRequest);

            if (!string.IsNullOrWhiteSpace(request.Password)){
                var passwordProtecter = _passwordProtectOperation.PasswordProtect(barCodeAttacher.OutputFileBytes, request.Password);
                return passwordProtecter;
            }
            return barCodeAttacher.OutputFileBytes;
        }

        private static string GetFile(string fileName)
        {
            var templateLocation = ConfigurationManager.AppSettings["ReportTemplatesDirectory"];
            return Path.Combine(templateLocation, fileName);
        }
    }
}
