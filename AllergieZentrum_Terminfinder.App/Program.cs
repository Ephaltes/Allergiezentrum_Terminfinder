// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Quartz;

using Refit;

using Serilog;
using Serilog.Core;

namespace AllergieZentrum_Terminfinder.App;

public static class Program
{
    private static IConfiguration _configuration;

    public static async Task Main(string[] args)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
                                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                        .AddJsonFile("AppSettings.json", true, true);

        _configuration = builder.Build();

        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
                   .UseSerilog()
                   .ConfigureServices(ConfigureService);
    }

    private static void ConfigureService(HostBuilderContext hostContext, IServiceCollection services)
    {
        AppointmentsConfiguration appointmentsConfiguration = new();
        _configuration.GetSection(nameof(AppointmentsConfiguration)).Bind(appointmentsConfiguration);

        services.Configure<AppointmentsConfiguration>(_configuration.GetSection(nameof(AppointmentsConfiguration)));

        services.AddRefitClient<IAppointmentsService>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://aceto365.com"));

        Logger logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(_configuration)
                        .CreateLogger();

        services.AddSerilog(logger);

        // base configuration for DI
        services.AddQuartz(quartz =>
                           {
                               // quickest way to create a job with single trigger is to use ScheduleJob
                               quartz.ScheduleJob<CheckAppointmentsJob>(trigger => trigger.StartAt(DateTimeOffset.UtcNow)
                                                                                          .WithDailyTimeIntervalSchedule(appointmentsConfiguration.CheckIntervalInSeconds,
                                                                                              IntervalUnit.Second));
                           });

        // Quartz.Extensions.Hosting hosting
        services.AddQuartzHostedService(options =>
                                        {
                                            // when shutting down we want jobs to complete gracefully
                                            options.WaitForJobsToComplete = true;

                                            // when we need to init another IHostedServices first
                                            options.StartDelay = TimeSpan.FromSeconds(10);
                                        });
    }
}