using api;
using api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    // tilføj bearer token auth til swagger ui
    options =>{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "bearer {token}",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// lav logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Month)
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();
// giv adgang til host kan bruge serilog
builder.Host.UseSerilog();

// tilføj auth token skema til at fortælle api hvordan token skal være til auth
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("JWT:Tokken").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
// tilføj singleton til injection service
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddSingleton<IItemData, ItemData>();
builder.Services.AddSingleton<IUserData, UserData>();
builder.Services.AddSingleton<IDepartmentData, DepartmentData>();
builder.Services.AddSingleton<IDepartmentItemData, DepartmentItemData>();

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// brug serilog til at logge alle handlinger i stedet for default logger
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// fortæller api den skal bruge auth
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
