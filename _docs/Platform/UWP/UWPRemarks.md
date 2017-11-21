# PlatformBindings-UWP Remarks

## Getting Started

To use the UWP Platform Library, you must first have the `PlatformBindings-UWP` Package installed. Then in your Application Class (App.xaml.cs), Create the AppServices Class Object either in the Constructor or Statically.

```C#
public static UWPAppServices Services = new UWPAppServices(true);
```

Due to UWP's Design, the Dispatcher isn't available yet, so you will need to attach the Dispatcher when it becomes available, such as in the `OnLaunched` Method. 

Make sure you don't call any AppServices.UI methods before Attaching the Dispatcher.

After Attaching the Dispatcher, this is the best time to load any PlatformBindings extensions, Such as PlatformBindings-SMB.

```C#
/// <summary>
/// Invoked when the application is launched normally by the end user.  Other entry points
/// will be used such as when the application is launched to open a specific file.
/// </summary>
/// <param name="e">Details about the launch request and process.</param>
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
    Frame rootFrame = Window.Current.Content as Frame;

    // Do not repeat app initialization when the Window already has content,
    // just ensure that the window is active
    if (rootFrame == null)
    {
        // Create a Frame to act as the navigation context and navigate to the first page
        rootFrame = new Frame();
        Services.AttachDispatcher(rootFrame.Dispatcher);
        Tests.Preparation.Prepare();

        rootFrame.NavigationFailed += OnNavigationFailed;

        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        {
            //TODO: Load state from previously suspended application
        }

        // Place the frame in the current Window
        Window.Current.Content = rootFrame;
    }

    if (e.PrelaunchActivated == false)
    {
        if (rootFrame.Content == null)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(MainPage), e.Arguments);
        }
        // Ensure the current window is active
        Window.Current.Activate();
    }
}
```