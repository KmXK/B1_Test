namespace Task_01.Services.Interfaces;

public interface IFileGenerator
{
    Task CreateFilesAsync(int count, DirectoryInfo directoryInfo, string fileNamePrefix = "file_");
    Task CreateFileAsync(string fullName);
}