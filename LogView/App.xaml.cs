using System.Configuration;
using System.Windows;
using CommunityToolkit.Diagnostics;
using LogView.Log;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using Serilog.Core;

namespace LogView;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public sealed partial class App
{
    private readonly IHost _host;

    public App()
    {
        _host = new HostBuilder()
            .ConfigureAppConfiguration(
                (context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                }
            )
            .ConfigureServices(
                (context, services) =>
                {
                    services.AddTransient<ViewModel>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<ILogEntryObservable, LogObserverable>();
                }
            )
            .ConfigureLogging(
                (HostBuilderContext context, ILoggingBuilder logging) =>
                {
                    logging.AddObservable();
                    Logger logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(context.Configuration)
                        .CreateLogger();
                    logging.AddSerilog(logger);
                }
            )
            .Build();
    }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetService<MainWindow>();
        Guard.IsNotNull(mainWindow);
        mainWindow.Show();
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }
    }
}
