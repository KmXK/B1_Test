using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Task_02.Persistence;

namespace Task_02;

public partial class App
{
    private readonly IHost _host = new HostBuilder()
        .ConfigureAppConfiguration(ConfigureAppConfiguration)
        .ConfigureServices(ConfigureServices)
        .Build();

    private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
    {
        builder.SetBasePath(context.HostingEnvironment.ContentRootPath);
        builder.AddJsonFile("appsettings.json", optional: false);
    }

    private static void ConfigureServices(
        HostBuilderContext context,
        IServiceCollection services)
    {
        var configuration = context.Configuration;

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Application"));
        });

        services.AddSingleton<MainWindow>();
    }

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetService<MainWindow>()!;
        mainWindow.Show();
    }

    private async void App_OnExit(object sender, ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }
    }
}