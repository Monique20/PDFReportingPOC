namespace PdfReportingPoc.Domain.BarCode
{
    public class CreateQrCodeRequest
    {
        public string Text { get; set; }
        public bool CheckSumEnabled { get; set; }
    }
}
