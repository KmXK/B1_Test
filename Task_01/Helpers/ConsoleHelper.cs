namespace Task_01.Helpers;

public static class ConsoleHelper
{
    public static void WriteLine(string text, ConsoleColor color)
    {
        var prevColor = Console.ForegroundColor;
        
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = prevColor;
    }
}