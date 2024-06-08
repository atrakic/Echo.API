using Echo.DAL;
using Echo.Model;

using NSwag.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddDbContext<MessageDbContext>(o => o.UseInMemoryDatabase("MyMessageDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<SwaggerGeneratorOptions>(o => { o.InferSecuritySchemes = true; });
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "EchoAPI";
    config.Title = "EchoAPI v1";
    config.Version = "v1";
    config.Description = "A simple API to echo messages";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    return TypedResults.Ok(msgRepository.GetItems());
})
.WithName("GetMessageItems")
.WithOpenApi();

app.MapGet("/api/messages/{id:int}", Results<Ok<MessageItem>, NotFound> ([FromRoute] int id, IMessageRepository msgRepository) =>
{
    var item = msgRepository.GetItem(id);
    return item != null ? TypedResults.Ok(item) : TypedResults.NotFound();
})
.WithName("GetMessageItem")
.WithOpenApi();

//POST a new message item
app.MapPost("/api/messages",
    [Authorize] (MessageItem item, IMessageRepository msgRepository) =>
{
    msgRepository.AddItem(item);
    msgRepository.SaveChanges();
    return Results.Created($"/api/messages/{item.Id}", item);
})
.WithName("PostMessageItem")
.WithOpenApi(operation =>
{
    operation.Security = new List<OpenApiSecurityRequirement>
    {
       JWTSecurity.GetDefaultOpenApiSecurityRequirement()
    };
    return operation;
}
);

// PUT an existing message item
app.MapPut("/api/messages/{id:int}",
    [Authorize] Results<NoContent, NotFound> (
        [FromRoute] int id, MessageItem msgItem, IMessageRepository msgRepository) =>
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
.WithName("UpdateMessageItem")
.WithOpenApi(operation =>
{
    operation.Security = new List<OpenApiSecurityRequirement>
    {
        JWTSecurity.GetDefaultOpenApiSecurityRequirement()
    };
    return operation;
});

// DELETE an existing message item
app.MapDelete("/api/messages/{id:int}",
    [Authorize] Results<NoContent, NotFound> (
        [FromRoute] int id, IMessageRepository msgRepository) =>
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
.WithName("DeleteMessageItem")
.WithOpenApi(operation =>
{
    operation.Security = new List<OpenApiSecurityRequirement>
    {
        JWTSecurity.GetDefaultOpenApiSecurityRequirement()
    };
    return operation;
});

app.Run();
