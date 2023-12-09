namespace Task_01.Services.Interfaces;

public interface IContentGenerator
{
    public string Generate(int linesCount, string separator);
}