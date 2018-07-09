namespace PdfReportingPoc.Domain.BarCode
{
    public class CreateQrCodeResponse
    {
        public string Status { get; set; }
        public byte[] BarCode { get; set; }
    }
}
