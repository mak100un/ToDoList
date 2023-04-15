using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ToDoList.iOS.Definitions.Converters;

public class InvertedValueConverter : MvxValueConverter<bool, bool>
{
    protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        => !value;
}
