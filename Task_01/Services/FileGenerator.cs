using Microsoft.Extensions.Logging;
using Task_01.Services.Interfaces;

namespace Task_01.Services;

public class FileGenerator(
        IContentGenerator contentGenerator,
        ILogger<FileGenerator> logger)
    : IFileGenerator
{
    private const int LinesCount = 100_000;
    
    public async Task CreateFilesAsync(int count, DirectoryInfo directoryInfo, string fileNamePrefix = "file_")
    {
        await Parallel.ForAsync(0, count, async (i, _) =>
        {
            var path = Path.Combine(directoryInfo.FullName, $"{fileNamePrefix}{i}");
            await CreateFileAsync(path);
        });
    }
    
    public async Task CreateFileAsync(string fullName)
    {
        var text = contentGenerator.Generate(LinesCount, Environment.NewLine);
        
        logger.LogDebug("Content for file {fileName} has been generated.", fullName);
        
        await File.WriteAllTextAsync(fullName, text);
        
        logger.LogInformation("File {fileName} has been generated.", fullName);
    }
}