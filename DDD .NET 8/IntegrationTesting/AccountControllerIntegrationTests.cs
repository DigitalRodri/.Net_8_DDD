using Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTesting;

[TestClass]
public class AccountControllerIntegrationTests : WebApplicationFactory<Program>
{
    private static TestContext _testContext = null!;
    private readonly string _email = "accountController@integrationtest.com";
    private readonly string _password = "password";

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
    }

    [TestMethod]
    public async Task GetAllAccounts_OK()
    {
        Console.WriteLine(_testContext.TestName);

        await using var factory = new WebApplicationFactory<Program>();
        using HttpClient client = factory.CreateClient();

        var authenticationDto = new AuthenticationDto
        {
            Email = _email,
            Password = _password
        };
        HttpResponseMessage authenticationResponse = await client.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.OK, authenticationResponse.StatusCode);
        string authenticationJwtToken = await authenticationResponse.Content.ReadAsStringAsync();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authenticationJwtToken);
        HttpResponseMessage response = await client.GetAsync("api/account");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        string result = await response.Content.ReadAsStringAsync();
        var accountDtoList = JsonConvert.DeserializeObject<IEnumerable<AccountDto>>(result);
        Assert.IsTrue(accountDtoList.Count() > 0);
    }
}