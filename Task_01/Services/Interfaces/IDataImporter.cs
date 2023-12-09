namespace Task_01.Services.Interfaces;

public interface IDataImporter
{
    Task Import(IEnumerable<FileInfo> files);
}