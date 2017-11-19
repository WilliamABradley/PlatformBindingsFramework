# PlatformBindings-Android Remarks

## Getting Started

To use the Android Platform Library, you must first have the `PlatformBindings-Android` Package installed. 

### Activity Inheritance

In order to use all of PlatformBindings-Android's Functionality, you must have an Activity that is compatible with the ActivityHandler, this can be achieved multiple ways for different Reasons.

You can Inherit your Activity from `PlatformBindingActivity`, this is a Super Class of `Activity`, where Overriden methods handoff to the ActivityHandler.

You can Inherit your Activity from `PlatformBindingCompatActivity`, this is a Super Class of `Android.Support.V7.App.AppCompatActivity`, where Overriden methods handoff to the ActivityHandler.

If you already have your own Activity Super Class, or you inherit from another library, such as MVVMCross, then you can Override the Required Methods yourself to create your own PlatformBinding Supported Activity.

See [here](https://github.com/WilliamABradley/PlatformBindingsFramework/blob/master/Android/Activities/PlatformBindingActivity.cs) for an example Implementation of an ActivityHandler handoff, Activtiy Super Class.

### Initialisation

In your First Activity, Create the AppServices Class Object either in the Constructor. It **MUST** be Constructed before `base.OnCreate(Bundle)` is called, as that is when Activity Registration begins.

```C#
public class MainActivity : PlatformBindingActivity
{
    public static AndroidAppServices Services { get; private set; } = new AndroidAppServices(true);


    protected override void OnCreate(Bundle bundle)
    {
        // Build App Services before calling base, to allow binding.
        base.OnCreate(bundle);
        StartActivity(typeof(Shell));
        Finish();
    }
}
```