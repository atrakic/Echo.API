using EchoApi.Auth;
using EchoApi.DAL;
using EchoApi.Model;
using EchoApi.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EchoApi;

public static class EndpointMappings
{
    public static void MapEchoApiV1(this IEndpointRouteBuilder group)
    {
        group.MapGet("/healthz", () => Results.Ok());
        group.MapPost("/token", (TokenService tokenService, [FromBody] UserCredentials credentials) =>
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

        group.MapGet("/", GetAllMessages).RequireAuthorization().WithOpenApi();
        group.MapPost("/", CreateMessage).RequireAuthorization().WithOpenApi();
        group.MapGet("/api/message/{id:int}", GetMessageById).RequireAuthorization().WithOpenApi();
        group.MapPut("/api/message/{id}", UpdateMessage).RequireAuthorization().WithOpenApi();
        group.MapDelete("/api/message/{id}", DeleteMessage).RequireAuthorization().WithOpenApi();
    }

    private static IResult GetAllMessages(IMessageRepository msgRepository)
    {
        int MAX_MESSAGE_ITEMS = 10;
        return TypedResults.Ok(msgRepository.GetItems().Take(MAX_MESSAGE_ITEMS));
    }

    private static IResult GetMessageById(int id, IMessageRepository msgRepository)
    {
        var item = msgRepository.GetItem(id);
        return item != null ? TypedResults.Ok(item) : TypedResults.NotFound();
    }

    private static IResult CreateMessage(Messages item, IMessageRepository msgRepository)
    {
        msgRepository.AddItem(item);
        msgRepository.SaveChanges();
        return Results.Created($"/api/message/{item.Id}", item);
    }

    private static IResult UpdateMessage(int id, Messages msgItem, IMessageRepository msgRepository)
    {
        var existingItem = msgRepository.GetItem(id);

        if (existingItem is null)
        {
            return TypedResults.NotFound();
        }

        existingItem.Message = msgItem.Message;

        msgRepository.UpdateItem(existingItem);
        msgRepository.SaveChanges();
        return TypedResults.NoContent();
    }

    private static IResult DeleteMessage(int id, IMessageRepository msgRepository)
    {
        var existingItem = msgRepository.GetItem(id);

        if (existingItem is null)
        {
            return TypedResults.NotFound();
        }

        msgRepository.RemoveItem(existingItem);
        msgRepository.SaveChanges();
        return TypedResults.NoContent();
    }

    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <param name="credentials">The user credentials to authenticate.</param>
    /// <returns>True if the user is authenticated, otherwise false.</returns>
    private static bool AuthenticateUser(UserCredentials credentials)
    {
        var USERNAME = Environment.GetEnvironmentVariable("USERNAME") ?? "admin";    //builder.Configuration["AppSettings:Authentication:Username"];
        var PASSWORD = Environment.GetEnvironmentVariable("PASSWORD") ?? "admin123"; //builder.Configuration["AppSettings:Authentication:Password"];

        if (credentials.Username != USERNAME || credentials.Password != PASSWORD)
        {
            return false;
        }
        return true;
    }
}
