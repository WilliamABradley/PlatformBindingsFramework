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

using System.Collections.Generic;
using PlatformBindings.Controls.MenuLayout;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;

namespace PlatformBindings.Models
{
    public class AndroidPopupMenuRenderer
    {
        private AndroidPopupMenuRenderer(Menu Menu, AndroidContextMenuBinding Binding)
        {
            this.Menu = Menu;
            this.Binding = Binding;
            DisplayMenu = new PopupMenu(Binding.Activity, Binding.TargetElement);
            AttachSubElements(Menu, DisplayMenu.Menu);
            DisplayMenu.Show();
        }

        public static void Attach(Menu Menu, AndroidContextMenuBinding Binding)
        {
            new AndroidPopupMenuRenderer(Menu, Binding);
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
        private PopupMenu DisplayMenu { get; }

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