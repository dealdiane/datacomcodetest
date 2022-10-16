using Datacom.PayslipGenerator;
using Datacom.PayslipGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);

        services.AddHostedService<ConsoleHostedService>();
        services.AddDefaultPayslipGeneratorServices();
    });

await builder.Build().RunAsync();