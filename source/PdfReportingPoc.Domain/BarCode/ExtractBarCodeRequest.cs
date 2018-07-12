namespace PdfReportingPoc.Domain.BarCode
{
    public class ExtractBarCodeRequest
    {
        public byte[] Image { get; set; }
        public bool CheckSumEnabled { get; set; }
    }
}
