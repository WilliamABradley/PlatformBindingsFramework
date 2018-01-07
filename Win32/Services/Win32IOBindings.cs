namespace PlatformBindings.Services
{
    public class Win32IOBindings : CoreIOBindings
    {
        public override FileSystemPickers Pickers => new Win32FileSystemPickers();
    }
}