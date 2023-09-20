using APITesting.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace APITesting.Steps
{
    [Binding]
    public class CreateUserSteps
    {
        private static readonly string ENDPOINT = "https://reqres.in/api/users";
        private HttpResponseMessage httpResponse;
        private CreateUserRequest createUserRequest;

        [Given(@"I populate the API call with ""(.*)"", ""(.*)"", and ""(.*)""")]
        public void GivenIPopulateTheAPICallWithAnd(string fName, string lName, string work)
        {
            createUserRequest = new CreateUserRequest { FirstName = fName, LastName = lName, Job = work };
        }

        [When(@"I make the API call to create a new user")]
        public async Task WhenIMakeTheAPICallToCreateANewUserAsync()
        {
            var httpClient = new HttpClient();

            var serialized = JsonConvert.SerializeObject(createUserRequest);
            var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");

            httpResponse = await httpClient.PostAsync(ENDPOINT, stringContent);
        }

        [Then(@"the call is successful")]
        public void ThenTheCallIsSuccessful()
        {
            Assert.AreEqual(HttpStatusCode.Created, httpResponse.StatusCode);
        }

        [Then(@"the user profile is created")]
        public void ThenTheUserProfileIsCreatedAsync()
        {
            var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
            var createdUser = JsonConvert.DeserializeObject<CreateUserRequest>(responseContent);

            Assert.NotNull(createdUser);
            Assert.AreEqual("John", createdUser.FirstName);
            Assert.AreEqual("Tom", createdUser.LastName);
            Assert.AreEqual("QA", createdUser.Job);
        }
    }
}
