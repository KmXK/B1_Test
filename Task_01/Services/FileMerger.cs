﻿using Microsoft.Extensions.Logging;
using Task_01.Services.Interfaces;

namespace Task_01.Services;

public class FileMerger(ILogger<FileMerger> logger) 
    : IFileMerger
{
    public async Task MergeAsync(
        IEnumerable<FileInfo> files,
        string outputFilePath,
        Func<string, bool>? mergeLinePredicate = null)
    {
        logger.LogDebug("Connection to output file {filePath} was opened.", outputFilePath);
        
        await using var outputFile = File.OpenWrite(outputFilePath);
        await using var streamWriter = new StreamWriter(outputFile);

        foreach (var fileInfo in files)
        {
            using var file = fileInfo.OpenText();
            
            logger.LogDebug("Connection to source file {filePath} was opened.", fileInfo.FullName);

            string? line;

            do
            {
                line = await file.ReadLineAsync();
                logger.LogTrace("Line has been read.");

                if (line != null && mergeLinePredicate?.Invoke(line) == true)
                {
                    await streamWriter.WriteLineAsync(line);
                    logger.LogTrace("Line has been written.");
                }
            } while (line != null);
            
            logger.LogDebug("Closing connection to source file {filePath}.", fileInfo.FullName);
        }
        
        logger.LogDebug("Closing connection to output file {filePath}.", outputFilePath);
    }
}
