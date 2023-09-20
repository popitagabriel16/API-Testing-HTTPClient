using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace APITesting.Configuration
{
    public class ConfigurationReader
    {
        private readonly string _configFilePath;
        private ConfigModel _config;
        private readonly string _environment;

        public ConfigurationReader(string configFilePath, string environment)
        {
            _configFilePath = configFilePath;
            _environment = environment;
            LoadConfiguration();
            _environment = environment;
        }

        private void LoadConfiguration()
        {
            var jsonString = File.ReadAllText(_configFilePath);
            var configData = JsonConvert.DeserializeObject<ConfigData>(jsonString);

            switch (_environment)
            {
                case "Development":
                    _config = configData.Development;
                    break;
                case "Staging":
                    _config = configData.Staging;
                    break;
                case "Production":
                    _config = configData.Production;
                    break;
                default:
                    throw new ArgumentException($"Invalid environment: {_environment}");
            }
        }

        public string GetBaseUrl()
        {
            return _config.BaseUrl;
        }

        public string GetApiKey()
        {
            return _config.ApiKey;
        }
    }

    public class ConfigData
    {
        public ConfigModel Development { get; set; }
        public ConfigModel Staging { get; set; }
        public ConfigModel Production { get; set; }
    }

    public class ConfigModel
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    [TestFixture]
    public class ConfigurationReaderTests
    {
        private ConfigurationReader _configurationReader;

        [SetUp]
        public void Setup()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var configFilePath = Path.Combine(currentDirectory, "..", "..", "..", "Configuration", "config.json");
            var environment = "Development";

            _configurationReader = new ConfigurationReader(configFilePath, environment);
        }

        //SOME SMALL "TDD" WITHOUT BDD CHARACTER(SPECFLOW) TEST-ACTION JUST FOR FUN
        [Test]
        public void GetBaseUrl_ReturnsCorrectValue()
        {
            // Arrange
            var baseUrl = _configurationReader.GetBaseUrl();

            // Assert
            Assert.AreEqual("https://reqres.in/api/", baseUrl);
        }

        [Test]
        public void GetApiKey_ReturnsCorrectValue()
        {
            // Arrange
            var apiKey = _configurationReader.GetApiKey();

            // Assert
            Assert.AreEqual("API_TOKEN", apiKey);
        }
    }
}
