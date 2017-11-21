using PlatformBindings;
using PlatformBindings.Controls.MenuLayout;
using PlatformBindings.Models;

namespace Tests.Tests
{
    public class ContextMenuTestPage
    {
        public ContextMenuTestPage()
        {
            void Selected(MenuItem Item)
            {
                AppServices.Current.UI.PromptUser("Selected", $"^B^{Item.Label}^B^ Pressed", "OK", null);
                if (Item is ToggleMenuItem tog)
                {
                    tog.IsToggled = tog.IsToggled != true;
                }
            }

            Menu = new Menu
            {
                new MenuItem{ Label = "Element", Action = Selected },
                new MenuItem { Label = "Disabled", IsEnabled = false, Action = Selected },
                new ToggleMenuItem { Label = "Toggleable", Action = Selected },
                new ToggleMenuItem { Label = "Toggleable Disabled", IsToggled = true, IsEnabled = false, Action = Selected },
                new MenuSeparator(),
                new SubMenu("SubMenu")
                {
                    new MenuItem { Label = "SubElement", Action = Selected }
                }
            };
        }

        public void ShowMenu(IMenuBinding Binding)
        {
            AppServices.Current.UI.ShowMenu(Menu, Binding);
        }

        public Menu Menu { get; }
    }
}