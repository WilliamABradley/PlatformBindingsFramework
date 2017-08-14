using System;
using PlatformBindings.Models;

namespace PlatformBindings.Controls.MenuLayout
{
    public class MenuItem : IMenuItem
    {
        public string Label { get; set; }
        public Action<MenuItem> Action { get; set; }
        public object DataContext { get; set; }
        public object Data { get; set; }
        public bool IsEnabled { get; set; } = true;
        public Shortcut Shortcut { get; set; }
        public string Text { get { return Label + Shortcut ?? ""; } }
    }
}