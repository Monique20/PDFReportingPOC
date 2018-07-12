using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.UseCase.BarCode
{
   public class CreateBarCodeUseCase : ICreateBarCodeUseCase
    {
        private readonly IBarCodes _barCodes;
        public CreateBarCodeUseCase(IBarCodes barCodes)
        {
            _barCodes = barCodes;
        }
        public CreateBarCodeResponse Execute(CreateBarCodeRequest inputData)
        {
            var createBarCode = _barCodes.With_Text(inputData.Text)
                .With_Default_Resolution()
                .With_Default_Dimension()
                .Of_Type_QR_Code(inputData.CheckSumEnabled)
                .As_Png()
                .Create();

            var result = new CreateBarCodeResponse { BarCode = createBarCode};
            return result;
        }
    }
}

