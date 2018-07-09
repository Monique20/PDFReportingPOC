using Aspose.Pdf;
using Aspose.Pdf.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfReportingPoc.Domain.Pdf;

namespace PdfReportingPoc.Fields
{
    public class PdfOperations : IPdfOperations
    {
        public PdfOperations()
        {
            var license = new License();
            license.SetLicense("Aspose.Pdf.Lic");
        }

        public PdfFieldsOperationsResponse MarkFieldsReadOnly(byte[] template, List<PdfFields> listOfFields)
        {
            var results = new PdfFieldsOperationsResponse();
            var documentBytes = new byte[0];

            using (var incomingStream = new MemoryStream(template))
            {
                var pdfDocument = new Document(incomingStream);
                foreach (var field in listOfFields)
                {
                    var fieldName = field.FieldName;

                    bool fieldNotFound = FieldNotFound(pdfDocument, fieldName);
                    if (fieldNotFound)
                    {
                        return FieldErrorMessage(documentBytes, results, fieldName);
                    }
                    pdfDocument.Form[fieldName].ReadOnly = true;
                }

                documentBytes = Save(pdfDocument);
            }
            results.ErrorMessage = string.Empty;
            results.Output = documentBytes;

            return results;

        }

        public PdfFieldsOperationsResponse PopulateFieldValues(byte[] template, List<PdfFields> listOfFields)
        {
            var documentBytes = new byte[0];
            var results = new PdfFieldsOperationsResponse();
            using (var incomingStream = new MemoryStream(template))
            {
                var pdfDocument = new Document(incomingStream);

                foreach (var field in listOfFields)
                {
                    var fieldName = field.FieldName;
                    var fieldValue = field.FieldValue;

                    bool fieldNotFound = FieldNotFound(pdfDocument, fieldName);
                    if (fieldNotFound)
                    {
                        return FieldErrorMessage(documentBytes, results, fieldName);
                    }

                    var textBoxField = pdfDocument.Form[fieldName] as TextBoxField;
                    if (textBoxField != null)
                    {
                        var maxLength = (pdfDocument.Form[fieldName] as TextBoxField).MaxLen;
                        var exceededLength = fieldValue.Length - maxLength;

                        if (ValueExceededMaxLength(fieldValue, maxLength) && HasMaximumLength(maxLength))
                        {
                            results.Output = documentBytes;
                            results.ErrorMessage = $"{fieldName} has exceeded its maximum value by {exceededLength}";

                            return results;
                        }

                        textBoxField.Value = fieldValue;
                        continue;
                    }

                    var checkboxField = pdfDocument.Form[fieldName] as CheckboxField;
                    checkboxField.Checked = true;
                }

                documentBytes = Save(pdfDocument);
            }
            results.ErrorMessage = string.Empty;
            results.Output = documentBytes;
            return results;
        }

        public byte[] PasswordProtect(byte[] fileBytes, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new byte[0];
            }

            using (var incomingStream = new MemoryStream(fileBytes))
            {
                var pdfDocument = new Document(incomingStream);
                pdfDocument.Encrypt(password, password, 0, CryptoAlgorithm.RC4x128);
                using (var documentStream = new MemoryStream())
                {
                    pdfDocument.Save(documentStream);

                    return documentStream.ToArray();
                }
            }
        }

        private static PdfFieldsOperationsResponse FieldErrorMessage(byte[] documentBytes, PdfFieldsOperationsResponse results, string fieldName)
        {
            results.Output = documentBytes;
            results.ErrorMessage = $"{fieldName} cannot be found";

            return results;
        }

        private static bool ValueExceededMaxLength(string fieldValue, int maxLength)
        {
            return fieldValue.Length > maxLength;
        }

        private static bool HasMaximumLength(int maxLength)
        {
            return maxLength != -1;
        }

        private bool FieldNotFound(Document pdfDocument, string field)
        {
            return !pdfDocument.Form.Any(x => x.FullName.Equals((field)));
        }

        private static byte[] Save(Document pdfDocument)
        {
            byte[] documentBytes;
            using (var documentStream = new MemoryStream())
            {
                pdfDocument.Save(documentStream);
                documentBytes = documentStream.ToArray();
            }

            return documentBytes;
        }
    }

}
