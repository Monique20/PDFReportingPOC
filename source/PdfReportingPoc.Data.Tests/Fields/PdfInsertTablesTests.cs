using Aspose.Pdf;
using Aspose.Pdf.Text;
using FluentAssertions;
using NUnit.Framework;
using PdfReportingPoc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfReportingPoc.Data.Tests.Fields
{
    [TestFixture]
    public class PdfInsertTablesTests
    {
        [Test]
        public void InsertTable_GivenAFile_ShouldAddTableToFile()
        {
            //Arrange
            var fileName = "DemoForm.pdf";
            var newFileName = "DemoFormWithTable.pdf";

            var sut = new PdfInsertTable();

            //Act
            var actual = sut.InsertTable(fileName, newFileName);

            //Assert
            actual.Length.Should().BeGreaterThan(0);
        }

        //[Test]
        //public void LearningTest()
        //{
        //    //Arrange
        //    var fileName = "DemoForm.pdf";
        //    var newFileName = "DemoFormHiddenText.pdf";

        //    var sut = new PdfInsertTable();

        //    //Act
        //    var actual = sut.HideHeader(fileName, newFileName);

        //    //Assert
        //    actual.Length.Should().BeGreaterThan(0);
        //}
    }
}
