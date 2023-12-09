using System.Globalization;
using System.Reflection;
using System.Text;

namespace Task_01.ContentGenerator;

public class RandomContentGenerator : IContentGenerator
{
    private const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    
    private readonly Random _random = new();

    [ThreadStatic] private static StringBuilder? _sb;
    [ThreadStatic] private static StringBuilder? _littleSb;

    public string Generate(int linesCount, string separator)
    {
        _sb ??= new StringBuilder(1000);
        _sb.Clear();

        for (var i = 0; i < linesCount; i++)
        {
            var date = GetRandomDate();
            var englishString = GetRandomEnglishString(10);
            var russianString = GetRandomRussianString(10);
            var evenNumber = GetRandomEvenInteger(100_000_000);
            var doubleNumber = _random.NextDouble() * 19 + 1;

            Append(date.ToString("dd.MM.yyyy"));
            Append(englishString);
            Append(russianString);
            Append(evenNumber.ToString());
            Append(doubleNumber.ToString("0.00000000"));

            _sb.Append(separator);
        }

        return _sb.ToString();

        void Append(string value)
        {
            _sb!.Append(value);
            _sb.Append("||");
        }
    }

    private int GetRandomEvenInteger(int max)
    {
        return _random.Next(max / 2 + 1)  * 2;
    }

    private string GetRandomRussianString(int length)
    {
        return GetRandomString(length, RussianAlphabet);
    }
    
    private string GetRandomEnglishString(int length)
    {
        return GetRandomString(length, EnglishAlphabet);
    }

    private DateOnly GetRandomDate()
    {
        var fromDate = DateTime.Now.AddYears(-5);
        
        var maxDays = (int)(DateTime.Now - fromDate).TotalDays;

        var days = _random.Next(maxDays);

        return DateOnly.FromDateTime(fromDate.AddDays(days));
    }

    private string GetRandomString(int length, string alphabet)
    {
        _littleSb ??= new StringBuilder(50);
        _littleSb.Clear();
        
        for (var i = 0; i < length; i++)
        {
            _littleSb.Append(alphabet[_random.Next(alphabet.Length)]);
        }

        return _littleSb.ToString();
    }
}