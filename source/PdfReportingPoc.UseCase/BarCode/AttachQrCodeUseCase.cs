using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.UseCase.BarCode
{
    public class AttachQrCodeUseCase: IAttachQrCodeUseCase
    {
        private readonly IBarCodeAttachmentOperations _barCodeAttachmentOperations;

        public AttachQrCodeUseCase(IBarCodeAttachmentOperations barCodeAttachmentOperations)
        {
            _barCodeAttachmentOperations = barCodeAttachmentOperations;
        }
        public AttachQrCodeResponse Execute(AttachQrCodeRequest request)
        {
            var outputFileBytes = _barCodeAttachmentOperations.Attach(request);

            var attachQrCodeResponse = new AttachQrCodeResponse
            {
                OutputFileBytes = outputFileBytes
            };

            return attachQrCodeResponse;
        }
    }
}
