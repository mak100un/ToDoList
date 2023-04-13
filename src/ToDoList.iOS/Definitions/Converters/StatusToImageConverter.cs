using System;
using System.Globalization;
using MvvmCross.Converters;
using ToDoList.Core.Definitions.Enums;
using UIKit;

namespace ToDoList.iOS.Definitions.Converters;

public class StatusToImageConverter : MvxValueConverter<ToDoTaskStatus, UIImage>
{
    protected override UIImage Convert(ToDoTaskStatus value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            ToDoTaskStatus.Done => UIImage.FromBundle(nameof(ToDoTaskStatus.Done)),
            ToDoTaskStatus.InProgress => UIImage.FromBundle(nameof(ToDoTaskStatus.InProgress)),
            ToDoTaskStatus.ToDo => UIImage.FromBundle(nameof(ToDoTaskStatus.ToDo)),
        };
}
