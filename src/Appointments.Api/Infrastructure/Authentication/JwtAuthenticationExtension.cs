using System.Security.Claims;
using System.Text;
using Appointments.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Appointments.Api.Infrastructure.Authentication;

public static class JwtAuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection(JwtConfiguration.SectionName).Get<JwtConfiguration>();
        ArgumentNullException.ThrowIfNull(jwtConfig, nameof(jwtConfig));

        string key = jwtConfig.Secret;
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(jwtConfig.Secret));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,

                RequireAudience = true,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                ValidAlgorithms = [SecurityAlgorithms.HmacSha256],

                RoleClaimType = "role"
            };
        });

        services.AddAuthorization();
        services.AddSingleton(jwtConfig);

        return services;
    }
}