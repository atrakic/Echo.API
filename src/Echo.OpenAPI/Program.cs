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
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "EchoAPI";
    config.Title = "EchoAPI v1";
    config.Version = "v1";
    config.Description = "A simple API to echo messages";

    /**
    config.AddSecurity("Bearer", new
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        Bearer = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = OpenApiSecuritySchemeType.ApiKey,
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        }
    });
    */
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


app.Run();
