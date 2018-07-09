namespace PdfReportingPoc.Domain.BarCode
{
    public interface ICreateBarCodeUseCase
    {
        CreateQrCodeResponse Execute(CreateQrCodeRequest requestData);

    }
}
