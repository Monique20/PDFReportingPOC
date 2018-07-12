using System.IO;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using PdfReportingPoc.Domain.BarCode;

namespace PdfReportingPoc.BarCode
{
    public class BarCodeAttachmentOperations : IBarCodeAttachmentOperations
    {
        public byte[] Attach(AttachBarCodeRequest request)
        {
            byte[] bytes;
            using (var qrCodeStream = GetStream(request.QrCodeBytes))
            {
                using (var fileStream = GetStream(request.FileBytes))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        var outputFile = GetDocument(fileStream);

                        var lowerLeftX = request.LowerLeftX;
                        var lowerLeftY = request.LowerLeftY;
                        var upperRightX = request.UpperRightX;
                        var upperRightY = request.UpperRightY;

                        outputFile.AddImage(qrCodeStream, request.PageNumber, lowerLeftX, lowerLeftY, upperRightX, upperRightY);

                        outputFile.Save(outputStream);

                        bytes = outputStream.ToArray();
                    }
                }
            }
            return bytes;
        }

        private static PdfFileMend GetDocument(Stream fileStream)
        {
            var document = new Document(fileStream);
            return new PdfFileMend(document);
        }

        private static MemoryStream GetStream(byte[] file)
        {
            var stream = new MemoryStream();
            stream.Write(file, 0, file.Length);
            return stream;
        }
    }
}
