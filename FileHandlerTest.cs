using System.Collections.Generic;
using System.IO;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class FileHandlerTest
    {
        public string LogFilePath = "D:/log/IoTAgent/";

        [TestMethod]
        public void CreateFile_createsFileInSpecifiedFolder()
        {
            FileHandler.CreateFile(LogFilePath + "TestingAgentService.log");
            var expectedFilePath = "D:/log/IoTAgent/TestingAgentService.log";
            Assert.IsTrue(File.Exists(expectedFilePath));
        }

        [TestMethod]
        public void CreateFile_returnsFalseForInvalidFilePath()
        {
            var createFileResult = FileHandler.CreateFile("TestingAgentService.log");
            Assert.IsFalse(createFileResult);
        }

        [TestMethod]
        public void MoveWithDiffName_movesTheSourceFileToNewFile()
        {
            string sourceFileName = LogFilePath + "TestingAgentService.log";
            string DestFileName = LogFilePath + "TestingAgentServiceMoved.log";
            FileHandler.CreateFile(sourceFileName);
            FileHandler.MoveWithDiffName(sourceFileName, DestFileName);
            Assert.IsTrue(File.Exists(DestFileName));
        }

        [TestMethod]
        public void DeleteFile_deletesAlreadyExistingFile()
        {
            string sourceFileName = LogFilePath + "TestingAgentService.log";
            FileHandler.CreateFile(sourceFileName);
            Assert.IsTrue(File.Exists(sourceFileName));
            FileHandler.DeleteFile(sourceFileName);
            Assert.IsFalse(File.Exists(sourceFileName));
        }

        [TestMethod]
        public void GetFileSize_returnsFileSizeInMB()
        {
            string sourceFileName = LogFilePath + "TestingAgentService.log";
            FileHandler.CreateFile(sourceFileName);
            long fileSize = FileHandler.GetFileSize(sourceFileName);
            Assert.IsTrue(fileSize >= 0);
        }

        [TestMethod]
        public void GetFileSize_returnsLessThanZeroForInvalidFile()
        {
            string sourceFileName = LogFilePath + "abcd.log";
            long fileSize = FileHandler.GetFileSize(sourceFileName);
            Assert.IsTrue(fileSize < 0);
        }

        [TestMethod]
        public void VerifyFilePath_returnsTrueForValidPath()
        {
            string sourceFileName = LogFilePath + "TestingAgentService.log";
            var filePathResult = FileHandler.VerifyFilePath(sourceFileName);
            Assert.IsTrue(filePathResult);
        }

        [TestMethod]
        public void VerifyFilePath_returnsFalseForInvalidPath()
        {
            string InvalidPath = @"F:\log\";
            var filePathResult = FileHandler.VerifyFilePath(InvalidPath);
            Assert.IsFalse(filePathResult);
        }

        [TestMethod]
        public void GetFilesInDirectory_returnsListOfFiles()
        {
            Create5TextFiles();
            var fileNamesResult = FileHandler.GetFilesInDirectory(@"D:\TestListFiles\", "*");
            var expectedFileNames = GetExpectedFileNames();
            CollectionAssert.AreEquivalent(expectedFileNames, fileNamesResult);
        }

        [TestMethod]
        public void RemoveReadOnly_removesReadOnlyAttribute()
        {
            string sourceFileName = LogFilePath + "TestingAgentService.log";
            File.SetAttributes(sourceFileName, FileAttributes.Archive | FileAttributes.ReadOnly);
            var filePathResult = FileHandler.RemoveReadOnly(sourceFileName);
            FileAttributes attribs = File.GetAttributes(sourceFileName);
            Assert.IsFalse(attribs.Equals(FileAttributes.ReadOnly));
        }

        private List<string> GetExpectedFileNames()
        {
            List<string> fileNames = new List<string>();
            string basePath = @"D:\TestListFiles\";
            fileNames.Add(basePath + "Test1.txt");
            fileNames.Add(basePath + "Test2.txt");
            fileNames.Add(basePath + "Test3.txt");
            fileNames.Add(basePath + "Test4.txt");
            fileNames.Add(basePath + "Test5.txt");
            return fileNames;
        }

        private void Create5TextFiles()
        {
            string basePath = @"D:\TestListFiles\";
            FileHandler.CreateFile(basePath + "Test1.txt");
            FileHandler.CreateFile(basePath + "Test2.txt");
            FileHandler.CreateFile(basePath + "Test3.txt");
            FileHandler.CreateFile(basePath + "Test4.txt");
            FileHandler.CreateFile(basePath + "Test5.txt");
        }
    }
}
