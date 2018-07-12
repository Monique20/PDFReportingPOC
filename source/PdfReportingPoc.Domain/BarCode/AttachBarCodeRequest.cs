namespace PdfReportingPoc.Domain.BarCode
{
    public class AttachBarCodeRequest
    {
        public byte[] FileBytes { get; set; }
        public byte[] QrCodeBytes { get; set; }
        public int PageNumber { get; set; }
        public float LowerLeftX { get; set; }
        public float LowerLeftY { get; set; }
        public float UpperRightX { get; set; }
        public float UpperRightY { get; set; }
    }

}
