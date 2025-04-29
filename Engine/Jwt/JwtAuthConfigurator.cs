using System.Security.Claims;
using System.Text.Json;
using Firebase_Auth.Helper.Response;
using Firebase_Auth.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Firebase_Auth.Engine.Jwt;
public class JwtAuthConfigurator
{
    private readonly ILogger<JwtAuthConfigurator> _logger;
    private readonly IConfiguration _configuration;

    public JwtAuthConfigurator(ILogger<JwtAuthConfigurator> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IServiceCollection Configure(IServiceCollection services)
    {
        _logger.LogInformation("Configuring Firebase JWT authentication");

        var projectId = _configuration["Firebase:ProjectId"];
        var issuer = $"https://securetoken.google.com/{projectId}";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = projectId,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async context =>
                    {
                        var jwksManager = context.HttpContext.RequestServices.GetRequiredService<FirebaseJwksManager>();
                        context.Options.TokenValidationParameters.IssuerSigningKeys = await jwksManager.GetSigningKeysAsync();
                    },
                    OnAuthenticationFailed = context =>
                    {
                        _logger.LogError(context.Exception, "JWT authentication failed");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context => WriteErrorResponse<object>(
                        context.Response,
                        StatusCodes.Status403Forbidden,
                        "Access denied. The authenticated user does not have sufficient permissions."
                    ),
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        return WriteErrorResponse<object>(
                            context.Response,
                            StatusCodes.Status401Unauthorized,
                            "Authentication failed. Please provide a valid token."
                        );
                    },
                    OnTokenValidated = context =>
                    {
                        if (context.Principal?.Identity is ClaimsIdentity identity && _logger.IsEnabled(LogLevel.Debug))
                        {
                            var claimsDictionary = identity.Claims.ToDictionary(c => c.Type, c => c.Value);
                            _logger.LogDebug("Token validated with claims: {@Claims}", claimsDictionary);
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        _logger.LogInformation("Firebase JWT authentication configuration completed");
        return services;
    }

    private static Task WriteErrorResponse<T>(HttpResponse response, int statusCode, string message, IEnumerable<string>? errors = null)
    {
        response.StatusCode = statusCode;
        response.ContentType = "application/json";
        return response.WriteAsync(JsonSerializer.Serialize(new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        }));
    }
}