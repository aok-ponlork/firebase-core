using Firebase_Auth.Context;
using Firebase_Auth.Infrastructure.Security;
using Firebase_Auth.Services.Authentication;
using Firebase_Auth.Services.Authentication.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Firebase_Auth.Engine
{
    public static class Starter
    {
        public static void StartEngine(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            ConfigureDatabase(services, configuration);
            RegisterServices(services);
            RegisterFirebaseService(services, configuration);
            JwtConfiguration(services, configuration);
        }
        //Database configuration method
        private static void ConfigureDatabase(IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

            services.AddPooledDbContextFactory<CoreDbContext>(options =>
            {
                options.UseNpgsql(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null
                    );
                });
            }, poolSize: 4);

            services.AddScoped(p => p.GetRequiredService<IDbContextFactory<CoreDbContext>>().CreateDbContext());
        }
        //Register Service method
        private static void RegisterServices(IServiceCollection services)
        {
            //Jwk Key 
            services.AddSingleton<FirebaseJwksManager>();
            services.AddAutoMapper(typeof(Starter));
            services.AddScoped<IAuthService, AuthService>();
            services.AddHttpClient("httpClient", client => { client.Timeout = TimeSpan.FromSeconds(15); });
        }
        //Register firebase
        private static void RegisterFirebaseService(IServiceCollection services, ConfigurationManager configuration)
        {
            var firebaseCredPath = configuration["Firebase:CredentialsPath"];
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseCredPath),
                });
            }
            services.AddSingleton(FirebaseAuth.DefaultInstance);
        }
        //Set up JWT 
        private static void JwtConfiguration(IServiceCollection services, ConfigurationManager configuration)
        {
            var projectId = configuration["Firebase:ProjectId"];
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
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = async context =>
                        {
                            // Get the FirebaseJwksManager from the service provider
                            var jwksManager = context.HttpContext.RequestServices.GetRequiredService<FirebaseJwksManager>();
                            // Set the signing keys
                            context.Options.TokenValidationParameters.IssuerSigningKeys =
                                await jwksManager.GetSigningKeysAsync();
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception}");
                            return Task.CompletedTask;
                        }
                    };
                }
            );
        }

    }
}