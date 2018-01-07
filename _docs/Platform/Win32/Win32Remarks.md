# PlatformBindings-UWP Remarks

## Getting Started

To use the Platform Bindings Framework, you must first have the `PlatformBindings` Package installed. Then in your Program Class (Program.cs or equivalent), call the Bootstrapper.

```C#
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        PlatformBindingsBootstrapper.Initialise(true);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form());
    }
}
```