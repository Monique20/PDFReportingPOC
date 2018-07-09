namespace PdfReportingPoc.Domain.Report
{
    public interface IRenderReportUseCase
    {
        byte[] Execute(RenderReportRequest request);
    }
}
