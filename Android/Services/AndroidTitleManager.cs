using PlatformBindings.Common;
using System;

namespace PlatformBindings.Services
{
    public class AndroidTitleManager : ITitleManager
    {
        public string WindowTitle { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public string PageTitle
        {
            get
            {
                return AndroidHelpers.GetCurrentActivity().Title;
            }
            set
            {
                try
                {
                    AndroidHelpers.GetCurrentActivity().Title = value;
                }
                catch { }
            }
        }

        public bool SupportsWindowTitle => false;

        public bool SupportsPageTitle => true;
    }
}