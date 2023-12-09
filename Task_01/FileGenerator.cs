using Task_01.ContentGenerator;

namespace Task_01;

public class FileGenerator(IContentGenerator contentGenerator, int linesCount)
{
    public async Task CreateFilesAsync(int count, DirectoryInfo directoryInfo, string fileNamePrefix = "file_")
    {
        await Parallel.ForAsync(0, count - 1, async (i, _) =>
        {
            var path = Path.Combine(directoryInfo.FullName, $"{fileNamePrefix}{i}");
            await CreateFileAsync(path);
        });
    }
    
    public async Task CreateFileAsync(string name)
    {
        var text = contentGenerator.Generate(linesCount, Environment.NewLine);
        
        await File.WriteAllTextAsync(name, text);
    }
}