namespace Task_01.ContentGenerator;

public interface IContentGenerator
{
    public string Generate(int linesCount, string separator);
}