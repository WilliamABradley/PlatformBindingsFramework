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

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using PlatformBindings.Activities;

namespace Test_Android.Views
{
    [Activity(Label = "Return Activity")]
    public class ReturnActivity : PlatformBindingActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            var button = new Button(this)
            {
                Text = "Complete with Result",
            };
            button.Click += Button_Click;
            SetContentView(button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            SetResult(Result.Ok);
            Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}