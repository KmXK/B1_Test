using System.Globalization;
using System.Windows.Data;
using Task_02.Persistence.Entities;

namespace Task_02.Converters;

[ValueConversion(typeof(TurnoverStatement), typeof(string))]
public class TurnoverStatementNameConverter : BaseConverter, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TurnoverStatement statement)
        {
            return $"{statement.Bank.Name} {statement.CreationDate}";
        }

        throw new InvalidOperationException();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}