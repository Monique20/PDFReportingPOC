using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.UseCase.BarCode
{
    public class ExtractQrCodeUseCase : IExtractQrCodeUseCase
    {
        private readonly IBarCodes _barCodes;
        public ExtractQrCodeUseCase(IBarCodes barCodes)
        {
            _barCodes = barCodes;
        }
        public ExtractQrCodeResponse Execute(ExtractQrCodeRequest inputData)
        {
            var textFromQrCode = _barCodes.With_Image(inputData.Image)
                                 .Of_Type_QR_Code(inputData.CheckSumEnabled)
                                 .As_Png()
                                 .Extract_Text();

            var result = new ExtractQrCodeResponse { Text = textFromQrCode };
            return result;
        }
    }
}
