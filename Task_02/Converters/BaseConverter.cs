using System.Windows.Markup;

namespace Task_02.Converters;

public abstract class BaseConverter : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}