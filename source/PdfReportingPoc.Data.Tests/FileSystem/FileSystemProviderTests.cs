using System.IO;
using FluentAssertions;
using NUnit.Framework;
using PdfReportingPoc.FileSystem;

namespace PdfReportingPoc.Data.Tests.FileSystem
{
    [TestFixture]
    public class FileSystemProviderTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void LoadFile_GivenEmptyFileName_ShouldReturnByteOfZero(string fileName)
        {
            //Arrange
            var pdfPath = GetFilePath(fileName);

            var sut = CreateFileSystemProvider();

            //Act
            var actual = sut.LoadFile(pdfPath);

            //Assert
            var expected = new byte[0];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoadFile_GivenFileThatDoesNotExist_ShouldReturnByteOfZero()
        {
            //Arrange
            var fileName = "Invalid.pdf";
            var pdfPath = GetFilePath(fileName);

            var sut = CreateFileSystemProvider();

            //Act
            var actual = sut.LoadFile(pdfPath);

            //Assert
            var expected = new byte[0];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoadFile_GivenValidPath_ShouldLoadFileAndReturnBytesOfTheFile()
        {
            //Arrange
            var fileName = "BootCampForm-v2.pdf";
            var pdfPath = GetFilePath(fileName);

            var sut = CreateFileSystemProvider();

            //Act
            var actual = sut.LoadFile(pdfPath);

            //Assert
            var fileSize = actual.Length;
            Assert.AreEqual(94853, fileSize);
        }

        [Test]
        public void SaveFile_GivenAValidFile_ShouldWriteFile()
        {
            //Arrange
            var targetPath = Path.GetTempFileName();
            var fileName = "BootCampForm-v2.pdf";

            var pdfPath = GetFilePath(fileName);
            byte[] bytes = GetBytes(pdfPath);

            var sut = CreateFileSystemProvider();

            //Act
            sut.SaveFile(targetPath, bytes);
            var assertBytes = File.ReadAllBytes(targetPath);

            //Assert
            assertBytes.Length.Should().BeGreaterThan(0);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SaveFile_GivenANullOrEmptyTargetPath_ShouldReturnFalse(string targetPath)
        {
            //Arrange 
            var expected = false;
            var fileName = "BootCampForm-v2.pdf";

            var pdfPath = GetFilePath(fileName);
            var bytes = GetBytes(pdfPath);

            var sut = CreateFileSystemProvider();

            //Act
            var actual = sut.SaveFile(targetPath, bytes);


            //Assert
            Assert.AreEqual(expected, actual);
        }

        private static FileSystemProvider CreateFileSystemProvider()
        {
            return new FileSystemProvider();
        }

        private static byte[] GetBytes(string pdfPath)
        {
            return File.ReadAllBytes(pdfPath);
        }

        private static string GetFilePath(string fileName)
        {
            var baseDirectory = TestContext.CurrentContext.TestDirectory;
            var pdfPath = Path.Combine(baseDirectory + @"\TestData\" + fileName);
            return pdfPath;
        }
    }
}
