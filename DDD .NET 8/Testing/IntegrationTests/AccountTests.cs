using Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Testing.Helpers;

namespace Testing.IntegrationTests;

[TestClass]
public class AccountTests : WebApplicationFactory<Program>
{
    private const string AdminEmail = "accountController@integrationtest.com";
    private const string AdminPassword = "password";

    private static TestContext s_testContext;
    private static WebApplicationFactory<Program> s_factory;
    private static HttpClient s_regularHttpClient;
    private static HttpClient s_authenticatedHttpClient;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        s_testContext = testContext;
        s_factory = new WebApplicationFactory<Program>();

        s_regularHttpClient = s_factory.CreateClient();
        s_authenticatedHttpClient = s_factory.CreateClient();

        string authenticationJwtToken = GetAuthenticationJwtToken(s_authenticatedHttpClient).Result;
        s_authenticatedHttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authenticationJwtToken);
    }

    #region POST/GET/PUT/DELETE

    [TestMethod]
    public async Task Create_Get_Update_Delete_Account_OK()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        // POST Account

        HttpResponseMessage createAccountResponse = await s_authenticatedHttpClient.PostAsJsonAsync("api/account", ObjectHelper.GetSimpleAccountDto());
        Assert.AreEqual(HttpStatusCode.Created, createAccountResponse.StatusCode);

        string createAccountResponseString = await createAccountResponse.Content.ReadAsStringAsync();
        var newAccountDto = JsonConvert.DeserializeObject<AccountDto>(createAccountResponseString);
        Assert.AreEqual(ObjectHelper.Mail, newAccountDto.Email);
        Assert.IsNotNull(newAccountDto.UUID);

        // GET Account

        HttpResponseMessage getAccountResponse = await s_authenticatedHttpClient.GetAsync("api/account/" + newAccountDto.UUID);
        Assert.AreEqual(HttpStatusCode.OK, getAccountResponse.StatusCode);

        string getAccountResponseString = await getAccountResponse.Content.ReadAsStringAsync();
        var getAccountDto = JsonConvert.DeserializeObject<AccountDto>(getAccountResponseString);
        Assert.AreEqual(newAccountDto.UUID, getAccountDto.UUID);
        Assert.AreEqual(newAccountDto.Name, getAccountDto.Name);
        Assert.AreEqual(newAccountDto.Surname, getAccountDto.Surname);

        // PUT Account

        HttpResponseMessage updateAccountResponse = await s_authenticatedHttpClient.PutAsJsonAsync("api/account/" + newAccountDto.UUID, ObjectHelper.GetUpdateAccountDto());
        Assert.AreEqual(HttpStatusCode.OK, updateAccountResponse.StatusCode);

        string updateAccountResponseString = await updateAccountResponse.Content.ReadAsStringAsync();
        var updatedAccountDto = JsonConvert.DeserializeObject<AccountDto>(updateAccountResponseString);
        Assert.AreEqual(ObjectHelper.UpdatedName, updatedAccountDto.Name);
        Assert.AreEqual(ObjectHelper.UpdatedSurname, updatedAccountDto.Surname);

        // DELETE Account

        HttpResponseMessage deleteAccountResponse = await s_authenticatedHttpClient.DeleteAsync("api/account/" + newAccountDto.UUID);
        Assert.AreEqual(HttpStatusCode.OK, deleteAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Get_Account_NoContent()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        HttpResponseMessage getAccountResponse = await s_authenticatedHttpClient.GetAsync("api/account/" + Guid.NewGuid());
        Assert.AreEqual(HttpStatusCode.NoContent, getAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Get_Account_BadRequest()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        HttpResponseMessage getAccountResponse = await s_authenticatedHttpClient.GetAsync("api/account/" + Guid.Empty);
        Assert.AreEqual(HttpStatusCode.BadRequest, getAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Create_Account_BadRequest()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        var simpleAccountDto = new SimpleAccountDto
        {
            Email = string.Empty
        };

        HttpResponseMessage createAccountResponse = await s_authenticatedHttpClient.PostAsJsonAsync("api/account", simpleAccountDto);
        Assert.AreEqual(HttpStatusCode.BadRequest, createAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Create_Account_Conflict()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        var simpleAccountDto = new SimpleAccountDto
        {
            Email = AdminEmail,
            Password = AdminPassword,
            Name = ObjectHelper.Name,
            Surname = ObjectHelper.Surname
        };

        HttpResponseMessage createAccountResponse = await s_authenticatedHttpClient.PostAsJsonAsync("api/account", simpleAccountDto);
        Assert.AreEqual(HttpStatusCode.Conflict, createAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Update_Account_BadRequest()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        var updateAccountDto = new UpdateAccountDto
        {
            Email = string.Empty
        };

        HttpResponseMessage updateAccountResponse = await s_authenticatedHttpClient.PutAsJsonAsync("api/account/" + Guid.NewGuid(), updateAccountDto);
        Assert.AreEqual(HttpStatusCode.BadRequest, updateAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Delete_Account_BadRequest()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        HttpResponseMessage updateAccountResponse = await s_authenticatedHttpClient.DeleteAsync("api/account/" + Guid.Empty);
        Assert.AreEqual(HttpStatusCode.BadRequest, updateAccountResponse.StatusCode);
    }

    #endregion

    #region Authenticate

    [TestMethod]
    public async Task Authenticate_OK()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        var authenticationDto = new AuthenticationDto
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        HttpResponseMessage authenticationResponse = await s_regularHttpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.OK, authenticationResponse.StatusCode);

        string result = await authenticationResponse.Content.ReadAsStringAsync();
        Assert.IsFalse(result.IsNullOrEmpty());
    }

    [TestMethod]
    public async Task Authenticate_Unauthorized()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        var authenticationDto = new AuthenticationDto
        {
            Email = AdminEmail,
            Password = "incorrectPassword"
        };

        HttpResponseMessage authenticationResponse = await s_regularHttpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.Unauthorized, authenticationResponse.StatusCode);
    }

    [TestMethod]
    public async Task Authenticate_BadRequest()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        var authenticationDto = new AuthenticationDto
        {
            Email = string.Empty
        };

        HttpResponseMessage authenticationResponse = await s_regularHttpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.BadRequest, authenticationResponse.StatusCode);
    }

    #endregion

    [TestMethod]
    public async Task GetAllAccounts_OK()
    {
        s_testContext.WriteLine(s_testContext.TestName);

        HttpResponseMessage getAllAccountsResponse = await s_authenticatedHttpClient.GetAsync("api/account");

        Assert.AreEqual(HttpStatusCode.OK, getAllAccountsResponse.StatusCode);

        string getAllAccountsResponseString = await getAllAccountsResponse.Content.ReadAsStringAsync();
        var accountDtoList = JsonConvert.DeserializeObject<IEnumerable<AccountDto>>(getAllAccountsResponseString);
        Assert.IsTrue(accountDtoList.Count() > 0);
        Assert.IsNotNull(accountDtoList.First().UUID);
    }

    private static async Task<string> GetAuthenticationJwtToken(HttpClient httpClient)
    {
        var authenticationDto = new AuthenticationDto
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        HttpResponseMessage authenticationResponse = await httpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.OK, authenticationResponse.StatusCode);

        return await authenticationResponse.Content.ReadAsStringAsync();
    }
}