using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using APITesting.Models;
using System.Linq;
using APITesting.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Net;

namespace APITesting.Steps
{
    [Binding]
    public class LoginUserSteps
    {
        private string email;
        private string password;
        private string token;
        private static readonly string ENDPOINT = "https://reqres.in/api/";
        private HttpResponseMessage httpResponse;
        private CreateUserRequest createUserRequest;
        private ConfigurationReader _configurationReader;
        private static TableRow loginDetails;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var configFilePath = Path.Combine(currentDirectory, "..", "..", "..", "Configuration", "config.json");
            _configurationReader = new ConfigurationReader(configFilePath, "Development");
        }

        [Given(@"I have the following login details:")]
        public void GivenIHaveTheFollowingLoginDetails(Table table)
        {
            loginDetails = table.Rows[0];
            email = loginDetails["email"];
            password = loginDetails["password"];
        }

        [When(@"I make a POST request to ""(.*)"" with the login details")]
        public async Task WhenIMakeAPostRequestToWithTheLoginDetails(string endpoint)
        {
            var httpClient = new HttpClient();

            var test = loginDetails;

            var baseUrl = _configurationReader.GetBaseUrl();
            var baseAddress = new Uri(baseUrl);
            httpClient.BaseAddress = baseAddress;

            var requestBody = new
            {
                email = "eve.holt@reqres.in",
                password = "cityslicka"
            };

            var serializedBody = JsonConvert.SerializeObject(requestBody);
            var stringContent = new StringContent(serializedBody, Encoding.UTF8, "application/json");

            httpResponse = await httpClient.PostAsync(endpoint, stringContent);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var actualStatusCode = (int)httpResponse.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Then(@"the response body should contain the token")]
        public async Task ThenTheResponseBodyShouldContainTheToken()
        {
            var responseBody = await httpResponse.Content.ReadAsStringAsync();
            var tokenObject = JsonConvert.DeserializeObject<JObject>(responseBody);
            token = tokenObject["token"].ToString();

            Assert.IsNotNull(token);
        }

        [Given(@"I have the following invalid login details:")]
        public void GivenIHaveTheFollowingInvalidLoginDetails(Table table)
        {
            var loginDetails = table.Rows.First();
            createUserRequest = new CreateUserRequest
            {
                Email = loginDetails["email"],
                Password = loginDetails["password"]
            };
        }

        [When(@"I make a POST request to ""(.*)"" with the invalid login details")]
        public async Task WhenIMakeAPostRequestToWithTheInvalidLoginDetails(string endpoint)
        {
            var httpClient = new HttpClient();

            var baseAddress = new Uri("https://reqres.in/api/");
            httpClient.BaseAddress = baseAddress;

            var serializedBody = JsonConvert.SerializeObject(createUserRequest);
            var stringContent = new StringContent(serializedBody, Encoding.UTF8, "application/json");

            httpResponse = await httpClient.PostAsync(endpoint, stringContent);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string expectedStatusCode)
        {
            var statusCode = (int)httpResponse.StatusCode;
            Assert.AreEqual(expectedStatusCode, statusCode.ToString());
        }

        [Then(@"the response body should contain the error message: ""(.*)""")]
        public void ThenTheResponseBodyShouldContainTheErrorMessage(string expectedErrorMessage)
        {
            var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(responseContent.Contains(expectedErrorMessage));
        }
    }
}
