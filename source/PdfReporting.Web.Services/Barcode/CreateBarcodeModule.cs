using System.IO;
using Nancy;
using Nancy.Responses;
using PdfReportingPoc.Domain.BarCode;
using Response = Nancy.Response;

namespace PdfReporting.Web.Services.BarCode
{
    public class CreateBarcodeModule : NancyModule
    {
        public readonly ICreateBarCodeUseCase CreateBarCodeUseCase;
        public CreateBarcodeModule(ICreateBarCodeUseCase createBarCodeUseCase)
        {
            Get("/api/CreateQrCode/{text}/{checkSumEnabled}", parameters => ExecuteCreateBarcodeEndPoint(parameters), null, "GetBarcode");
            CreateBarCodeUseCase = createBarCodeUseCase;
        }

        private Response ExecuteCreateBarcodeEndPoint(dynamic parameters)
        {
            var fileName = "barcode.png";
            if (string.IsNullOrWhiteSpace(parameters.text))
            {
                return HttpStatusCode.BadRequest;
            }
            var inputData = new CreateBarCodeRequest
            {
                Text = parameters.text,
                CheckSumEnabled = parameters.checkSumEnabled
            };

            var result = CreateBarCodeUseCase.Execute(inputData);        
            var stream = new MemoryStream(result.BarCode);
            var response = new StreamResponse(() => stream, MimeTypes.GetMimeType(fileName));
            response.AsAttachment(fileName);
            return response;
        }    
    }    
}

