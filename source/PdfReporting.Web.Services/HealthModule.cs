using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;

namespace PdfReporting.Web.Services
{
    public class HealthModule : NancyModule
    {
        public HealthModule()
        {
            Get["/api/health"] = parameters =>
            {
                var result = new { Status = "OK" };
                return Response.AsJson(result);
            };
        }
    }
}
