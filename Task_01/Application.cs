using Task_01.ContentGenerator;

namespace Task_01;

public static class Application
{
    public static async Task RunAsync()
    {
        await GenerateFilesAsync();
    }

    private static async Task GenerateFilesAsync()
    {
        Console.WriteLine("File generation started.");

        var folder = Path.Combine(Environment.CurrentDirectory, "files");

        var fileGenerator = new FileGenerator(
            new RandomContentGenerator(),
            linesCount: 100_000);

        await fileGenerator.CreateFilesAsync(100, folder);

        Console.WriteLine("File generation has been completed.");
    }
}