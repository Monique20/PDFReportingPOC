namespace PdfReportingPoc.Domain.BarCode
{
    public class CreateBarCodeRequest
    {
        public string Text { get; set; }
        public bool CheckSumEnabled { get; set; }
    }
}
