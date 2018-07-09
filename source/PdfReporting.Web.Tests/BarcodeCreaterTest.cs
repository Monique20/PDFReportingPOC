using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery.Utility;
using NUnit.Framework;
using PdfReportingPoc.Web.StartUp;


namespace PdfReporting.Web.Tests
{
    [TestFixture]
    public class BarcodeCreaterTest
    {
        [Test]
        public void CreateBarcode_GivenBarcodeEndPoint_ShouldCreateBarCode()
        {
            //arrange
        }

        public static CustomBootstrapper CreateCustomBootstrapper(IIpAddress environment)
        {
            var bootstrapper = new CustomBootstrapper(with =>
            {
                with.Module<IpModule>();
                with.Dependency(environment);
            });
            return bootstrapper;
        }

        public static Browser CreateBrowser(IIpAddress environment)
        {
            var browser = new Browser(with =>
            {

                with.Module<IpModule>();
                with.Dependency(environment);
            });
            return browser;
        }
    }
}
