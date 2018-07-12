namespace PdfReportingPoc.Domain.BarCode
{
    public interface ICreateBarCodeUseCase
    {
        CreateBarCodeResponse Execute(CreateBarCodeRequest requestData);

    }
}
