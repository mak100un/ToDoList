using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ToDoList.Droid.Definitions.Converters;

public class IsNotNullOrEmptyConverter : MvxValueConverter<string, bool>
{
    protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
        => !string.IsNullOrEmpty(value);
}
