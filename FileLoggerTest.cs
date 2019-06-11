using System.Diagnostics;
using System.IO;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class FileLoggerTest
    {
        [TestMethod]
        public void InitializeFileLogger_returnsTrue()
        {
            Assert.IsTrue(FileLogger.InitializeFileLogger());
        }

        [TestMethod]
        public void Log_VerifyLogFilePath()
        {
            var expectedFilePath = "D:/log/IoTAgent/IoTAgentService.log";
            FileLogger.InitializeFileLogger();
            var actualFilePath = FileLogger.LogFile;
            Assert.AreSame(expectedFilePath, actualFilePath);
        }

        [TestMethod]
        public void Log_VerifyFileExists()
        {
            FileLogger.InitializeFileLogger();
            Assert.IsTrue(File.Exists(FileLogger.LogFile));
        }

        [TestMethod]
        public void Log_VerifyInitialLog()
        {
            FileLogger.InitializeFileLogger();
            FileLogger.UnInitializeLogger();
            string log = File.ReadAllText(FileLogger.LogFile);
            Assert.IsTrue(log.Contains("Tracing Started for *****vstest.executionengine*****"));
        }

        [TestMethod]
        public void Log_VerifySpecifiedLog()
        {
            FileLogger.InitializeFileLogger();
            FileLogger.Log(TraceEventType.Information, "Log file verified!");
            FileLogger.UnInitializeLogger();
            string log = File.ReadAllText(FileLogger.LogFile);
            Assert.IsTrue(log.Contains("Log file verified!"));
        }

    }
}
