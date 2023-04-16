using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Core;
using ToDoList.Core;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            if (Window?.RootViewController is UINavigationController navigationController)
            {
                navigationController.NavigationBar.Layer.MasksToBounds = false;
                navigationController.NavigationBar.Layer.ShadowColor = UIColor.LightGray.CGColor;
                navigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 2);
                navigationController.NavigationBar.Layer.ShadowRadius = 1;
                navigationController.NavigationBar.Layer.ShadowOpacity = 0.7f;
                navigationController.NavigationBar.BarTintColor = ColorPalette.Primary;
                navigationController.NavigationBar.BackgroundColor = ColorPalette.Primary;
                navigationController.NavigationBar.TintColor = ColorPalette.Accent;
                navigationController.NavigationBar.BarStyle = UIBarStyle.Default;
            }
            return result;
        }
    }
}
