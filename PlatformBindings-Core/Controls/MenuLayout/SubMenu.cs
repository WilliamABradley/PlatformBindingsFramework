namespace PlatformBindings.Controls.MenuLayout
{
    public class SubMenu : Menu, IMenuItem
    {
        public SubMenu(string Label = null)
        {
            this.Label = Label;
        }

        public string Label { get; set; }
    }
}