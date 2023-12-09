using Task_01.Services.Interfaces;

namespace Task_01.Services;

public class FileMerger : IFileMerger
{
    public async Task MergeAsync(
        IEnumerable<FileInfo> files,
        string outputFilePath,
        Func<string, bool>? mergeLinePredicate = null)
    {
        await using var outputFile = File.OpenWrite(outputFilePath);
        await using var streamWriter = new StreamWriter(outputFile);

        foreach (var fileInfo in files)
        {
            using var file = fileInfo.OpenText();

            string? line;

            do
            {
                line = await file.ReadLineAsync();

                if (line != null && mergeLinePredicate?.Invoke(line) == true)
                {
                    await streamWriter.WriteLineAsync(line);
                }
            } while (line != null);
        }
    }
}
