using System.Collections.Generic;

namespace PdfReportingPoc.Domain.Pdf
{
    public interface IPdfOperations
    {
        PdfFieldsOperationsResponse MarkFieldsReadOnly(byte[] template, List<PdfFields> listOfFields);
        PdfFieldsOperationsResponse PopulateFieldValues(byte[] template, List<PdfFields> listOfFields);
        byte[] PasswordProtect(byte[] fileBytes, string password);
    }
}
