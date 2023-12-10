using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Task_01.Helpers;
using Task_01.Services.Interfaces;

namespace Task_01;

public class Application(
        IFileMerger fileMerger,
        IFileGenerator fileGenerator,
        IDataImporter dataImporter,
        ILogger<Application> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var folder = Path.Combine(Environment.CurrentDirectory, "files");
        var directoryInfo = Directory.CreateDirectory(folder);

        await GenerateFilesAsync(directoryInfo, cancellationToken);
        await MergeFilesAsync(directoryInfo, cancellationToken);
        await ImportFilesAsync(directoryInfo, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ImportFilesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("File importing started.");
        
        var files = directoryInfo.EnumerateFiles();

        await dataImporter.ImportAsync(files, cancellationToken);
        
        logger.LogInformation("File importing has been completed.");
    }

    private async Task MergeFilesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("File merging started.");
        
        var files = directoryInfo.EnumerateFiles();
        var outputFileName = Path.Combine(Environment.CurrentDirectory, "merge.txt");

        await fileMerger.MergeAsync(
            files,
            outputFileName,
            FileMergerPredicates.NotContainsValue("2023"),
            cancellationToken);
        
        logger.LogInformation("File merging has been completed.");
    }

    private async Task GenerateFilesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("File generation started.");

        await fileGenerator.CreateFilesAsync(100, directoryInfo, cancellationToken: cancellationToken);

        logger.LogInformation("File generation has been completed.");
    }
}