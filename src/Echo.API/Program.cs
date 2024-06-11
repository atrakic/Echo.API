using EchoApi.DAL;
using EchoApi.Model;
using EchoApi.Context;

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
builder.Services.AddDbContext<MessageDbContext>(o => o.UseInMemoryDatabase("MyMessageDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks();
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseOpenApi();
}

app.MapHealthChecks("/healthz");
//app.UseHttpsRedirection();

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
.WithName("GetMessageItemId")
.WithOpenApi();

app.MapPost("/api/messages", (MessageItem item, IMessageRepository msgRepository) =>
{
    msgRepository.AddItem(item);
    msgRepository.SaveChanges();
    return Results.Created($"/api/messages/{item.Id}", item);
})
.WithName("PostMessageItem");

app.MapPut("/api/messages/{id:int}", Results<NoContent, NotFound> (int id, MessageItem msgItem, IMessageRepository msgRepository) =>
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
.WithName("UpdateExistingMessageItem");

app.MapDelete("/api/messages/{id:int}", Results<NoContent, NotFound> (int id, IMessageRepository msgRepository) =>
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
.WithName("DeleteExistingMessageItem");

app.Run();

public partial class Program
{ }
