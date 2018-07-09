using System.Collections.Generic;

namespace PdfReportingPoc.Domain.Pdf
{
    public interface IPopulatePdfUseCase
    {
        PdfFieldsOperationsResponse Execute(byte[] templateByte, List<PdfFields> fieldsToPopulate);
    }
}