namespace PdfReportingPoc.Domain.BarCode
{
    public interface IAttachQrCodeUseCase
    {
        AttachQrCodeResponse Execute(AttachQrCodeRequest request);
    }
}
