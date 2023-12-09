using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Task_01;
using Task_01.Services;
using Task_01.Services.Interfaces;

var hostBuilder = Host.CreateApplicationBuilder();

hostBuilder.Logging
    .ClearProviders()
    .AddSimpleConsole();

hostBuilder.Services.AddSingleton<IContentGenerator, RandomContentGenerator>();
hostBuilder.Services.AddTransient<IFileGenerator, FileGenerator>();
hostBuilder.Services.AddTransient<IFileMerger, FileMerger>();

hostBuilder.Services.AddHostedService<Application>();

await hostBuilder.Build().RunAsync();
