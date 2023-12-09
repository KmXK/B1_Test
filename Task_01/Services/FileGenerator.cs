using Task_01.Services.Interfaces;

namespace Task_01.Services;

public class FileGenerator(IContentGenerator contentGenerator) : IFileGenerator
{
    private const int LinesCount = 100_000;
    
    public async Task CreateFilesAsync(int count, DirectoryInfo directoryInfo, string fileNamePrefix = "file_")
    {
        await Parallel.ForAsync(0, count - 1, async (i, _) =>
        {
            var path = Path.Combine(directoryInfo.FullName, $"{fileNamePrefix}{i}");
            await CreateFileAsync(path);
        });
    }
    
    public async Task CreateFileAsync(string fullName)
    {
        var text = contentGenerator.Generate(LinesCount, Environment.NewLine);
        
        await File.WriteAllTextAsync(fullName, text);
    }
}