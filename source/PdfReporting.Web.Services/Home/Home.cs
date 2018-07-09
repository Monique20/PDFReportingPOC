using System;
using Nancy;

namespace PdfReporting.Web.Services.Home
{
    [Obsolete]
    public class Home : NancyModule
    {
        public Home()
        {
            Get("/", _ => Response.AsRedirect("/swagger-ui"));

            Get("/swagger-ui", _ =>
            {
                var url = $"{Request.Url.BasePath}/api-docs";
                return View["doc", url];
            });
        }
    }
}