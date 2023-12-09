using System.Collections.Concurrent;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task_01.Options;
using Task_01.Persistence;
using Task_01.Persistence.Entities;
using Task_01.Services.Interfaces;

namespace Task_01.Services;

public class DataImporter(
    ApplicationDbContext context,
    IOptions<DataImporterOptions> options,
    ILogger<DataImporter> logger) 
    : IDataImporter
{
    public async Task Import(IEnumerable<FileInfo> files)
    {
        var bag = new ConcurrentBag<FileData>();
        var readSemaphore = new SemaphoreSlim(0);
        var writeSemaphore = new SemaphoreSlim(0);
        
        var readTask = Task.Run(() =>
        {
            foreach (var fileInfo in files)
            {
                using var fileStream = File.OpenText(fileInfo.FullName);

                logger.LogDebug("Reading file {fileName}.", fileInfo.FullName);

                var count = 0;
                
                while (fileStream.ReadLine() is { } line)
                {
                    while (bag.Count >= options.Value.ReadBufferSize)
                    {
                        readSemaphore.Wait();
                    }
                    
                    count++;
                    bag.Add(ParseLine(line));

                    if (count % 25000 == 0)
                    {
                        logger.LogInformation("Read {count} elements from file.", count);
                        count = 0;
                    }

                    if (bag.Count >= options.Value.BulkThreshold && writeSemaphore.CurrentCount == 0)
                    {
                        writeSemaphore.Release(1);
                    }
                }

                if (count > 0)
                {
                    logger.LogInformation("IMPORT: Read {count} elements from file.", count);
                }
            }
            
            writeSemaphore.Release(1);
        });

        var writeTask = Task.Run(() =>
        {
            var data = new List<FileData>(options.Value.MaxBulkSize);
            
            while (true)
            {
                writeSemaphore.Wait();
                
                if (bag.IsEmpty) return;
                
                while (data.Count < options.Value.MaxBulkSize && bag.TryTake(out var fileData))
                {
                    data.Add(fileData);
                }

                if (bag.Count < options.Value.ReadBufferSize && readSemaphore.CurrentCount == 0)
                {
                    readSemaphore.Release(1);
                }

                if (data.Count == options.Value.MaxBulkSize)
                {
                    writeSemaphore.Release(1);
                }

                logger.LogInformation("IMPORT: Wrote {data} elements to db. Waiting: {bag}", data.Count, bag.Count);
                context.BulkInsert(data);
                
                data.Clear();
            }
        });

        await Task.WhenAll(readTask, writeTask);
    }

    private static FileData ParseLine(string line)
    {
        var data = line.Split("||");

        return new FileData
        {
            Key = Guid.NewGuid(),
            Date = DateOnly.FromDateTime(DateTime.Parse(data[0])),
            EnglishString = data[1],
            RussianString = data[2],
            EvenInteger = int.Parse(data[3]),
            RandomDouble = double.Parse(data[4])
        };
    }
}