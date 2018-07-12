using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.UseCase.BarCode
{
    public class ExtractBarCodeUseCase : IExtractBarCodeUseCase
    {
        private readonly IBarCodes _barCodes;
        public ExtractBarCodeUseCase(IBarCodes barCodes)
        {
            _barCodes = barCodes;
        }
        public ExtractBarCodeResponse Execute(ExtractBarCodeRequest inputData)
        {
            var textFromQrCode = _barCodes.With_Image(inputData.Image)
                                 .Of_Type_QR_Code(inputData.CheckSumEnabled)
                                 .As_Png()
                                 .Extract_Text();

            var result = new ExtractBarCodeResponse { Text = textFromQrCode };
            return result;
        }
    }
}
