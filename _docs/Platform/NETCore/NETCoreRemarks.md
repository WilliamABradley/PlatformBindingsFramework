# PlatformBindings-NETCore Remarks

## Getting Started

To use the NETCore Platform Library, you must first have the `PlatformBindings-NETCore` Package installed. Then Create the AppServices Class Object in your Application's `static void Main(string[] args)` Method.
This can't be created Statically, as any Calls in the Main Method will come before Static Initialisation.

```C#
internal class Program
{
    public static NETCoreServices Services { get; private set; }

    private static void Main(string[] args)
    {
        Services = new NETCoreServices();
    }
}
```