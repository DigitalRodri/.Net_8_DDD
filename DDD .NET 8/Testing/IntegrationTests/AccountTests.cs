using Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Testing.IntegrationTests;

[TestClass]
public class AccountTests : WebApplicationFactory<Program>
{
    private static TestContext _testContext = null!;
    private static WebApplicationFactory<Program> _factory;
    private static readonly string _email = "accountController@integrationtest.com";
    private static readonly string _temporaryEmail = "temporaryMail@integrationtest.com";
    private static readonly string _Name = "Name";
    private static readonly string _updatedName = "updatedName";
    private static readonly string _Surname = "Surname";
    private static readonly string _updatedSurname = "updatedSurname";
    private static readonly string _password = "password";
    private static readonly string _title = "Mr";
    private static readonly string _updatedTitle = "Mrs";

    private static HttpClient _regularHttpClient;
    private static HttpClient _authenticatedHttpClient;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        _factory = new WebApplicationFactory<Program>();

        _regularHttpClient = _factory.CreateClient();
        _authenticatedHttpClient = _factory.CreateClient();

        string authenticationJwtToken = GetAuthenticationJwtToken(_authenticatedHttpClient).Result;
        _authenticatedHttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authenticationJwtToken);
    }

    #region POST/GET/PUT/DELETE

    [TestMethod]
    public async Task Create_Get_Update_Delete_Account_OK()
    {
        _testContext.WriteLine(_testContext.TestName);

        // POST Account

        var simpleAccountDto = new SimpleAccountDto
        {
            Email = _temporaryEmail,
            Password = _password,
            Name = _Name,
            Surname = _Surname,
            Title = _title
        };

        HttpResponseMessage createAccountResponse = await _authenticatedHttpClient.PostAsJsonAsync("api/account", simpleAccountDto);
        Assert.AreEqual(HttpStatusCode.Created, createAccountResponse.StatusCode);

        string createAccountResponseString = await createAccountResponse.Content.ReadAsStringAsync();
        var newAccountDto = JsonConvert.DeserializeObject<AccountDto>(createAccountResponseString);
        Assert.AreEqual(_temporaryEmail, newAccountDto.Email);
        Assert.IsNotNull(newAccountDto.UUID);

        // GET Account

        HttpResponseMessage getAccountResponse = await _authenticatedHttpClient.GetAsync("api/account/" + newAccountDto.UUID);
        Assert.AreEqual(HttpStatusCode.OK, getAccountResponse.StatusCode);

        string getAccountResponseString = await getAccountResponse.Content.ReadAsStringAsync();
        var getAccountDto = JsonConvert.DeserializeObject<AccountDto>(getAccountResponseString);
        Assert.AreEqual(newAccountDto.UUID, getAccountDto.UUID);
        Assert.AreEqual(newAccountDto.Name, getAccountDto.Name);
        Assert.AreEqual(newAccountDto.Surname, getAccountDto.Surname);

        // PUT Account

        var updateAccountDto = new UpdateAccountDto
        {
            Email = _temporaryEmail,
            Name = _updatedName,
            Surname = _updatedSurname,
            Title = _updatedTitle
        };

        HttpResponseMessage updateAccountResponse = await _authenticatedHttpClient.PutAsJsonAsync("api/account/" + newAccountDto.UUID, updateAccountDto);
        Assert.AreEqual(HttpStatusCode.OK, updateAccountResponse.StatusCode);

        string updateAccountResponseString = await updateAccountResponse.Content.ReadAsStringAsync();
        var updatedAccountDto = JsonConvert.DeserializeObject<AccountDto>(updateAccountResponseString);
        Assert.AreEqual(_updatedName, updatedAccountDto.Name);
        Assert.AreEqual(_updatedSurname, updatedAccountDto.Surname);

        // DELETE Account

        HttpResponseMessage deleteAccountResponse = await _authenticatedHttpClient.DeleteAsync("api/account/" + newAccountDto.UUID);
        Assert.AreEqual(HttpStatusCode.OK, deleteAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Get_Account_NoContent()
    {
        _testContext.WriteLine(_testContext.TestName);

        HttpResponseMessage getAccountResponse = await _authenticatedHttpClient.GetAsync("api/account/" + Guid.NewGuid());
        Assert.AreEqual(HttpStatusCode.NoContent, getAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Get_Account_BadRequest()
    {
        _testContext.WriteLine(_testContext.TestName);

        HttpResponseMessage getAccountResponse = await _authenticatedHttpClient.GetAsync("api/account/" + Guid.Empty);
        Assert.AreEqual(HttpStatusCode.BadRequest, getAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Create_Account_BadRequest()
    {
        _testContext.WriteLine(_testContext.TestName);

        var simpleAccountDto = new SimpleAccountDto
        {
            Email = string.Empty
        };

        HttpResponseMessage createAccountResponse = await _authenticatedHttpClient.PostAsJsonAsync("api/account", simpleAccountDto);
        Assert.AreEqual(HttpStatusCode.BadRequest, createAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Create_Account_Conflict()
    {
        _testContext.WriteLine(_testContext.TestName);

        var simpleAccountDto = new SimpleAccountDto
        {
            Email = _email,
            Password = _password,
            Name = _Name,
            Surname = _Surname
        };

        HttpResponseMessage createAccountResponse = await _authenticatedHttpClient.PostAsJsonAsync("api/account", simpleAccountDto);
        Assert.AreEqual(HttpStatusCode.Conflict, createAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Update_Account_BadRequest()
    {
        _testContext.WriteLine(_testContext.TestName);

        var updateAccountDto = new UpdateAccountDto
        {
            Email = string.Empty
        };

        HttpResponseMessage updateAccountResponse = await _authenticatedHttpClient.PutAsJsonAsync("api/account/" + Guid.NewGuid(), updateAccountDto);
        Assert.AreEqual(HttpStatusCode.BadRequest, updateAccountResponse.StatusCode);
    }

    [TestMethod]
    public async Task Delete_Account_BadRequest()
    {
        _testContext.WriteLine(_testContext.TestName);

        HttpResponseMessage updateAccountResponse = await _authenticatedHttpClient.DeleteAsync("api/account/" + Guid.Empty);
        Assert.AreEqual(HttpStatusCode.BadRequest, updateAccountResponse.StatusCode);
    }

    #endregion

    #region Authenticate

    [TestMethod]
    public async Task Authenticate_OK()
    {
        _testContext.WriteLine(_testContext.TestName);

        var authenticationDto = new AuthenticationDto
        {
            Email = _email,
            Password = _password
        };

        HttpResponseMessage authenticationResponse = await _regularHttpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.OK, authenticationResponse.StatusCode);

        string result = await authenticationResponse.Content.ReadAsStringAsync();
        Assert.IsFalse(result.IsNullOrEmpty());
    }

    [TestMethod]
    public async Task Authenticate_Unauthorized()
    {
        _testContext.WriteLine(_testContext.TestName);

        var authenticationDto = new AuthenticationDto
        {
            Email = _email,
            Password = "incorrectPassword"
        };

        HttpResponseMessage authenticationResponse = await _regularHttpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.Unauthorized, authenticationResponse.StatusCode);
    }

    [TestMethod]
    public async Task Authenticate_BadRequest()
    {
        _testContext.WriteLine(_testContext.TestName);

        var authenticationDto = new AuthenticationDto
        {
            Email = string.Empty
        };

        HttpResponseMessage authenticationResponse = await _regularHttpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.BadRequest, authenticationResponse.StatusCode);
    }

    #endregion

    [TestMethod]
    public async Task GetAllAccounts_OK()
    {
        _testContext.WriteLine(_testContext.TestName);

        HttpResponseMessage getAllAccountsResponse = await _authenticatedHttpClient.GetAsync("api/account");

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
            Email = _email,
            Password = _password
        };

        HttpResponseMessage authenticationResponse = await httpClient.PostAsJsonAsync("api/account/authentication", authenticationDto);
        Assert.AreEqual(HttpStatusCode.OK, authenticationResponse.StatusCode);

        return await authenticationResponse.Content.ReadAsStringAsync();
    }
}