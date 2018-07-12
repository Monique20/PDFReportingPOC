using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.UseCase.BarCode
{
    public class AttachBarCodeUseCase: IAttachBarCodeUseCase
    {
        private readonly IBarCodeAttachmentOperations _barCodeAttachmentOperations;

        public AttachBarCodeUseCase(IBarCodeAttachmentOperations barCodeAttachmentOperations)
        {
            _barCodeAttachmentOperations = barCodeAttachmentOperations;
        }

        public AttachBarCodeResponse Execute(AttachBarCodeRequest request)
        {
            var outputFileBytes = _barCodeAttachmentOperations.Attach(request);

            var attachQrCodeResponse = new AttachBarCodeResponse
            {
                OutputFileBytes = outputFileBytes
            };

            return attachQrCodeResponse;
        }
    }
}
