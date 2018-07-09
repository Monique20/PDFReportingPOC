namespace PdfReportingPoc.Domain.Pdf
{
    public class PdfFieldsOperationsResponse
    {
        public string ErrorMessage { get; set; }
        public byte[] Output { get; set; }

        public bool HadError()
        {
            return !string.IsNullOrWhiteSpace(ErrorMessage);
        }
    }
}
