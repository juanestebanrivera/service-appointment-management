using System.Text;
using Appointments.Api.Shared;
using Appointments.Domain.Users;
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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = GetTokenValidationParameters(jwtConfig);
            options.Events = GetJwtBearerEvents();
        });

        services.AddSingleton(jwtConfig);
        services.AddAuthorizationBuilder()
                .AddPolicy(AuthenticationPolicies.OnlyAdmin, policy => policy.RequireRole(nameof(UserRole.Admin)));

        return services;
    }

    private static TokenValidationParameters GetTokenValidationParameters(JwtConfiguration jwtConfig)
    {
        string key = jwtConfig.Secret;
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(jwtConfig.Secret));

        return new TokenValidationParameters
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
    }

    private static JwtBearerEvents GetJwtBearerEvents()
    {
        return new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var problemResult = Results.Problem(
                    title: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized,
                    detail: "Invalid or missing authentication token. Please provide a valid token to access this resource."
                );

                await problemResult.ExecuteAsync(context.HttpContext);
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                var problemResult = Results.Problem(
                    title: "Forbidden",
                    statusCode: StatusCodes.Status403Forbidden,
                    detail: "You don't have permission to access this resource."
                );

                await problemResult.ExecuteAsync(context.HttpContext);
            }
        };
    }
}