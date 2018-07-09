namespace PdfReportingPoc.Domain.FileSystem
{
    public interface IFileSystemProviderUseCase
    {
        byte[] LoadFile(string pdfPath);
    }
}
