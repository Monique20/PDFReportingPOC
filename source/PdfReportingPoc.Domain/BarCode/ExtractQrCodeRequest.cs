namespace PdfReportingPoc.Domain.BarCode
{
    public class ExtractQrCodeRequest
    {
        public byte[]Image { get; set; }
        public bool CheckSumEnabled { get; set; }
    }
}
