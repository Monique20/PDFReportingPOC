namespace PdfReportingPoc.Domain.BarCode
{
    public class CreateBarCodeResponse
    {
        public string Status { get; set; } // todo : remove this - it unused
        public byte[] BarCode { get; set; }
    }
}
