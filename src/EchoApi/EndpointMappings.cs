using System.Drawing;

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
    private const string API_VERSION = "v1";
    private const string API_BASE_PATH = "/api/" + API_VERSION;
    public static void MapEchoApiV1(this IEndpointRouteBuilder group)
    {
        group.MapGet("/healthz", () => Results.Ok("Healthy")).WithOpenApi();
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

        group.MapGet(API_BASE_PATH, GetHttpRequestContext).WithOpenApi();
        group.MapGet(API_BASE_PATH + "/message", GetAllMessages).RequireAuthorization().WithOpenApi();
        group.MapPost(API_BASE_PATH + "/message", CreateMessage).RequireAuthorization().WithOpenApi();
        group.MapGet(API_BASE_PATH + "/message/{id:int}", GetMessageById).RequireAuthorization().WithOpenApi();
        group.MapPut(API_BASE_PATH + "/message/{id}", UpdateMessage).RequireAuthorization().WithOpenApi();
        group.MapDelete(API_BASE_PATH + "/message/{id}", DeleteMessage).RequireAuthorization().WithOpenApi();
    }

    private static IResult GetHttpRequestContext(HttpContext context)
    {
        var cookies = context.Request.Cookies;
        var method = context.Request.Method;
        var headers = context.Request.Headers;
        var path = context.Request.Path;
        var subdomains = context.Request.Host;
        var connectionMethods = context.Connection.RemotePort;
        var protocol = context.Request.Protocol;
        var query = context.Request.QueryString;
        var osHostName = System.Environment.MachineName;

        string ip;
        string ips;
        try
        {
            ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            ips = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
        catch (System.Net.Sockets.SocketException)
        {
            ip = "Unknown";
            ips = "Unknown";
        }

        return Results.Ok(new
        {
            cookies,
            method,
            headers,
            path,
            subdomains,
            connectionMethods,
            protocol,
            query,
            ip,
            ips,
            osHostName
        });
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
        var USERNAME = Environment.GetEnvironmentVariable("USERNAME") ?? "admin";
        //builder.Configuration["AppSettings:Authentication:Username"];

        var PASSWORD = Environment.GetEnvironmentVariable("PASSWORD") ?? "admin123";
        //builder.Configuration["AppSettings:Authentication:Password"];

        if (credentials.Username != USERNAME || credentials.Password != PASSWORD)
        {
            return false;
        }
        return true;
    }
}
