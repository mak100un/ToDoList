using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Views;

namespace ToDoList.Droid.Views.Splash
{
    [Activity(
        NoHistory = true,
        MainLauncher = true,
        Label = "@string/app_name",
        Theme = "@style/AppTheme.Splash")]
    public class SplashActivity : MvxSplashScreenActivity
    {
    }
}
