using System.ComponentModel;

namespace Tests
{
    public enum TestNavigationPage
    {
        Unknown,
        Home,

        [Description("Test Context Menu")]
        ContextMenu,

        [Description("Test Credential Manager")]
        Credential,

        [Description("Test Files/Folders")]
        FileFolder,

        [Description("Test Loop Timer")]
        LoopTimer,

        [Description("Test OAuth")]
        OAuth,

        [Description("Test Pickers")]
        Pickers,

        [Description("Test Settings")]
        Settings
    }
}