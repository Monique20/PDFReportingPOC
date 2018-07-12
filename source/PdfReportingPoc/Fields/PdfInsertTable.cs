using Aspose.Pdf;
using Aspose.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfReportingPoc.Fields
{
    public class PdfInsertTable
    {
        public PdfInsertTable()
        {
            var license = new License();
            license.SetLicense("Aspose.Pdf.Lic");
        }

        public byte[] InsertTable(string fileName, string newFileName)
        {
            var pdfDocument = GetPdfDocument(fileName);
            var newFilePath = "";

            var employmentHistoryTable = CreateTableAndAddRows();
            if (employmentHistoryTable.Rows.Count == 0)
            {
                HideHeader(fileName, newFileName, pdfDocument);
                newFilePath = SaveNewDocument(newFileName, pdfDocument);
                return File.ReadAllBytes(newFilePath);
            }

            AddTableToDocument(pdfDocument, employmentHistoryTable);

            newFilePath = SaveNewDocument(newFileName, pdfDocument);
            return File.ReadAllBytes(newFilePath);
        }

        public void HideHeader(string fileName, string newFileName, Document pdfDocument)
        {
            TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber("Employment History");
            pdfDocument.Pages[1].Accept(textFragmentAbsorber);
            TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;
            if (textFragmentCollection.Any())
            {
                foreach (TextFragment textFragment in textFragmentCollection)
                {
                    textFragment.TextState.Invisible = true;
                }
            }
        }

        private static Document GetPdfDocument(string fileName)
        {
            var oldFile = GetFile(fileName);
            var pdfDocument = new Document(oldFile);
            return pdfDocument;
        }

        private static Table CreateTableAndAddRows()
        {
            var employmentHistoryTable = new Table();
            employmentHistoryTable.Border = new BorderInfo(BorderSide.All, .5f, Color.FromRgb(System.Drawing.Color.LightGray));
            employmentHistoryTable.DefaultCellBorder = new BorderInfo(BorderSide.All, .5f, Color.FromRgb(System.Drawing.Color.LightGray));
            employmentHistoryTable.Top = 220f;
            employmentHistoryTable.ColumnAdjustment = ColumnAdjustment.AutoFitToWindow;

            NewMethod(employmentHistoryTable);

            return employmentHistoryTable;
        }

        private static void NewMethod(Table employmentHistoryTable)
        {
            for (int row_count = 1; row_count < 50; row_count++)
            {
                Row row = employmentHistoryTable.Rows.Add();
                row.Cells.Add("Company Name");
                row.Cells.Add("Job Role");
                row.Cells.Add("Period of Employment");
                row.Cells.Add("Reason for leaving");
            }
        }

        private static void AddTableToDocument(Document pdfDocument, Table employmentHistoryTable)
        {
            pdfDocument.Pages[1].Paragraphs.Add(employmentHistoryTable);
        }

        private static string SaveNewDocument(string newFileName, Document pdfDocument)
        {
            var newFilePath = GetFile(newFileName);
            pdfDocument.Save(newFilePath);
            return newFilePath;
        }
        
        private static string GetFile(string fileName)
        {
            var templateLocation = ConfigurationManager.AppSettings["TableTemplatesDirectory"];
            return Path.Combine(templateLocation, fileName);
        }
    }
}
