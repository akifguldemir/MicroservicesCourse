using System.Globalization;
using System.Text;
using Duende.IdentityServer.Licensing;
using FreeCourse.IdentityServer;
using FreeCourse.IdentityServer.Data;
using FreeCourse.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", formatProvider: CultureInfo.InvariantCulture)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    // this seeding is only for the template to bootstrap the DB and users.
    // in production you will likely want a different approach.
    //if (args.Contains("/seed"))
    //{
    //    Log.Information("Seeding database...");
    //    SeedData.EnsureSeedData(app);
    //    Log.Information("Done seeding database. Exiting.");
    //    return;
    //}


    //Auto Migration
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Veritabaný migration iþlemi
        await appDbContext.Database.MigrateAsync();

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!userManager.Users.Any())
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@localhost",
                City = "Mugla"
            };

            // Kullanýcýyý asenkron þekilde oluþturma
            var result = await userManager.CreateAsync(user, "Akif1905+");

            if (result.Succeeded)
            {
                Log.Information("Admin user created successfully.");
            }
            else
            {
                Log.Error("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            Log.Information("Admin user already exists.");
        }
    }


    if (app.Environment.IsDevelopment())
    {
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            var usage = app.Services.GetRequiredService<LicenseUsageSummary>();
            Console.Write(Summary(usage));
            Console.ReadKey();
        });
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

static string Summary(LicenseUsageSummary usage)
{
    var sb = new StringBuilder();
    sb.AppendLine("IdentityServer Usage Summary:");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  License: {usage.LicenseEdition}");
    var features = usage.FeaturesUsed.Count > 0 ? string.Join(", ", usage.FeaturesUsed) : "None";
    sb.AppendLine(CultureInfo.InvariantCulture, $"  Business and Enterprise Edition Features Used: {features}");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.ClientsUsed.Count} Client Id(s) Used");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.IssuersUsed.Count} Issuer(s) Used");

    return sb.ToString();
}
