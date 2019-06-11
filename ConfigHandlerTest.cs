using System;
using System.Configuration;
using System.IO;
using IoTAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgentServiceTest
{
    [TestClass]
    public class ConfigHandlerTest
    {

        [TestMethod]
        public void ReadConfigFile_createsValidConfigObject()
        {
            ReadConfigFile();
            Assert.IsNotNull(ConfigFile.ConfigObj);
        }

        [TestMethod]
        public void WriteConfigFile_writesToAFile()
        {
            ReadConfigFile();
            ConfigFile.WriteConfigFile();
            var configPath = GetConfigPath();
            FileInfo configFile = new FileInfo(configPath);
            Assert.IsTrue(configFile.Length > 0);
        }

        private void ReadConfigFile()
        {
            ConfigFile config = new ConfigFile();
            config.ReadConfigFile();
        }

        private string GetConfigPath()
        {
            string getEnv = Environment.GetEnvironmentVariable(Constants.TestRootPath);
            var appReader = new AppSettingsReader();
            string keyvalue = appReader.GetValue("configpath", typeof(string)).ToString();
            var configPath = getEnv + keyvalue;
            return configPath;
        }
    }
}
