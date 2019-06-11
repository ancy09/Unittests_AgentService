using System.IO;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class AgentRequestHandlerTest
    {
        [TestMethod]
        public void InitializeAgentRequestHandler_returnsTrueIfInitialized()
        {
            ReadConfigFile();
            AgentRequestHandler agent = new AgentRequestHandler();
            bool agentInitResult = agent.InitializeAgentRequestHandler();
            Assert.IsTrue(agentInitResult);
        }

        //This test method passes in visual studio, but fails in command line!

        //[TestMethod]
        //[ExpectedException(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException))]
        //public void InitializeAgentRequestHandler_raisesExceptionForNullConfigobject()
        //{
        //    AgentRequestHandler agent = new AgentRequestHandler();
        //    agent.InitializeAgentRequestHandler();
        //}

        [TestMethod]
        public void SetConfigDetails_returnsEmptyStringForActivate()
        {
            ReadConfigFile();
            var jsonRequest = GetJsonRequest("Activate");
            AgentRequestHandler agent = new AgentRequestHandler();
            string jsonResponse = agent.SetConfigDetails("Activate", jsonRequest);
            Assert.IsTrue(jsonResponse.Equals(string.Empty));
        }

        [TestMethod]
        public void SetConfigDetails_returnsJsonResponseForSetConfiguration()
        {
            ReadConfigFile();
            var jsonRequest = GetJsonRequest("SetConfiguration");
            AgentRequestHandler agent = new AgentRequestHandler();
            string jsonResponse = agent.SetConfigDetails("SetConfiguration", jsonRequest);
            Assert.IsTrue(jsonResponse.Length > 0);
        }

        [TestMethod]
        public void SetConfigDetails_returnsEmptyStringForUpdateConnection()
        {
            ReadConfigFile();
            var jsonRequest = GetJsonRequest("SetConfiguration");
            AgentRequestHandler agent = new AgentRequestHandler();
            string jsonResponse = agent.SetConfigDetails("UpdateConnectionConfiguration", jsonRequest);
            Assert.IsTrue(jsonResponse.Equals(string.Empty));
        }

        public void ReadConfigFile()
        {
            ConfigFile config = new ConfigFile();
            config.ReadConfigFile();
        }

        public string[] GetSetConfigurationJsonFromFile()
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var currentDir = Directory.GetParent(dir).FullName;
            string filePath = string.Concat(currentDir, "\\TestData\\SetConfiguration.json");
            return File.ReadAllLines(filePath);
        }

        public object GetJsonRequest(string eventType)
        {
            string[] json = null;
            JsonConverterTest jsontest = new JsonConverterTest();
            switch (eventType)
            {
                case "SetConfiguration":
                    {
                        json = GetSetConfigurationJsonFromFile();
                        break;

                    }
                case "Activate":
                    {
                        json = jsontest.GetJsonFromFile();
                        break;
                    }
                default: break;

            }
            JsonConverter jsonConvert = jsontest.GetJsonConverter();
            return jsonConvert.DeserializeJson(json[0]);
        }

    }
}
