namespace Task_01.Services.Interfaces;

public interface IFileMerger
{
    Task MergeAsync(
        IEnumerable<FileInfo> files,
        string outputFilePath,
        Func<string, bool>? mergeLinePredicate = null);
}