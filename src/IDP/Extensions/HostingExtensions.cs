using IDP.Infrastructure.Domains;
using IDP.Infrastructure.Repositories;
using IDP.Presentation;
using IDP.Services.EmailService;

using Serilog;

namespace IDP.Extensions;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddConfigurationSettings(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(Program));
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddScoped<IEmailSender, SmtpMailService>();

        builder.Services.ConfigureCookiePolicy();

        builder.Services.ConfigureCors();

        builder.Services.ConfigureIdentity(builder.Configuration);

        builder.Services.ConfigureIdentityServer(builder.Configuration);

        builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
        builder.Services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddApplicationPart(typeof(AssemblyReference).Assembly);

        builder.Services.ConfigureSwagger(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();

        app.UseCors("CorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IDP"));

        app.UseRouting();

        app.UseCookiePolicy();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages().RequireAuthorization();
        });

        return app;
    }
}
