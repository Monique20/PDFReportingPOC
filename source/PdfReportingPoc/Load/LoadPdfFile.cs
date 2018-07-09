using PdfReportingPoc.Domain.File;

namespace PdfReportingPoc.File
{
    public class LoadPdfFile : ILoadFile
    {
        public byte[] LoadFile(string pdfPath)
        {
            if(System.IO.File.Exists(pdfPath))
            {
                var fileByte = System.IO.File.ReadAllBytes(pdfPath);
                return fileByte; ;
            }
            var response = new byte[0];
            return response;
        }
    }
}
