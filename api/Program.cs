using api;
using api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    // tilf�j bearer token auth til swagger ui
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
// tilf�j auth token skema til at fort�lle api hvordan token skal v�re til auth
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
// tilf�j singleton til injection service
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddSingleton<IItemData, ItemData>();
builder.Services.AddSingleton<IUserData, UserData>();
builder.Services.AddSingleton<IDepartmentData, DepartmentData>();

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// fort�ller api den skal bruge auth
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
