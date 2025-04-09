using Firebase_Auth.Context;
using Firebase_Auth.Services.Authentication;
using Firebase_Auth.Services.Authentication.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;

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
            services.AddAutoMapper(typeof(Starter));
            services.AddScoped<IAuthService, AuthService>();
        }

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
    }
}