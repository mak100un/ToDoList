using Foundation;
using MvvmCross.Platforms.Ios.Core;
using ToDoList.Core;

namespace ToDoList.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}
