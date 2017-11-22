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