using System.Collections.Generic;
using System.IO;
using Nancy.Metadata.Modules;
using Nancy.Swagger;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Domain.Report;
using Swagger.ObjectModel;

namespace PdfReporting.Web.Services.Pdf
{
    public class RenderReportMetadataModule : MetadataModule<PathItem>
    {
        public RenderReportMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(typeof(RenderReportRequest), typeof(RenderReportRequest));

            Describe["PostReport"] = desc => desc.AsSwagger(
                with => with.Operation(
                    op => op.OperationId("PostReport")
                        .Tag("Report")
                        .Summary("Returns pdf report")
                        .Description("Returns pdf report")
                        //specify the parameters of this api   
                        .Parameter(new Parameter
                        {
                            Name = "RenderReportRequest",
                            //specify the type of this parameter is path  
                            In = ParameterIn.Body,
                            //specify this parameter is required or not  
                            Required = true,
                            Description = "text to embed into code",
                            Default = new {
                                FileName= "DemoForm.pdf",
                                ListOfFields = "[]",
                                QrCodeData = new {
                                    Text = "unique id here",
                                    CheckSumEnabled = true
                                },
                                attachQrCodeRequest = new{
                                    PageNumber = 1,
                                    LowerLeftX = 100f,
                                    LowerLeftY = 100f,
                                    UpperRightX = 200f,
                                    UpperRightY = 200f,
                                },
                                Password = "siphenathi"
                            },
                            
                        })
                        .Response(r => r.Schema<Stream>(modelCatalog).Description("Returns report"))
                        .Response(404, r => r.Description("Can't find stuffs"))
                ));
        }
    }
}
