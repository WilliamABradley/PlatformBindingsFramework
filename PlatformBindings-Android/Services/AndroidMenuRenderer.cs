using System.Collections.Generic;
using Android.Text;
using Android.Text.Style;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Services
{
    public class AndroidMenuRenderer
    {
        private AndroidMenuRenderer()
        {
        }

        public static void Attach(Menu Menu, AndroidContextMenuBinding Binding, Android.Views.IContextMenu ActivityMenu)
        {
            var renderer = new AndroidMenuRenderer()
            {
                Menu = Menu,
                ActivityMenu = ActivityMenu,
                Binding = Binding
            };
            renderer.AttachSubElements(Menu, ActivityMenu);
        }

        private void CreateElement(IMenuItem item, Android.Views.IMenu AndroidMenu)
        {
            switch (item)
            {
                case MenuItem reg:
                    var button = AndroidMenu.Add(new Java.Lang.String(reg.Label));
                    button.SetEnabled(reg.IsEnabled);
                    button.SetOnMenuItemClickListener(new MenuClickHandler(reg));
                    if (reg is ToggleMenuItem tog)
                    {
                        button.SetCheckable(true);
                        button.SetChecked(tog.IsToggled);
                    }
                    break;

                case SubMenu sub:
                    var submenu = AndroidMenu.AddSubMenu(new Java.Lang.String(sub.Label));
                    AttachSubElements(sub, submenu);
                    break;

                case MenuSeparator sep:
                    var separator = AndroidMenu.Add(new Java.Lang.String("______________________________"));
                    separator.SetEnabled(false);

                    SpannableString s = new SpannableString(separator.TitleFormatted);
                    s.SetSpan(new AlignmentSpanStandard(Layout.Alignment.AlignCenter), 0, s.Length(), 0);
                    separator.SetTitle(s);
                    break;
            }
        }

        private void AttachSubElements(List<IMenuItem> sub, Android.Views.IMenu AndroidMenu)
        {
            foreach (var subitem in sub)
            {
                if (subitem is MenuExpansionContainer exp)
                {
                    foreach (var containerItem in exp)
                    {
                        CreateElement(containerItem, AndroidMenu);
                    }
                }
                else
                {
                    CreateElement(subitem, AndroidMenu);
                }
            }
        }

        private AndroidContextMenuBinding Binding { get; set; }
        private Menu Menu { get; set; }
        private Android.Views.IContextMenu ActivityMenu { get; set; }

        private class MenuClickHandler : Java.Lang.Object, Android.Views.IMenuItemOnMenuItemClickListener
        {
            public MenuClickHandler(MenuItem Element)
            {
                this.Element = Element;
            }

            public bool OnMenuItemClick(Android.Views.IMenuItem item)
            {
                Element.Action?.Invoke(Element);
                return true;
            }

            public MenuItem Element { get; }
        }
    }
}