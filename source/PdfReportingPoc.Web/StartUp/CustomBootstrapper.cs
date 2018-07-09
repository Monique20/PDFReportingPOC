using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Testing;
using Nancy.TinyIoc;

namespace PdfReportingPoc.Web.StartUp
{
    public class CustomBootstrapper : ConfigurableBootstrapper
    {
        public CustomBootstrapper(Action<ConfigurableBootstrapperConfigurator> actions) : base(actions) { }

        protected override void RequestStartup(TinyIoCContainer requestContainer, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((nancyContext, exception) =>
            {
                var errorBytes = Encoding.UTF8.GetBytes(exception.Message);
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ContentType = "text/plain",
                    Contents = stream => stream.Write(errorBytes, 0, errorBytes.Length)
                };
            });

            base.RequestStartup(requestContainer, pipelines, context);
        }
    }
}
