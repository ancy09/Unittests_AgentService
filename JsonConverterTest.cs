using System.IO;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class JsonConverterTest
    {
        [TestMethod]
        public void DeserializeJson_deserializesAJsonString()
        {
            string[] json = GetJsonFromFile();
            JsonConverter jsonConvert = GetJsonConverter();
            var jsonRequest = jsonConvert.DeserializeJson(json[0]);
            Assert.IsNotNull(jsonRequest);
        }

        public JsonConverter GetJsonConverter()
        {
            return new JsonConverter();
        }

        public string[] GetJsonFromFile()
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var currentDir = Directory.GetParent(dir).FullName;
            string filePath = string.Concat(currentDir, "\\TestData\\Activate.json");
            return File.ReadAllLines(filePath);
        }

    }
}
