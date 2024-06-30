using Domain.Interfaces;
using Domain.Profiles;
using Domain.Services;
using Infraestructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Get current environment appsettings.json file
string environment = builder.Environment.EnvironmentName;
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, true)
    .Build();


builder.Services.AddSystemWebAdapters();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddAutoMapper(typeof(AccountProfile));

var issuerSigningKey = configuration["Keys:IssuerSigningKey"];
var validIssuer = configuration["Keys:ValidIssuer"];

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapControllers();

app.Run();
