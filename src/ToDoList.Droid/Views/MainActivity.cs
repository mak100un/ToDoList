using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions.Constants;
using ToDoList.Core.Services.Interfaces;
using ToDoList.Core.ViewModels.Extra;

namespace ToDoList.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(
        Theme = "@style/AppTheme",
        Label = "@string/app_name",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTask,
        WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : BaseActivity<MainViewModel>
    {
        private const string EXIT_APP_MESSAGE = "Are you want to exit application?";

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

        public override async void OnBackPressed()
        {
            if (SupportFragmentManager?.BackStackEntryCount > 1)
            {
                base.OnBackPressed();
                return;
            }

            if (!await Mvx.IoCProvider.Resolve<IDialogService>().DisplayAlertAsync(null, EXIT_APP_MESSAGE,
                    MessageConstants.YES_MESSAGE, MessageConstants.NO_MESSAGE))
            {
                return;
            }

            Process.KillProcess(Process.MyPid());
        }
    }
}
