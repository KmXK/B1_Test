using System.Globalization;
using System.Windows.Data;

namespace Task_02.Converters;

[ValueConversion(typeof(bool), typeof(int))]
public class BoolToIntegerConverter : BaseConverter, IValueConverter
{
    public int FalseValue { get; set; }
    
    public int TrueValue { get; set; }
    
    public object Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        if ((bool)(value ?? false))
        {
            return TrueValue;
        }

        return FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, 
        CultureInfo culture)
    {            
        return parameter;
    }
}