using PlatformBindings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Test_UWP.Services
{
    public class TestNavigationManager : WinNavigationManager
    {
        public TestNavigationManager(Frame Frame) : base(Frame)
        {
        }

        public override bool MenuOpen { get; set; }

        public override void Navigate(object PageRequest, object Parameter = null)
        {
        }
    }
}