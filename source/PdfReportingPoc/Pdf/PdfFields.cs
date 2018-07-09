using System;

namespace PdfReportingPoc.Pdf
{
    public class PdfFields
    {
        // todo : DataModel is a horrible name - Find a better one WITH MODEL in it!!!!!
        public DataModel MarkFieldsReadOnly(string fieldsToMark, string currentFilePath, string newFilePath)
        {
            DataModel result = new DataModel(); // todo : use var!
            if (string.IsNullOrWhiteSpace(fieldsToMark))
            {
                result.ErrorMessage = "No field to mark";
                return result;
            }

            // todo : This try catch belows in the DisableFields method, not here! Minimize nesting!
            try
            {
                DisableFields(fieldsToMark, currentFilePath, newFilePath);
            }
            catch (Exception)
            {
                result.ErrorMessage = "Invalid field entered";
            }
            return result;
        }

        private static void DisableFields(string fieldsToMark, string currentFilePath, string newFilePath)
        {
            // TODO : DO NOT USE iTextSharp FOR THIS WORK!!! It must come from the aspose.pdf sdk!!!!

            //PdfReader reader = new PdfReader(currentFilePath);
            //using (PdfStamper stamper = new PdfStamper(reader, new FileStream(newFilePath, FileMode.Create)))
            //{
            //    AcroFields form = stamper.AcroFields;
            //    form.SetFieldProperty(fieldsToMark, "setfflags", PdfFormField.FF_READ_ONLY, null);
            //}
        }
    }
}
