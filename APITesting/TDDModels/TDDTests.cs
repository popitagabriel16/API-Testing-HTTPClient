using APITesting.Steps;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using APITesting.Configuration;
using APITesting.Models;
using System.IO;
using TechTalk.SpecFlow;
using System.Linq;
using APITesting.TDDModels;
using APITesting.TDDTests;

namespace APITesting.TDD_Models
{
    [TestFixture]
    public class Tests
    {

        private HttpResponseMessage httpResponse;
        private UserData userData;
        private User user;
        private ConfigurationReader _configurationReader;

        [SetUp]
        public void BeforeScenario()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var configFilePath = Path.Combine(currentDirectory, "..", "..", "..", "Configuration", "config.json");
            _configurationReader = new ConfigurationReader(configFilePath, "Development");
        }

        [Test]
        public async Task GetUserByID_ShouldReturnUser()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "https://reqres.in/api/users?page=2";
            string endpoint = "users?page=2";

            var baseUrl = _configurationReader.GetBaseUrl();
            var baseAddress = new Uri(baseUrl);
            client.BaseAddress = baseAddress;

            var httpResponse = await client.GetAsync(endpoint);
            if (httpResponse.IsSuccessStatusCode)
            {
                string json = await httpResponse.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<UserData>(json);
                var user = userData.Data.FirstOrDefault(u => u.Id == 7);
                Assert.IsNotNull(user);
                Assert.AreEqual(TestData.getName, user.First_name);
            }
            else
            {
                Assert.Fail("API request failed");
            }
        }

        [Test]
        public async Task DeleteUserByID()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "https://reqres.in/api/users/2";
            var httpResponse = await client.DeleteAsync(apiUrl);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, httpResponse.StatusCode);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnUpdatedUser()
        {
            // Arrange
            HttpClient client = new HttpClient();
            string apiUrl = "https://reqres.in/api/users/2";

            var updateUser = new Job
            {
                name = TestData.putName,
                job = TestData.putJob,
            };

            string payload = JsonConvert.SerializeObject(updateUser);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            string endpoint = "users/2";

            var baseUrl = _configurationReader.GetBaseUrl();
            var baseAddress = new Uri(baseUrl);
            client.BaseAddress = baseAddress;

            // Act
            var httpResponse = await client.PutAsync(endpoint, content);

            // Assert
            Assert.IsTrue(httpResponse.IsSuccessStatusCode);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var updatedUser = JsonConvert.DeserializeObject<Job>(jsonResponse);

            Assert.AreEqual("morpheus", updatedUser.name);
            Assert.AreEqual("zion resident", updatedUser.job);
            Assert.IsNotNull(updatedUser.updatedAt);
        }
    }
}
