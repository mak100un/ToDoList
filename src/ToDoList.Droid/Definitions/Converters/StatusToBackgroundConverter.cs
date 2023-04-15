using System;
using System.Globalization;
using MvvmCross.Converters;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Droid.Definitions.Converters;

public class StatusToBackgroundConverter : MvxValueConverter<ToDoTaskStatus, int>
{
    protected override int Convert(ToDoTaskStatus value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            ToDoTaskStatus.Done => Resource.Drawable.todo_item_disabled_background,
            _ => Resource.Drawable.todo_item_primary_background,
        };
}
