using Nancy.Metadata.Modules;
using Nancy.Swagger;
using PdfReportingPoc.Domain.BarCode;
using Swagger.ObjectModel;

namespace PdfReporting.Web.Services.BarCode
{
    public class CreateBarcodeMetadataModule : MetadataModule<PathItem>
    {
        public CreateBarcodeMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(typeof(CreateBarCodeResponse), typeof(CreateBarCodeResponse));

            Describe["GetBarcode"] = desc => desc.AsSwagger(
                with => with.Operation(
                    op => op.OperationId("GetBarcode")
                        .Tag("BarCode")
                        .Summary("Returns bytes of barcode")
                        .Description("Returns bytes of barcode")
                        //specify the parameters of this api   
                        .Parameter(new Parameter
                        {
                            Name = "text",
                            //specify the type of this parameter is path  
                            In = ParameterIn.Path,
                            //specify this parameter is required or not  
                            Required = true,
                            Description = "text to embed into code"
                        })
                        .Parameter(new Parameter
                        {
                            Name = "checkSumEnabled",
                            //specify the type of this parameter is path  
                            In = ParameterIn.Path,
                            //specify this parameter is required or not  
                            Required = true,
                            Description = "enable checksum on barcode"
                        })
                        .Response(r => r.Schema<CreateBarCodeResponse>(modelCatalog).Description("Returns barcode bytes"))
                        .Response(404, r => r.Description("Can't find stuffs"))
                ));
        }
    }
}
