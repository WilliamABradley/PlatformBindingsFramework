// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Threading.Tasks;
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