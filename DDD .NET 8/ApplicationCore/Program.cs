using ApplicationCore.Middleware;
using Azure.Identity;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Profiles;
using Domain.Services;
using Infrastructure.Repository;
using Infrastructure.Repository.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Get current environment appsettings.json file
string environment = builder.Environment.EnvironmentName;
var Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

var keyVaultEndpoint = new Uri(Configuration["Keys:VaultUri"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

builder.Services.AddSystemWebAdapters();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string DDDConnectionString = builder.Configuration["DDD-Connection-String"];
builder.Services.AddDbContext<DDDContext>(options =>
                options.UseSqlServer(DDDConnectionString));
builder.Services.AddScoped<DDDContext>();

// Dependency injection
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuthorizationHelper, AuthorizationHelper>();
builder.Services.AddAutoMapper(typeof(AccountProfile));

var issuerSigningKey = Configuration["Keys:IssuerSigningKey"];
var validIssuer = Configuration["Keys:ValidIssuer"];

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(cfg => {
    cfg.IncludeErrorDetails = true;
    cfg.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerSigningKey)),
        ValidateIssuer = true,
        ValidIssuer = validIssuer,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LanguageMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapControllers();

app.Run();

public partial class Program { }