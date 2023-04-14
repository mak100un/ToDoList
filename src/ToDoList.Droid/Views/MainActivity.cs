using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.ViewModels.Extra;

namespace ToDoList.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(
        Theme = "@style/AppTheme",
        Label = "@string/app_name",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTask,
        WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : BaseActivity<MainViewModel>
    {
        protected override int ActivityLayoutId => Resource.Layout.activity_main_container;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
