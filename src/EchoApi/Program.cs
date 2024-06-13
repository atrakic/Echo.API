// How can I generate and issue JWT tokens in an ASP.NET Core application?
// https://stackoverflow.com/questions/38740984/how-can-i-generate-and-issue-jwt-tokens-in-an-asp-net-core-application


// How can I apply a simple JWT token autorisation in an c# minimal api?
// https://stackoverflow.com/questions/69029292/how-can-i-apply-a-simple-jwt-token-autorisation-in-an-c-sharp-minimal-api

using EchoApi.DAL;
using EchoApi.Model;
using EchoApi.Context;
using EchoApi.Services;
//using EchoApi.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using NSwag.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddDbContext<ApiDbContext>(o => o.UseInMemoryDatabase("MyApiDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks();
//builder.Services.AddSingleton<TokenService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseOpenApi();
}

//app.MapSwagger().RequireAuthorization();
app.MapHealthChecks("/healthz");
//app.UseHttpsRedirection();

// curl -X POST http://localhost:5000/token -H "Content-Type: application/json" -d '{"Username":"your_username","Password":"your_password"}'
/**
app.MapPost("/token", (TokenService tokenService, [FromBody] UserCredentials credentials) =>
{
    bool isValidUser = AuthenticateUser(credentials);

    if (isValidUser)
    {
        var token = tokenService.GenerateToken(credentials.Username);
        return Results.Ok(new { token });
    }
    else
    {
        return Results.Unauthorized();
    }
});
*/

app.MapGet("/api/messages", (IMessageRepository msgRepository) =>
{
    int MAX_MESSAGE_ITEMS = 10;
    return TypedResults.Ok(msgRepository.GetItems().Take(MAX_MESSAGE_ITEMS));
})
.WithName("GetAllMessageItems")
.WithOpenApi();

app.MapGet("/api/messages/{id:int}", Results<Ok<MessageItem>, NotFound> (int id, IMessageRepository msgRepository) =>
{
    var item = msgRepository.GetItem(id);
    return item != null ? TypedResults.Ok(item) : TypedResults.NotFound();
})
.RequireAuthorization()
.WithName("GetMessageItemId")
.WithOpenApi();

app.MapPost("/api/messages", (MessageItem item, IMessageRepository msgRepository) =>
{
    msgRepository.AddItem(item);
    msgRepository.SaveChanges();
    return Results.Created($"/api/messages/{item.Id}", item);
})
.RequireAuthorization()
.WithName("PostMessageItem")
.WithOpenApi();

app.MapPut("/api/messages/{id:int}",
    Results<NoContent, NotFound> (int id, MessageItem msgItem, IMessageRepository msgRepository) =>
{
    var existingItem = msgRepository.GetItem(id);

    if (existingItem is null)
    {
        return TypedResults.NotFound();
    }

    existingItem.Name = msgItem.Name;
    msgRepository.UpdateItem(existingItem);

    msgRepository.SaveChanges();
    return TypedResults.NoContent();

})
.RequireAuthorization()
.WithName("UpdateExistingMessageItem")
.WithOpenApi();

app.MapDelete("/api/messages/{id:int}",
    Results<NoContent, NotFound> (int id, IMessageRepository msgRepository) =>
{
    var existingItem = msgRepository.GetItem(id);

    if (existingItem is null)
    {
        return TypedResults.NotFound();
    }

    msgRepository.RemoveItem(existingItem);
    msgRepository.SaveChanges();
    return TypedResults.NoContent();
})
.RequireAuthorization()
.WithName("DeleteExistingMessageItem")
.WithOpenApi();

app.Run();

/**
static bool AuthenticateUser(UserCredentials credentials)
{
    var USERNAME = Environment.GetEnvironmentVariable("USERNAME") ?? "admin";    //builder.Configuration["Authentication:Username"];
    var PASSWORD = Environment.GetEnvironmentVariable("PASSWORD") ?? "admin123"; //builder.Configuration["Authentication:Password"];

    if (credentials.Username != USERNAME || credentials.Password != PASSWORD)
    {
        return false;
    }
    return true;
}
*/

public partial class Program
{ }
