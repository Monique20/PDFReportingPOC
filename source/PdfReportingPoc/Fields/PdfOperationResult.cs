namespace PdfReportingPoc.Fields
{
    // todo : This needs a better name. Builder postfix signals that this is a builder, which it is NOT!
    // todo : A simpler name would be PdfOperationResult
    public class PdfOperationResult
    {
        public string ErrorMessage { get; set; }
        public byte[] Output { get; set; }
    }
}
