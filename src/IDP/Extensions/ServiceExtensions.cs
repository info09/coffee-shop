using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace IDP.Extensions;

public static class ServiceExtensions
{
    public static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", p =>
            {
                p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
            var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console().ReadFrom.Configuration(context.Configuration)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        });
    }

    public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentitySqlConnection");
        services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseInformationEvents = true;
            })
#if DEBUG
            .AddDeveloperSigningCredential()
#endif
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            //.AddInMemoryApiScopes(Config.ApiScopes)
            //.AddInMemoryClients(Config.Clients)
            //.AddInMemoryApiResources(Config.ApiResources)
            //.AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("IDP"));
            })
            .AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("IDP"));
                opt.EnableTokenCleanup = true;
                opt.TokenCleanupInterval = 3600; // every hour
            });
    }
}
