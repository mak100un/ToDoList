using System.Threading.Tasks;
using System;
using Android.App;
using Android.Content;
using Android.Views;
using MvvmCross.Platforms.Android;
using ToDoList.Core.Services.Interfaces;

namespace ToDoList.Droid.Services
{
    public class DialogService : IDialogService
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;
        
        public DialogService(IMvxAndroidCurrentTopActivity topActivity) => _topActivity = topActivity;
        
        public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            var alertDialog = new AlertDialog.Builder(_topActivity.Activity)
                .SetTitle(title)
                ?.SetMessage(message)
                ?.Create();

            if (alertDialog == null)
            {
                return Task.FromResult(false);
            }

            var tcs = new TaskCompletionSource<bool>();

            alertDialog.SetButton((int)DialogButtonType.Positive, accept, (s, e) =>
            {
                tcs.TrySetResult(true);
                Dispose();
            });
            alertDialog.SetButton((int)DialogButtonType.Negative, cancel, (s, e) =>
            {
                tcs.TrySetResult(false);
                Dispose();
            });
            alertDialog.SetCanceledOnTouchOutside(true);
            alertDialog.SetCancelable(true);
            alertDialog.CancelEvent += OnCancelEvent;

            alertDialog.Window?.SetSoftInputMode(SoftInput.StateVisible);
            alertDialog.Show();

            return tcs.Task;

            void OnCancelEvent(object sender, EventArgs e)
            {
                tcs.TrySetResult(false);
                Dispose();
            }

            void Dispose()
            {
                if (alertDialog == null)
                {
                    return;
                }
                alertDialog.CancelEvent -= OnCancelEvent;
                alertDialog.Dispose();
                alertDialog = null;
            }
        }
    }
}