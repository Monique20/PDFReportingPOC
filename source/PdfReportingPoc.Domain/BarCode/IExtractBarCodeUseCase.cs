namespace PdfReportingPoc.Domain.BarCode
{
    public interface IExtractBarCodeUseCase
    {
        ExtractBarCodeResponse Execute(ExtractBarCodeRequest inputData);
    }
}