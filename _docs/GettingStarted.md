# Getting Started with the Platform Bindings Framework

## Initialisation

In order to be able to use this Framework, you must first call `PlatformBindingsBootstrapper.Initialise()` at your Application's Startup. This must be inside your App's Project, as it requires the Project's Platform.

To see how to Initialise your Platform, see:
* [UWP](Platform/UWP/UWPRemarks.md#Getting-Started)
* [Android](Platform/Android/AndroidRemarks.md#Getting-Started)
* [Win32/.NET Framework](Platform/Win32/Win32Remarks.md#Getting-Started)
* [NETCore](Platform/NETCore/NETCoreRemarks.md#Getting-Started)
* [Xamarin Forms](Platform/XamarinForms/XamarinFormsRemarks.md)

### Things to be aware of

Due to AppServices being Initialised after Static Construction, be wary of Static `AppSettings` Properties, and other Settings Classes that try to access `AppServices.Current.IO.LocalSettings` or `AppServices.Current.IO.RoamingSettings`. These will cause an exception if called before AppServices is Initialised for the Platform.

Instead use a static Property with a Getter to construct the Property, only when needed. This should also be better for memory usage, as it allows the Garbage Collector to clean up the Object when finished manipulation. As well as update values when they change from other sources.

**Use:**
```c#
public static AppSetting<bool> ExampleToggleSetting => new AppSetting<bool>();
```

**Instead of:**
```C#
public static AppSetting<bool> ExampleToggleSetting = new AppSetting<bool>();
```

While the latter might work, if the Static Property is constructed before AppServices, it will cause an Exception.