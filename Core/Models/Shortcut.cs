using PlatformBindings.Enums;

namespace PlatformBindings.Models
{
    public class Shortcut
    {
        public Shortcut(KeyboardKey Key, bool CTRL = false, bool SHFT = false, bool ALT = false)
        {
            this.Key = Key;
            this.CTRL = CTRL;
            this.SHFT = SHFT;
            this.ALT = ALT;
        }

        public bool CTRL, SHFT, ALT;
        public KeyboardKey Key;

        public override string ToString()
        {
            return " (" + (CTRL ? "Ctrl + " : "") + (ALT ? "Alt + " : "") + (SHFT ? "Shft + " : "") + Key.ToString() + ")";
        }
    }
}