using Task_01.ContentGenerator;

namespace Task_01;

public static class Application
{
    public static async Task RunAsync()
    {
        var folder = Path.Combine(Environment.CurrentDirectory, "files");
        var directoryInfo = Directory.CreateDirectory(folder);
        
        await GenerateFilesAsync(directoryInfo);
        await MergeFilesAsync(directoryInfo);
    }

    private static async Task MergeFilesAsync(DirectoryInfo directoryInfo)
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss.fff}] Merging started.");
        
        var files = directoryInfo.EnumerateFiles();
        var outputFileName = Path.Combine(Environment.CurrentDirectory, "merge.txt");

        await FileMerger.Merge(files, outputFileName, FileMergerPredicates.NotContainsValue("2023"));
        
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss.fff}] Merging has been completed.");
    }

    private static async Task GenerateFilesAsync(DirectoryInfo directoryInfo)
    {
        Console.WriteLine("File generation started.");

        var fileGenerator = new FileGenerator(
            new RandomContentGenerator(),
            linesCount: 100_000);

        await fileGenerator.CreateFilesAsync(100, directoryInfo);

        Console.WriteLine("File generation has been completed.");
    }
}