using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Task_01;
using Task_01.Options;
using Task_01.Persistence;
using Task_01.Services;
using Task_01.Services.Interfaces;

var hostBuilder = Host.CreateApplicationBuilder();

var configuration = hostBuilder.Configuration;

hostBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("Application"));
});

hostBuilder.Logging
    .ClearProviders()
    .AddSimpleConsole();

hostBuilder.Services.Configure<DataImporterOptions>(configuration.GetSection("Importer"));

hostBuilder.Services.AddSingleton<IContentGenerator, RandomContentGenerator>();
hostBuilder.Services.AddTransient<IFileGenerator, FileGenerator>();
hostBuilder.Services.AddTransient<IFileMerger, FileMerger>();
hostBuilder.Services.AddTransient<IDataImporter, DataImporter>();

hostBuilder.Services.AddHostedService<Application>();

var host = hostBuilder.Build();

await host.Services.GetService<ApplicationDbContext>()!.Database.MigrateAsync();

await host.RunAsync();
