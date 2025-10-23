
using AsZero.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using StdUnit.Zero.Core;
using StdUnit.Zero.DAL;
using System.Runtime.CompilerServices;


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateBootstrapLogger();

try {
    var builder = WebApplication.CreateBuilder(args);




    var startup = new Startup(builder.Configuration, builder.Environment);
    startup.ConfigureServices(builder.Services);


    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    var host = builder.Build();

    // ensure db exists
    using (var scope = host.Services.CreateScope())
    {
        var sp = scope.ServiceProvider;
        await sp.DoHostPreRunHooksAsync();
    }

    // run
    startup.Configure(host, host.Environment);
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex,"An unhandled exception occurred during bootstrapping");
}
finally {
    Log.CloseAndFlush();
}

