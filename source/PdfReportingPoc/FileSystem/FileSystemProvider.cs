using System.IO;
using PdfReportingPoc.Domain.FileSystem;

namespace PdfReportingPoc.FileSystem
{
    public class FileSystemProvider : IFileSystemProvider
    {
        public byte[] LoadFile(string pdfPath)
        {
            if (File.Exists(pdfPath))
            {
                var fileByte = File.ReadAllBytes(pdfPath);
                return fileByte;
            }

            return new byte[0];
        }
        public bool SaveFile(string targetPath, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                return false;
            }

            File.WriteAllBytes(targetPath, bytes);

            return true;
        }
    }
}
