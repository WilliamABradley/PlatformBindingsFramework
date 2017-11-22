using System;
using Windows.UI.ViewManagement;

namespace PlatformBindings.Services
{
    public class UWPTitleManager : ITitleManager
    {
        public string PageTitle { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public string WindowTitle
        {
            get
            {
                return ApplicationView.GetForCurrentView().Title;
            }
            set
            {
                if (value == null) value = "";
                ApplicationView.GetForCurrentView().Title = value;
            }
        }

        public bool SupportsWindowTitle => true;

        public bool SupportsPageTitle => false;
    }
}