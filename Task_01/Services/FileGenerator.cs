using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Task_01.Helpers;
using Task_01.Services.Interfaces;

namespace Task_01.Services;

public class FileGenerator(
        IContentGenerator contentGenerator,
        ILogger<FileGenerator> logger)
    : IFileGenerator
{
    private const int LinesCount = 100_000;
    
    public async Task CreateFilesAsync(
        int count,
        DirectoryInfo directoryInfo,
        string fileNamePrefix = "file_",
        CancellationToken cancellationToken = default)
    {
        var currentCount = 0;

        var consoleLock = new object();
        
        await Parallel.ForAsync(0, count, cancellationToken, async (i, c) =>
        {
            c.ThrowIfCancellationRequested();
            
            var path = Path.Combine(directoryInfo.FullName, $"{fileNamePrefix}{i}");
            await CreateFileAsync(path, c);
            
            lock (consoleLock)
            {
                currentCount++;
                ConsoleHelper.WriteLine($"File [{currentCount}/100] has been generated.", ConsoleColor.Green);
            }
        });
    }
    
    public async Task CreateFileAsync(string fullName, CancellationToken cancellationToken = default)
    {
        var text = contentGenerator.Generate(LinesCount, Environment.NewLine);
        
        logger.LogDebug("Content for file {fileName} has been generated.", fullName);
        
        await File.WriteAllTextAsync(fullName, text, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        
        logger.LogInformation("File {fileName} has been generated.", fullName);
    }
}