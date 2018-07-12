namespace PdfReportingPoc.Domain.BarCode
{
    public interface IAttachBarCodeUseCase
    {
        AttachBarCodeResponse Execute(AttachBarCodeRequest request);
    }
}
