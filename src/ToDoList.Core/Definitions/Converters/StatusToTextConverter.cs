using System;
using System.Globalization;
using MvvmCross.Converters;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Core.Definitions.Converters;

public class StatusToTextConverter : MvxValueConverter<ToDoTaskStatus, string>
{
    protected override string Convert(ToDoTaskStatus value, Type targetType, object parameter, CultureInfo culture)
        => value.ToString().ToUpper();
}
