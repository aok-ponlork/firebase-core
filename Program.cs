using System.Text.Json.Serialization;
using Firebase_Auth.Engine;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);
var app = builder.Build();
// Configure the HTTP request pipeline.
ConfigurePipeline(app);

app.Run();
static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.StartEngine(builder.Configuration);
    builder.Services.AddHttpClient();
    builder.Services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
    });
    builder.Services.Configure<FormOptions>(fo =>
    {
        fo.ValueLengthLimit = int.MaxValue;
        fo.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
        fo.MultipartHeadersLengthLimit = int.MaxValue;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please Input Your Token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer",
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Firebase Core",
            Version = "v1"
        });

        options.AddSecurityDefinition("Bearer", securityScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, new string[] {} }
        });
    });
}
static void ConfigurePipeline(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Default");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}
