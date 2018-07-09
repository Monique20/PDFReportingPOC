namespace PdfReportingPoc.Domain.BarCode
{
    public interface IExtractQrCodeUseCase
    {
        ExtractQrCodeResponse Execute(ExtractQrCodeRequest inputData);
    }
}