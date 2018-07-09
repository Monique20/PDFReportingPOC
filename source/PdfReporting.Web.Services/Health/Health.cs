using System;
using System.IO;
using System.Reflection;
using Nancy;
using Nancy.Responses;
using Response = Nancy.Response;

namespace PdfReporting.Web.Services.Health
{
    public class HealthModule : NancyModule
    {
        public HealthModule()
        {
            Get("/api/health", _ => HealthResponse(), null, "GetHealth");
            Get("/api/datetime/{isLongFormat}", _ => FetchDateTime((bool)_.isLongFormat), null, "GetDateTime");
            Get("/api/manual", _ => FetchManual(), null, "GetManual");
        }

        private Response FetchManual()
        {
            var fileName = "Manual.pdf";
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(basePath, "Health", fileName);
            var bytes = File.ReadAllBytes(filePath);

            // note: No need for using statement since this descopes the stream before the response can be sent
            var stream = new MemoryStream(bytes);
            var response = new StreamResponse(() => stream, MimeTypes.GetMimeType(fileName));
            response.AsAttachment(fileName);
            return response;
        }

        private Response HealthResponse()
        {
            var result = new HealthResponse{ Response = "I am alive!" };
            return Response.AsJson(result);
        }
        
        private Response FetchDateTime(bool isLongFormat)
        {
            var dateTimeString = $"{DateTime.Now.ToShortDateString()}  {DateTime.Now.ToShortTimeString()}";
            if (isLongFormat)
            {
                dateTimeString = $"{DateTime.Now.ToLongDateString()}  {DateTime.Now.ToLongTimeString()}";
            }
            var result = new HealthResponse{ Response = dateTimeString };
            return Response.AsJson(result);
        }
    }

}
