using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Common
{
    public static class ContentDialogHelper
    {
        public static ContentDialog ActiveDialog;
        private static TaskCompletionSource<bool> DialogAwaiter;
        public static bool Locked = false;

        public static async void CreateContentDialog(this ContentDialog Dialog, bool awaitPreviousDialog)
        {
            await CreateDialog(Dialog, awaitPreviousDialog);
        }

        public static async Task<ContentDialogResult> CreateContentDialogAsync(this ContentDialog Dialog, bool awaitPreviousDialog, bool Lock = false)
        {
            return await CreateDialog(Dialog, awaitPreviousDialog, Lock);
        }

        private static async Task<ContentDialogResult> CreateDialog(ContentDialog Dialog, bool awaitPreviousDialog, bool Lock = false)
        {
            if (ActiveDialog != null)
            {
                if (awaitPreviousDialog || Locked)
                {
                    await DialogAwaiter.Task;
                }
                else ActiveDialog.Hide();
            }

            Locked = Lock;

            ContentDialogResult result = ContentDialogResult.None;

            DialogAwaiter = new TaskCompletionSource<bool>();
            ActiveDialog = Dialog;

            try
            {
                ActiveDialog.Closed += ActiveDialog_Closed;
                result = await ActiveDialog.ShowAsync();
                ActiveDialog.Closed -= ActiveDialog_Closed;
                ActiveDialog = null;
            }
            catch { }

            if (Lock) Locked = false;
            return result;
        }

        private static void ActiveDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (!DialogAwaiter.Task.IsCompleted)
            {
                DialogAwaiter.TrySetResult(true);
            }
        }
    }
}