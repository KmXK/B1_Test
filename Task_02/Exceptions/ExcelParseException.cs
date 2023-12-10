namespace Task_02.Exceptions;

public class ExcelParseException(string message) : Exception("Excel parsing error: " + message)
{
    public string GetHumanReadableMessage()
    {
        return $"""
                Excel parsing error occured:
                {message}
                """;
    }
}