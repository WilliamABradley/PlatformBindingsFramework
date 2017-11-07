using PlatformBindings.ConsoleTools;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PlatformBindings.Services
{
    public class CoreUIBindings : ConsoleUIBindings
    {
        public override void OpenLink(Uri Uri)
        {
            var url = Uri.ToString();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
    }
}
