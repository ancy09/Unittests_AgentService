using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class PipeServerTest
    {

        [TestMethod]
        public void PipeServerConstructor_createsAReadWritePipe()
        {
            var pipeServerStream = new NamedPipeServerStream("Agent Pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            AgentRequestHandler agent = new AgentRequestHandler();
            PipeServer serverObject = new PipeServer(pipeServerStream, agent);
            Assert.IsTrue(pipeServerStream.CanRead & pipeServerStream.CanWrite);
        }

        [TestMethod]
        public void PipeServer_ReadFromPipeWorksFine()
        {
            AgentRequestHandlerTest agentTest = new AgentRequestHandlerTest();
            agentTest.ReadConfigFile();
            AgentRequestHandler agentHandler = new AgentRequestHandler();

            NamedPipeServerStream serverStream = new NamedPipeServerStream("Agent_Pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            var json = agentTest.GetSetConfigurationJsonFromFile();
            byte[] messageBytes = Encoding.UTF8.GetBytes(json[0]);
            PipeServer obj = new PipeServer(serverStream, agentHandler);
            Task t = Task.Run(() =>
                {
                    using (NamedPipeClientStream client = new NamedPipeClientStream(".", "Agent_Pipe", PipeDirection.InOut))
                    {
                        client.Connect();

                        //Assert client connection.
                        Assert.IsTrue(client.IsConnected);
                        client.Write(messageBytes, 0, messageBytes.Length);

                    }
                });
           
            serverStream.WaitForConnection();

            //Assert server connection.
            Assert.IsTrue(serverStream.IsConnected);
            obj.ReadFromPipe();
        }

        [TestMethod]
        public void PipeServer_WriteToPipeWorksFine()
        {
            NamedPipeServerStream serverStream = new NamedPipeServerStream("Pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            AgentRequestHandlerTest agentTest = new AgentRequestHandlerTest();
            AgentRequestHandler agentHandler = new AgentRequestHandler();

            var json = agentTest.GetSetConfigurationJsonFromFile();
            PipeServer obj = new PipeServer(serverStream, agentHandler);
          
            Task t = Task.Run(() =>
            {
                using (NamedPipeClientStream client = new NamedPipeClientStream(".", "Pipe", PipeDirection.InOut))
                {
                    client.Connect();

                    //Assert client connection.
                    Assert.IsTrue(client.IsConnected);
                }
            });
            serverStream.WaitForConnection();

            //Assert server connection.
            Assert.IsTrue(serverStream.IsConnected);
            obj.WriteToPipe(json[0]);
        }
    }
}
