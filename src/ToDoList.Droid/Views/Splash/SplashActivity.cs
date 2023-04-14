using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Views;

namespace ToDoList.Droid.Views.Splash
{
    [Activity(
        NoHistory = true,
        MainLauncher = true,
        Label = "@string/app_name",
        Theme = "@style/AppTheme.Splash",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round")]
    public class SplashActivity : MvxSplashScreenActivity
    {
    }
}
