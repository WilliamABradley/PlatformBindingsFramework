using System.Collections.Generic;

namespace PlatformBindings.Controls.MenuLayout
{
    public class Menu : List<IMenuItem>
    {
        public Menu(object DataContext = null)
        {
            this.DataContext = DataContext;
        }

        public object DataContext { get; set; }
    }
}