using System;
using System.Globalization;
using MvvmCross.Converters;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Droid.Definitions.Converters;

public class StatusToTextColorConverter : MvxValueConverter<ToDoTaskStatus, int>
{
    protected override int Convert(ToDoTaskStatus value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            ToDoTaskStatus.Done => Resource.Color.disabled_text_color,
            _ => Resource.Color.primaryColor,
        };
}
