namespace Task_01.Options;

public class DataImporterOptions
{
    public int BulkThreshold { get; set; }
    public int ReadBufferSize { get; set; }
    public int MaxBulkSize { get; set; }
}