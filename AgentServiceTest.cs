using System.IO;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class AgentServiceTest
    {
        [TestMethod]
        public void InitAgentService_createsTheLogFile()
        {
            InitializeAndInvoke();
            Assert.IsTrue(File.Exists(FileLogger.LogFile));
        }

        [TestMethod]
        public void InitAgentService_logsTheInitialLog()
        {
            InitializeAndInvoke();
            string text = ReadLogFile();
            Assert.IsTrue(text.Contains("Starting IoTAgent Service"));
        }

        [TestMethod]
        public void InitAgentService_createsThePipeServerThread()
        {
            InitializeAndInvoke();
            string text = ReadLogFile();
            Assert.IsTrue(text.Contains("Initializes a new instance of the NamedPipeServerStream"));
            Assert.IsTrue(text.Contains("Starting named pipe read thread"));
        }

        [TestMethod]
        public void StopAgentServicee_logsTheFinalLog()
        {
            InitializeAndInvoke();
            string text = ReadLogFile();
            Assert.IsTrue(text.Contains("Stopping IoTAgent Service"));
        }

        private void InitializeAndInvoke()
        {
            AgentService agent = new AgentService();
            agent.InitAgentService();
            agent.StopAgentService();
        }

        private string ReadLogFile()
        {
            return File.ReadAllText(FileLogger.LogFile);
        }
    }
}
