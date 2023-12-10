namespace Task_01.Services.Interfaces;

public interface IFileGenerator
{
    Task CreateFilesAsync(
        int count,
        DirectoryInfo directoryInfo,
        string fileNamePrefix = "file_",
        CancellationToken cancellationToken = default);
    
    Task CreateFileAsync(string fullName, CancellationToken cancellationToken = default);
}