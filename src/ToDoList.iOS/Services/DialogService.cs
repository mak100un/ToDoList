using System;
using System.Drawing;
using System.Threading.Tasks;
using Foundation;
using ToDoList.Core.Services.Interfaces;
using ToDoList.iOS.Extensions;
using UIKit;

namespace ToDoList.iOS.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            var tcs = new TaskCompletionSource<bool>();

            var currentViewController = UIApplication.SharedApplication?.KeyWindow?.RootViewController?.GetPresentedViewController();

            if (currentViewController == null)
            {
                return false;
            }

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            var oldFrame = alert.View.Frame;
            alert.View.Frame = new RectangleF((float)oldFrame.X, (float)oldFrame.Y, (float)oldFrame.Width, (float)oldFrame.Height - 10 * 2);

            alert.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel, a => CancelAction()));

            alert.AddAction(UIAlertAction.Create(accept, UIAlertActionStyle.Default, a =>
            {
                tcs.TrySetResult(true);
                Dispose();
            }));

            await currentViewController.PresentViewControllerAsync(alert, true);
            alert.View.Superview.UserInteractionEnabled = true;
            alert.View.Superview.AddGestureRecognizer(new UITapGestureRecognizer((Action)CancelAction));

            return await tcs.Task;

            void Dispose()
            {
                alert?.DismissViewController(true, null);
                alert?.Dispose();
                alert = null;
            }
            
            void CancelAction()
            {
                tcs.TrySetResult(false);
                Dispose();
            }
        }
    }
}