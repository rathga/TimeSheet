
using System.ComponentModel;
using AxiTimeSheet.CLI;
using AxiTimeSheet.Domain;
using AxiTimeSheet.Domain.Commands;
using AxiTimeSheet.Infrastructure;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

await Host.CreateDefaultBuilder()
    //.ConfigureAppConfiguration((_, config) => config.SetBasePath(AppContext.BaseDirectory))
    .ConfigureServices(ConfigureServices)
    .Build().Services.GetRequiredService<CommandLineInterface>().Run();

void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));
    TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
    services
        .AddSingleton<IDateTimeProvider, DateTimeProvider>()
        .AddSingleton<CommandLineInterface>()
        .AddSingleton<CommandParser>()
        .AddSingleton<ITimeSheetRepository>(_ => new TimeSheetRepository(TimeSpan.FromHours(3.5)))
        .AddAutoMapper(config => config.AddProfile(typeof(CLIProfile)))
        .AddMediatR(typeof(CommandLineInterface));
}
