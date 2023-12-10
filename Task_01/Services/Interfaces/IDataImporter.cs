namespace Task_01.Services.Interfaces;

public interface IDataImporter
{
    Task ImportAsync(IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);
}