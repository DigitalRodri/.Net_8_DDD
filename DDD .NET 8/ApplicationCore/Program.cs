using ApplicationCore.Middleware;
using Domain.Interfaces;
using Domain.Profiles;
using Domain.Services;
using Infraestructure.Repository;
using Infraestructure.Repository.Models;
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

builder.Services.AddSystemWebAdapters();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DDDContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DDDConnectionString")));
builder.Services.AddScoped<DDDContext>();

// Dependency injection
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
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
