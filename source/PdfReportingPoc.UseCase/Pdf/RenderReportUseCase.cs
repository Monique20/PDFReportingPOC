using System.Configuration;
using System.IO;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.FileSystem;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Domain.Report;
using AutoMapper;

namespace PdfReportingPoc.UseCase.Pdf
{
    public class RenderReportUseCase : IRenderReportUseCase
    {
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly IPopulatePdfUseCase _pdfFieldsOperations;
        private readonly ICreateBarCodeUseCase _createBarCodeUseCase;
        private readonly IAttachBarCodeUseCase _attachQrCodeUseCase;
        private readonly IPdfOperations _passwordProtectOperation;

        public RenderReportUseCase(IFileSystemProvider fileSystemProvider, 
                                   IPopulatePdfUseCase populatePdfUseCase, 
                                   ICreateBarCodeUseCase createBarCodeUseCase, 
                                   IAttachBarCodeUseCase attachQrCodeUseCase, 
                                   IPdfOperations pdfOperations) 
        {
            _fileSystemProvider = fileSystemProvider;
            _pdfFieldsOperations = populatePdfUseCase;
            _createBarCodeUseCase = createBarCodeUseCase;
            _attachQrCodeUseCase = attachQrCodeUseCase;
            _passwordProtectOperation = pdfOperations;
        }
       
        public byte[] Execute(RenderReportRequest request )
        {
            var path = GetFile(request.FileName);
            var fileLoader = _fileSystemProvider.LoadFile(path);
            if (fileLoader.Length == 0)
            {
                return new byte[0];
            }
            var fieldsOperator = _pdfFieldsOperations.Execute(fileLoader, request.ListOfFields);
            var barCodeCreator = _createBarCodeUseCase.Execute(request.QrCodeData);

            var config = CreateMapperConfiguration(); 
            var attachBarCodeRequest = Map(request.Barcode, fieldsOperator, barCodeCreator, config); 
            var barCodeAttacher = _attachQrCodeUseCase.Execute(attachBarCodeRequest);

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var passwordProtecter = _passwordProtectOperation.PasswordProtect(barCodeAttacher.OutputFileBytes, request.Password);
                return passwordProtecter;
            }

            return barCodeAttacher.OutputFileBytes;
        }
        
        private static AttachBarCodeRequest Map(Barcode barcode, PdfFieldsOperationsResponse fieldsOperator, CreateBarCodeResponse barCodeCreator, IConfigurationProvider config)
        {
            var mapper = config.CreateMapper(); 
            var attachBarCodeRequest = mapper.Map<Barcode, AttachBarCodeRequest>(barcode);
            attachBarCodeRequest.FileBytes = fieldsOperator.Output;
            attachBarCodeRequest.QrCodeBytes = barCodeCreator.BarCode;
            return attachBarCodeRequest;
        } 

        private MapperConfiguration CreateMapperConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Barcode, AttachBarCodeRequest>();
            });
        }

        private static string GetFile(string fileName)
        {
            var templateLocation = ConfigurationManager.AppSettings["ReportTemplatesDirectory"];
            return Path.Combine(templateLocation, fileName);
        }
    }
}
