using System.IO;
using Nancy.Metadata.Modules;
using Nancy.Swagger;
using Swagger.ObjectModel;

namespace PdfReporting.Web.Services.Health
{
    public class HealthMetadataModule : MetadataModule<PathItem>
    {
        public HealthMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(typeof(HealthResponse), typeof(HealthResponse));

            Describe["GetHealth"] = desc => desc.AsSwagger(
                with => with.Operation(
                    op => op.OperationId("GetHealth")
                        .Tag("Health")
                        .Summary("Returns a message if the endpoint can be reached")
                        .Description("Returns a message if the endpoint can be reached")
                        .Response(r => r.Schema<HealthResponse>(modelCatalog).Description("Here is the health status"))
                        .Response(404, r => r.Description("Can't find stuffs"))
                ));

            Describe["GetDateTime"] = desc => desc.AsSwagger(
                with => with.Operation(
                    op => op.OperationId("GetDateTime")
                        .Tag("Health")
                        .Summary("Returns a message with server datetime")
                        .Description("Returns a message with server datetime")
                        //specify the parameters of this api   
                        .Parameter(new Parameter
                        {
                            Name = "isLongFormat",
                            //specify the type of this parameter is path  
                            In = ParameterIn.Path,
                            //specify this parameter is required or not  
                            Required = true,
                            Description = "returns long format of datetime if true"
                        })
                        .Response(r => r.Schema<HealthResponse>(modelCatalog).Description("Returns timestamp from server"))
                        .Response(404, r => r.Description("Can't find stuffs"))
                ));

            Describe["GetManual"] = desc => desc.AsSwagger(
                with => with.Operation(
                    op => op.OperationId("GetManaul")
                        .Tag("Health")
                        .Summary("Returns a pdf of the system manual")
                        .Description("Returns a pdf of the system manual")
                        .ProduceMimeType("application/pdf")
                        .Response(r => r.Schema<Stream>(modelCatalog).Description("Pdf manual"))
                        .Response(404, r => r.Description("Can't find stuffs"))
                ));
        }
    }
}
