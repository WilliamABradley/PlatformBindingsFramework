# PlatformBindings-Android Remarks

## Getting Started

To use the Platform Bindings Framework, you must first have the `PlatformBindings` Package installed. 
In your First Activity, Call the Bootstrapper in the Constructor. It **MUST** be Constructed before `base.OnCreate(Bundle)` is called, as that is when Activity Registration begins.

```C#
public class MainActivity : PlatformBindingActivity
{
    public MainActivity()
    {
        PlatformBindingsBootstrapper.Initialise(true);
    }


    protected override void OnCreate(Bundle bundle)
    {
        // Build App Services before calling base, to allow binding.
        base.OnCreate(bundle);
        StartActivity(typeof(Shell));
        Finish();
    }
}
```

### Activity Inheritance

In order to use all of PlatformBindings-Android's Functionality, you must have an Activity that is compatible with the ActivityHandler, this can be achieved multiple ways for different Reasons.

You can Inherit your Activity from `PlatformBindingActivity`, this is a Super Class of `Activity`, where Overriden methods handoff to the ActivityHandler.

You can Inherit your Activity from `PlatformBindingCompatActivity`, this is a Super Class of `Android.Support.V7.App.AppCompatActivity`, where Overriden methods handoff to the ActivityHandler.

If you already have your own Activity Super Class, or you inherit from another library, such as MVVMCross, then you can Override the Required Methods yourself to create your own PlatformBinding Supported Activity.

See [here](https://github.com/WilliamABradley/PlatformBindingsFramework/blob/master/Android/Activities/PlatformBindingActivity.cs) for an example Implementation of an ActivityHandler handoff, Activtiy Super Class.