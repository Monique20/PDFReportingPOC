using System.Collections.Generic;
using PdfReportingPoc.Domain.Pdf;

namespace PdfReportingPoc.UseCase.Pdf
{
    public class PopulatePdfUseCase : IPopulatePdfUseCase
    {
        private readonly IPdfOperations _fieldsOperations;
        public PopulatePdfUseCase(IPdfOperations fieldsOperations)
        {
            _fieldsOperations = fieldsOperations;
        }

        public PdfFieldsOperationsResponse Execute(byte[] templateByte,List<PdfFields> fieldsToPopulate)
        {
            var populatedFieldBytes = _fieldsOperations.PopulateFieldValues(templateByte, fieldsToPopulate);

            if (populatedFieldBytes.HadError())
            {
                return populatedFieldBytes;
            }

            var readOnlyBytes =_fieldsOperations.MarkFieldsReadOnly(populatedFieldBytes.Output, fieldsToPopulate);
            var result = new PdfFieldsOperationsResponse{Output = readOnlyBytes.Output };

            return result;
        }
       
    }
}