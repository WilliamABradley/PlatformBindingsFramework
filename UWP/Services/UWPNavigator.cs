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

using PlatformBindings.Common;
using System;
using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Services
{
    public abstract class UWPNavigator<T> : Navigator<T>
    {
        public UWPNavigator(Frame Frame)
        {
            this.Frame = Frame;
            Frame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            _Parameter = (string)e.Parameter;
        }

        protected virtual bool InternalNavigate(Type Type, string Parameter, bool ShowBackButton = false, bool ClearBackStack = false)
        {
            PlatformBindingHelpers.OnUIThread(() =>
            {
                Frame.Navigate(Type, Parameter);
                AppServices.Current.UI.NavigationManager.ShowBackButton = ShowBackButton;

                if (ClearBackStack) Frame.BackStack.Clear();
            });
            return true;
        }

        public override string Parameter => _Parameter;
        private string _Parameter;

        private Frame Frame { get; }
    }
}