using System;
using Aspose.Pdf;
using Aspose.Pdf.Forms;
using System.IO;
using System.Linq;

namespace PdfReportingPoc.Fields
{
    public class PdfFields
    {

        public PdfFields()
        {
            var license = new License();
            license.SetLicense("Aspose.Pdf.Lic");
        }

        public PdfOperationResult MarkFieldsReadOnly(string[] fields, byte[] temp)
        {
            var results = new PdfOperationResult();

            var documentBytes = new byte[0];


            using (var incomingStream = new MemoryStream(temp))
            {
                var pdfDocument = new Document(incomingStream);
                foreach (var field in fields)
                {
                    var fieldNotFound = FieldNotFound(pdfDocument, field); 
                    if (fieldNotFound)

                    {
                        results.Output = documentBytes;
                        results.ErrorMessage = $"{field} does not exist";
                        return results;
                    }
                    pdfDocument.Form[field].ReadOnly = true;
                }

                using (var documentStream = new MemoryStream())
                {
                    pdfDocument.Save(documentStream);
                    documentBytes = documentStream.ToArray();
                }
            }

            results.Output = documentBytes;

            return results;


        }


        private static bool FieldNotFound(Document pdfDocument, string field)
        {
            return !pdfDocument.Form.Any(x => x.FullName.Equals((field)));

        }

    }


}
