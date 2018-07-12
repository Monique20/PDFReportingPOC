using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Swagger.Services;
using Nancy.TinyIoc;
using PdfReportingPoc.BarCode;
using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.FileSystem;
using PdfReportingPoc.Domain.Pdf;
using PdfReportingPoc.Domain.Report;
using PdfReportingPoc.Fields;
using PdfReportingPoc.FileSystem;
using PdfReportingPoc.UseCase.BarCode;
using PdfReportingPoc.UseCase.Pdf;
using Swagger.ObjectModel;

namespace PdfReportingPoc.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            SwaggerMetadataProvider.SetInfo("Pdf Reporting Poc", "v0.9.2", "Provides pdf reporting functionality", new Contact()
            {
                EmailAddress = "travisf@sahomeloans.co.za",
                Name = "G-Team"
            }, "#");

            container.Register(typeof(IBarCodes), typeof(BarCodes));
            container.Register(typeof(ICreateBarCodeUseCase), typeof(CreateBarCodeUseCase));
            container.Register(typeof(IPopulatePdfUseCase), typeof(PopulatePdfUseCase));
            container.Register(typeof(IRenderReportUseCase), typeof(RenderReportUseCase));
            container.Register(typeof(IFileSystemProvider), typeof(FileSystemProvider));
            container.Register(typeof(IPdfOperations), typeof(PdfOperations));
            container.Register(typeof(IBarCodeAttachmentOperations), typeof(BarCodeAttachmentOperations));
            container.Register(typeof(IBarCodeAttachmentOperations), typeof(BarCodeAttachmentOperations));
            container.Register(typeof(IAttachBarCodeUseCase), typeof(AttachBarCodeUseCase));

            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions); nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("swagger-ui"));
        }

    }
}
