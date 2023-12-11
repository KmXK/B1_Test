using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Task_01.Helpers;
using Task_01.Services.Interfaces;

namespace Task_01;

public class Application(
        IFileMerger fileMerger,
        IFileGenerator fileGenerator,
        IDataImporter dataImporter,
        IHostApplicationLifetime lifetime,
        ILogger<Application> logger)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        WorkAsync(cancellationToken).ContinueWith((_, _) =>
        {
            lifetime.StopApplication();
        }, null, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task WorkAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            Console.WriteLine("Choose option to perform:");
            Console.WriteLine("1. Generate text files.");
            Console.WriteLine("2. Merge generated text files.");
            Console.WriteLine("3. Import files to database.");
            Console.WriteLine("4. Exit");

            Console.Write("Choose option to continue: ");
            
            var value = Console.ReadLine()!;
            
            switch (value)
            {
                case "1":
                    await GenerateFilesAsync(cancellationToken);
                    break;
                case "2":
                    await MergeFilesAsync(cancellationToken);
                    break;
                case "3":
                    await ImportFilesAsync(cancellationToken);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid number. Expected integer 1-4.");
                    break;
            }
        }
    }

    private static DirectoryInfo? GetDirectory(string message)
    {
        Console.Write(message);

        var input = Console.ReadLine()!;

        try
        {
            var directory = new DirectoryInfo(input);
            
            directory.Create();
            
            return directory;
        }
        catch
        {
            Console.WriteLine("Invalid directory name was found. Try again.");
            return null;
        }
    }

    private FileInfo[]? GetFilesList(string message, bool mustExist = false)
    {
        Console.Write(message);
        
        var files = Console.ReadLine()!.Split(' ');

        if (files.Length == 0)
        {
            Console.WriteLine("You didn't enter any file. Try again.");
            return null;
        }

        try
        {
            var fileInfos = files.Select(x => new FileInfo(x)).ToArray();

            if (mustExist && fileInfos.Any(x => x.Exists == false))
            {
                Console.WriteLine("File must exists. Try again.");
                return null;
            }
            
            return fileInfos;
        }
        catch
        {
            Console.WriteLine("Invalid file name was found. Try again.");
            return null;
        }
    }

    private async Task ImportFilesAsync(CancellationToken cancellationToken)
    {
        var files = GetFilesList("Enter files to import: ", true);

        if (files == null)
        {
            return;
        }
        
        logger.LogInformation("File importing started.");

        await dataImporter.ImportAsync(files, cancellationToken);
        
        logger.LogInformation("File importing has been completed.");
    }

    private async Task MergeFilesAsync(CancellationToken cancellationToken)
    {
        var files = GetFilesList("Enter files' names to merge: ", true);

        if (files == null) return;

        var mergeFiles = GetFilesList("Enter file name to save result: ");

        if (mergeFiles == null) return;

        if (mergeFiles.Length != 1)
        {
            Console.WriteLine("Invalid output files count. Expected: 1.");
            return;
        }

        Console.Write("Enter value to filter files: ");
        
        var excludeValue = Console.ReadLine();

        if (excludeValue == null)
        {
            Console.WriteLine("Invalid input. Try again.");
            return;
        }
        
        logger.LogInformation("File merging started.");

        var count = await fileMerger.MergeAsync(
            files,
            mergeFiles[0].FullName,
            FileMergerPredicates.NotContainsValue(excludeValue),
            cancellationToken);

        Console.WriteLine($"{count} lines contain string \"{excludeValue}\" and have been excluded.");
        
        logger.LogInformation("File merging has been completed.");
    }

    private async Task GenerateFilesAsync(CancellationToken cancellationToken)
    {
        var directory = GetDirectory("Enter directory to store files: ");
        if (directory == null)
        {
            return;
        }
        
        logger.LogInformation("File generation started.");

        Console.WriteLine("File generation started.");
        
        await fileGenerator.CreateFilesAsync(100, directory, cancellationToken: cancellationToken);

        Console.WriteLine("File generation has been completed.");
        
        logger.LogInformation("File generation has been completed.");
    }
}