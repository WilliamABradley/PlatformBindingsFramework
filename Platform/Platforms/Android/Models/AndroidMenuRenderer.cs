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
using Android.Text;
using Android.Text.Style;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Models
{
    public class AndroidMenuRenderer
    {
        private AndroidMenuRenderer()
        {
        }

        public static void Attach(Menu Menu, Android.Views.IContextMenu ActivityMenu)
        {
            var renderer = new AndroidMenuRenderer()
            {
                Menu = Menu,
                ActivityMenu = ActivityMenu
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
                    var str = new SpannableString("______________________________");
                    var length = str.Length();
                    str.SetSpan(new AlignmentSpanStandard(Layout.Alignment.AlignCenter), 0, length, 0);
                    //str.SetSpan(new LineHeightSpan(10), 0, length, SpanTypes.ExclusiveExclusive);

                    var separator = AndroidMenu.Add(str);
                    separator.SetEnabled(false);
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