namespace PdfReportingPoc.Domain.BarCode
{
    public class Barcode
    {
        public int PageNumber { get; set; }
        public float LowerLeftX { get; set; }
        public float LowerLeftY { get; set; }
        public float UpperRightX { get; set; }
        public float UpperRightY { get; set; }

    }
}
