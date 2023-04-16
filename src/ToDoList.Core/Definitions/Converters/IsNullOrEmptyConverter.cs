using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ToDoList.Core.Definitions.Converters;

public class IsNullOrEmptyConverter : MvxValueConverter<string, bool>
{
    protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
        => string.IsNullOrEmpty(value);
}
