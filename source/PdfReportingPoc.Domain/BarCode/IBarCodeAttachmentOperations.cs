namespace PdfReportingPoc.Domain.BarCode
{
    public interface IBarCodeAttachmentOperations
    {
         byte[] Attach(AttachBarCodeRequest resquest);
    }
}
