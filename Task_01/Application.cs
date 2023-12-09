using Microsoft.Extensions.Hosting;
using Task_01.Helpers;
using Task_01.Services.Interfaces;

namespace Task_01;

public class Application(
        IFileMerger fileMerger,
        IFileGenerator fileGenerator)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var folder = Path.Combine(Environment.CurrentDirectory, "files");
        var directoryInfo = Directory.CreateDirectory(folder);
        
        await GenerateFilesAsync(directoryInfo);
        await MergeFilesAsync(directoryInfo);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task MergeFilesAsync(DirectoryInfo directoryInfo)
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss.fff}] Merging started.");
        
        var files = directoryInfo.EnumerateFiles();
        var outputFileName = Path.Combine(Environment.CurrentDirectory, "merge.txt");

        await fileMerger.MergeAsync(files, outputFileName, FileMergerPredicates.NotContainsValue("2023"));
        
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss.fff}] Merging has been completed.");
    }

    private async Task GenerateFilesAsync(DirectoryInfo directoryInfo)
    {
        Console.WriteLine("File generation started.");

        await fileGenerator.CreateFilesAsync(100, directoryInfo);

        Console.WriteLine("File generation has been completed.");
    }
}