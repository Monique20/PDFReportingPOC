namespace PdfReportingPoc.Domain.FileSystem
{
    public interface IFileSystemProvider
    {
        byte[] LoadFile(string pdfPath);
        bool SaveFile(string targetPath, byte[] bytes);
    }
}
