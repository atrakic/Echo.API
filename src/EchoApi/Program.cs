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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using EchoApi.DAL;
using EchoApi.Model;
using EchoApi.Context;
using EchoApi.Services;
using EchoApi.Auth;

namespace EchoApi;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddLogging();
        builder.Services.AddDbContext<ApiDbContext>(o => o.UseInMemoryDatabase("EchoApiDb"));
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                }
            );

            x.SupportNonNullableReferenceTypes();

            x.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            );
        });

        builder.Services.AddHealthChecks();
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
                    ValidIssuer = builder.Configuration?["AppSettings:Jwt:Issuer"],
                    ValidAudience = builder.Configuration?["AppSettings:Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration?["AppSettings:Jwt:Key"]))
                };
            }
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(
            c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Echo API V1");
            }
        );
        app.UseOpenApi();
        //app.UseHttpsRedirection();
        app.MapEchoApiV1();
        app.Run();
    }
}
