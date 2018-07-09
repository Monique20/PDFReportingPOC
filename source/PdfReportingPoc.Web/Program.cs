using System;
using Nancy.Hosting.Self;

namespace PdfReportingPoc.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostConfigs = new HostConfiguration
            {
                UrlReservations =
                {
                    CreateAutomatically = true
                }
            };

            var uri = new Uri("http://localhost:1234");
            using (var host = new NancyHost( hostConfigs, uri))
            {
                host.Start();
                Console.ReadKey();
            }

        }

    }
}
