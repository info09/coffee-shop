using IDP.Extensions;
using IDP.Persistence;
using Serilog;

Log.Information("Starting up");
var builder = WebApplication.CreateBuilder(args);
try
{
    builder.Host.AddAppConfigurations();
    builder.Host.ConfigureSerilog();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    SeedUserData.EnsureSeedData(builder.Configuration.GetConnectionString("IdentitySqlConnection")!);

    await app.MigrateDatabaseAsync(builder.Configuration);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}