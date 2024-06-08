namespace Utilities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

internal static class JWTSecurity
{
    static OpenApiSecurityRequirement GetDefaultOpenApiSecurityRequirement()
        => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme,
                },
                    Scheme = SecuritySchemeType.Http.ToString(),
                    Name = JwtBearerDefaults.AuthenticationScheme,
                },
                new List<string>()
            }
        };
}
