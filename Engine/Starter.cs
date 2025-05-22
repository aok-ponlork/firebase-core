using Firebase_Auth.Context;
using Firebase_Auth.Data.Models.Authentication.DTO;
using Firebase_Auth.Engine.Jwt;
using Firebase_Auth.Helper.Firebase.FCM;
using Firebase_Auth.Infrastructure.MessageQueue.Interface;
using Firebase_Auth.Infrastructure.MessageQueue.Settings;
using Firebase_Auth.Infrastructure.Security;
using Firebase_Auth.Services;
using Firebase_Auth.Services.Authentication;
using Firebase_Auth.Services.Authentication.Interfaces;
using Firebase_Auth.Services.Authorization.Interfaces;
using Firebase_Auth.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Firebase_Auth.Engine
{
    public static class Starter
    {
        public static void StartEngine(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddHttpContextAccessor();
            ConfigureDatabase(services, configuration);
            InitMQSetting(configuration, services);
            RegisterServices(services);
            RolePermissionSetUp(services);
            RegisterFirebaseService(services, configuration);
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
                }).ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            }, poolSize: 4);

            services.AddScoped(p => p.GetRequiredService<IDbContextFactory<CoreDbContext>>().CreateDbContext());
        }
        //Register Service method
        private static void RegisterServices(IServiceCollection services)
        {
            //Jwk Key 
            services.AddSingleton<FirebaseJwksManager>();
            services.AddScoped<NotificationHelper>();
            services.AddAutoMapper(typeof(Starter));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICookieManage, CookieManagerService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationTopicService, NotificationTopicService>();
            services.AddHttpClient("httpClient", client => { client.Timeout = TimeSpan.FromSeconds(15); });
            //Jwt 
            services.AddSingleton<JwtAuthConfigurator>();
            // Configure JWT Authentication (after the app is built)
            var jwtConfigurator = services.BuildServiceProvider().GetRequiredService<JwtAuthConfigurator>();
            jwtConfigurator.Configure(services);
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
            services.AddSingleton(FirebaseMessaging.DefaultInstance);
        }

        public static async Task SeedRoodUser(IServiceProvider services)
        {
            try
            {
                var userManager = services.GetRequiredService<IAuthService>();
                var roleManager = services.GetRequiredService<IRolePermissionService>();
                var config = services.GetRequiredService<IConfiguration>();
                // Get the RootUser section from appsettings
                var rootUserConfig = config.GetSection("RootUser");
                var registerRequest = new RegisterRequest
                {
                    Email = rootUserConfig["Email"]!,
                    Password = rootUserConfig["Password"]!,
                    UserName = rootUserConfig["UserName"]!
                };
                var user = await userManager.RegisterWithEmailAndPasswordAsync(registerRequest);
                await roleManager.AssignRoleToUserAsync((Guid)user.Id!, Guid.Parse("99999999-9999-9999-9999-999999999999"));
            }
            catch (Exception)
            {
                return;
            }
        }

        private static void RolePermissionSetUp(IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
                .AddPolicy("EditorPolicy", policy => policy.RequireRole("Editor"))
                .AddPolicy("UserPolicy", policy => policy.RequireRole("User"))
                .AddPolicy("CreateContent", policy => policy.RequireClaim("permission", "CreateContent"))
                .AddPolicy("EditContent", policy => policy.RequireClaim("permission", "EditContent"))
                .AddPolicy("DeleteContent", policy => policy.RequireClaim("permission", "DeleteContent"))
                .AddPolicy("ViewContent", policy => policy.RequireClaim("permission", "ViewContent"));
        }

        private static void InitMQSetting(ConfigurationManager configuration, IServiceCollection services)
        {
            // Keep the existing configuration binding
            services.Configure<QueueSettings>(configuration.GetSection("RabbitMQSettings"));
            // Register your consumer and publisher
            services.AddSingleton<IRabbitConnectionManager, RabbitConnectionManager>();
            services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
            services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
            services.AddHostedService<RabbitMqConsumerHostedService>();
        }

    }
}