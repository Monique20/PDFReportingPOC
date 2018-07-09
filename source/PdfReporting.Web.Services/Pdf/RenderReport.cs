using System.IO;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using PdfReportingPoc.Domain.Report;

namespace PdfReporting.Web.Services.Pdf
{
    public class RenderReportModule : NancyModule
    {
        public readonly IRenderReportUseCase _renderReportUseCase;
        public RenderReportModule(IRenderReportUseCase renderReportUseCase)
        {

            Post("/api/RenderReport", parameters => ExecuteRenderReportEndPoint(), null, "PostReport");
            _renderReportUseCase = renderReportUseCase;

        }

        private Response ExecuteRenderReportEndPoint()
        {
            var request = this.Bind<RenderReportRequest>();
            var response = new Response();
            if (string.IsNullOrWhiteSpace(request.FileName))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            var fileBytes = _renderReportUseCase.Execute(request);
            if (fileBytes.Length == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            var stream = new MemoryStream(fileBytes);

            response = new StreamResponse(() => stream, MimeTypes.GetMimeType(request.FileName));
            response.AsAttachment(request.FileName);
            return response;
        }
    }
}
