# Getting Started with the Platform Bindings Framework

## Initialisation

In order to be able to use this Framework, you must first Initalise the AppServices Class at your Application's Startup. This must be in your App's Project, as it requires the Project's Platform.

To see how to Initialise your Platform, see:
* [UWP](Platform/UWP/UWPRemarks.md#Getting-Started)
* [Android](Platform/Android/AndroidRemarks.md#Getting-Started)
* [Xamarin Forms](Platform/XamarinForms/XamarinFormsRemarks.md)
* [NETCore](Platform/NETCore/NETCoreRemarks.md#Getting-Started)

### Things to be aware of

Due to AppServices being Initialised after Static Construction, be wary of `AppSettings`, and other Settings Classes that try to Access AppServices.IO.LocalSettings or AppServices.IO.RoamingSettings. These will cause and exception if that is the case.

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