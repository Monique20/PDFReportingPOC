using PdfReportingPoc.Domain.BarCode;
using PdfReportingPoc.Domain.Pdf;
using System.Collections.Generic;

namespace PdfReportingPoc.Domain.Report
{
    public class RenderReportRequest
    {
        public string FileName { get; set; }
        public List<PdfFields> ListOfFields { get; set; }
        public CreateBarCodeRequest QrCodeData { get; set; }
        public Barcode Barcode { get; set; }
        public string Password { get; set; }
    }
}
