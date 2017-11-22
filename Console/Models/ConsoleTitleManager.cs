using PlatformBindings.Services;
using System;

namespace PlatformBindings.Models
{
    public class ConsoleTitleManager : ITitleManager
    {
        public string PageTitle { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public string WindowTitle
        {
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = value;
            }
        }

        public bool SupportsWindowTitle => true;

        public bool SupportsPageTitle => false;
    }
}