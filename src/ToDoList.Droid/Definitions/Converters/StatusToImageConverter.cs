using System;
using System.Globalization;
using MvvmCross.Converters;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Droid.Definitions.Converters;

public class StatusToImageConverter : MvxValueConverter<ToDoTaskStatus, int>
{
    protected override int Convert(ToDoTaskStatus value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            ToDoTaskStatus.Done => Resource.Drawable.done,
            ToDoTaskStatus.InProgress => Resource.Drawable.inprogress,
            ToDoTaskStatus.ToDo => Resource.Drawable.todo,
        };
}
