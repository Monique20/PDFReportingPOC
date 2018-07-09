using System;
using System.IO;
using Aspose.Pdf.Facades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using File = System.IO.File;

namespace PdfReportingPoc.BarCode
{
    public static class AttachBarCodes
    {
        // todo : Remove unsed method
        public static byte[] To_PDF(string pdfPath, string barCodeImagePath)
        {
            var pdfWithAttachmentBytes = new byte[] { };


            var pathToSavePDf = AppDomain.CurrentDomain.BaseDirectory + "Document\\PDFBarCodeAttached.pdf";

            using (Stream inputPdfStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new FileStream(barCodeImagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(pathToSavePDf, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                var jpg = Image.GetInstance(inputImageStream);
                jpg.ScaleToFit(140f, 120f);
                jpg.SpacingBefore = 10f;
                jpg.SpacingAfter = 1f;
                jpg.Alignment = Element.ALIGN_LEFT;
                jpg.SetAbsolutePosition(100, 100);

                pdfContentByte.AddImage(jpg);
                stamper.Close();

                pdfWithAttachmentBytes = File.ReadAllBytes(pathToSavePDf);

            }

            return pdfWithAttachmentBytes;
        }
    
        // todo : This needs a better name
        public static byte[] DoThis(string pdfPath, string barCodeImagePath)
        {
            // todo : This needs to be a byte[] passed into the method 
            var pathToSavePDf = AppDomain.CurrentDomain.BaseDirectory + "Document\\PDFBarCodeAttached.pdf"; 

            // todo : This needs to be a byte[] passed into the method
            var barcode = File.ReadAllBytes(barCodeImagePath);

            //todo : This needs to be in a using statement!
            var stream = new MemoryStream();
            stream.Write(barcode, 0, barcode.Length);

            // todo : This needs to be in a using statment!
            var mender = new PdfFileMend(pdfPath, pathToSavePDf);

            var fileInfo = new PdfFileInfo(pdfPath); //todo : this must be in a using method!

            // todo : These values need to be passed into the DoThis method!
            var lowerLeftX = 100f;
            var lowerLeftY = 100f;
            var upperRightX = 200f;
            var upperRightY = 200f;

            var filePages = fileInfo.NumberOfPages;

            // why would we add it to each page?
            // todo : Take page number as a paraemter into this method!
            for (var pageNumber = 1; pageNumber <= filePages; pageNumber++)
            {
                mender.AddImage(stream, pageNumber, lowerLeftX, lowerLeftY, upperRightX, upperRightY);
            }

            mender.Close();
            stream.Close();
            
            return barcode;
        }
    }
}
