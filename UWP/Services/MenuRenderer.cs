using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Services
{
    public static class MenuRenderer
    {
        private static KeyboardCapabilities keyboardCapabilities = new KeyboardCapabilities();

        public static void ShowAt(this Menu Menu, FrameworkElement Element, Point? Point = null)
        {
            var flyout = CreateFlyout(Menu, Menu);
            if (Point.HasValue) flyout.ShowAt(Element, Point.Value);
            else flyout.ShowAt(Element);
        }

        public static void AttachTo(this Menu Menu, CommandBar Bar)
        {
            Bar.SecondaryCommands.Clear();
            foreach (var item in Menu)
            {
                if (item is MenuExpansionContainer exp)
                {
                    foreach (var containerItem in exp)
                    {
                        var element = CreateAppBarElement(Menu, containerItem);
                        if (element != null) Bar.SecondaryCommands.Add(element);
                    }
                }
                else
                {
                    var element = CreateAppBarElement(Menu, item);
                    if (element != null) Bar.SecondaryCommands.Add(element);
                }
            }
        }

        private static MenuFlyout CreateFlyout(Menu Menu, IList<IMenuItem> items)
        {
            var flyout = new MenuFlyout();
            AttachSubElements(Menu, items, flyout);
            return flyout;
        }

        private static ICommandBarElement CreateAppBarElement(Menu Menu, IMenuItem item)
        {
            switch (item)
            {
                case ToggleMenuItem tog:
                    var togitem = new AppBarToggleButton { Label = keyboardCapabilities.KeyboardPresent != 0 ? tog.Text : tog.Label, IsChecked = tog.IsToggled, IsEnabled = tog.IsEnabled };
                    togitem.Click += delegate { tog.Action?.Invoke(tog); };
                    tog.DataContext = Menu.DataContext;
                    return togitem;

                case MenuItem reg:
                    var menuitem = new AppBarButton { Label = keyboardCapabilities.KeyboardPresent != 0 ? reg.Text : reg.Label, IsEnabled = reg.IsEnabled };
                    menuitem.Click += delegate { reg.Action?.Invoke(reg); };
                    reg.DataContext = Menu.DataContext;
                    return menuitem;

                case SubMenu sub:
                    var submenu = new AppBarButton { Label = sub.Label };
                    submenu.Flyout = CreateFlyout(Menu, sub);
                    return submenu;

                case MenuSeparator sep:
                    return new AppBarSeparator();

                default:
                    return null;
            }
        }

        private static MenuFlyoutItemBase CreateElement(Menu Menu, IMenuItem item)
        {
            switch (item)
            {
                case ToggleMenuItem tog:
                    var togmenuitem = new ToggleMenuFlyoutItem { Text = keyboardCapabilities.KeyboardPresent != 0 ? tog.Text : tog.Label, IsChecked = tog.IsToggled, IsEnabled = tog.IsEnabled };
                    togmenuitem.Click += delegate { tog.Action?.Invoke(tog); };
                    tog.DataContext = Menu.DataContext;
                    return togmenuitem;

                case MenuItem reg:
                    var menuitem = new MenuFlyoutItem { Text = keyboardCapabilities.KeyboardPresent != 0 ? reg.Text : reg.Label, IsEnabled = reg.IsEnabled };
                    menuitem.Click += delegate { reg.Action?.Invoke(reg); };
                    reg.DataContext = Menu.DataContext;
                    return menuitem;

                case SubMenu sub:
                    var submenu = new MenuFlyoutSubItem { Text = sub.Label };
                    AttachSubElements(Menu, sub, submenu);
                    return submenu;

                case MenuSeparator sep:
                    return new MenuFlyoutSeparator();

                default:
                    return null;
            }
        }

        private static void AttachSubElements(Menu Menu, IList<IMenuItem> sub, dynamic submenu)
        {
            foreach (var subitem in sub)
            {
                if (subitem is MenuExpansionContainer exp)
                {
                    foreach (var containerItem in exp)
                    {
                        var element = CreateElement(Menu, containerItem);
                        if (element != null) ((IList<MenuFlyoutItemBase>)submenu.Items).Add(element);
                    }
                }
                else
                {
                    var element = CreateElement(Menu, subitem);
                    if (element != null) ((IList<MenuFlyoutItemBase>)submenu.Items).Add(element);
                }
            }
        }
    }
}