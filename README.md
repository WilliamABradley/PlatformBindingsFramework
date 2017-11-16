# Platform Bindings Framework for .NET
A Framework for creating code that performs Platform Dependent Tasks in a Generic, OS Independent manner.

Since this Framework is still in Early Alpha/Planning stages, the Documentation and API surface is subject to change, with breaking changes on new Releases until it is Production ready.

Be sure to perform null checks when interacting with the Framework, such as `AppServices.UI?.NavigationManager?.CanGoBack`, as well as check if a Platform Supports a certain feature, such as UI Access in Background Tasks, using the File/FolderPicker or File/Folder Open Functions.

Not all Platforms have the same level of implementation, UWP is currently fully implemented, and a lot of the APIs are based off of UWP APIs. 
Android is somewhat implemented, while Xamarin.Forms is mostly unimplemented. 

## Build status

| Target | Branch | Status | Recommended Nuget packages |
| ------ | ------ | ------ | ------ |
| Pre-Release | master | Alpha/Planning | [Packages](https://www.nuget.org/packages?q=platformbindings) |

## Documentation

All documentation is accessible on [GitHub](https://github.com/WilliamABradley/PlatformBindingsFramework/tree/master/_docs).

* [Getting Started](_docs/GettingStarted.md)
* [AppServices](_docs/AppServices.md)
* [Helpers & Extensions](_docs/Helpers.md)

### Platform Remarks

* [UWP](_docs/Platform/UWP/UWPRemarks.md)
* [Android](_docs/Platform/Android/AndroidRemarks.md)
* [XamarinForms](_docs/Platform/XamarinForms/XamarinFormsRemarks.md)

### Services

* [UIBindings](_docs/Services/UIBindings.md)
* [IOBindings](_docs/Services/IOBindings.md)
* [NetworkUtilities](_docs/Services/NetworkUtilities.md)
* [InteractionManager](_docs/Services/InteractionManager.md)
* [ICredentialManager](_docs/Services/ICredentialManager.md)
* [INavigationManager](_docs/Services/INavigationManager.md)
* [IOAuthBroker](_docs/Services/IOAuthBroker.md)

### File System

* [FileSystemContainer](_docs/Models/FileSystem/FileSystemContainer.md)
* [FileContainer](_docs/Models/FileSystem/FileContainer.md)
* [FolderContainer](_docs/Models/FileSystem/FolderContainer.md)

### Settings

* [ISettingsContainer](_docs/Models/Settings/ISettingsContainer.md)
* [Settings Properties](_docs/Models/Settings/Properties.md)
* [Property Lists](_docs/Models/Settings/PropertyLists.md)
* [Property Set Classes](_docs/Models/Settings/PropertySets.md)

### Models

* [IUIBindingInfo](_docs/Models/IUIBindingInfo.md)
* [LoopTimer](_docs/Models/LoopTimer.md)
* [IMenuBinding](_docs/Models/IMenuBinding.md)
* [IncrementalLoading](_docs/Models/IncrementalLoading.md)

### ViewModels

* [ViewModelBase](_docs/ViewModels/ViewModelBase.md)

## NuGet Packages

### Platform Libraries

| NuGet Package | Description |
| --- | --- |
| [PlatformBindings-Core](https://www.nuget.org/packages/PlatformBindings-Core) | Platform Bindings Framework Core Library and API Surface |
| [PlatformBindings-UWP](https://www.nuget.org/packages/PlatformBindings-UWP) | Universal Windows Platform Library |
| [PlatformBindings-Android](https://www.nuget.org/packages/PlatformBindings-Android) | Android Platform Library |
| [PlatformBindings-Console](https://www.nuget.org/packages/PlatformBindings-Console) | .NET Console Helpers for [System.Console](https://msdn.microsoft.com/en-us/library/system.console(v=vs.110).aspx) |
| [PlatformBindings-NETCore](https://www.nuget.org/packages/PlatformBindings-NETCore) | .NET Core Application Library |
| [PlatformBindings-XamarinForms](https://www.nuget.org/packages/PlatformBindings-XamarinForms) | Xamarin Forms Platform Library |

### Extensions

| NuGet Package | Description |
| --- | --- |
| [PlatformBindings-SMB](https://www.nuget.org/packages/PlatformBindings-SMB) | SMB/CIFS FileContainer & FolderContainer Extension |

## Supported SDKs

### UWP

**Anniversary Update (14393) Upwards**

### Android

**Android API 21\* Upwards**

\*PlatformBindings-Android utilises `Android.Support.V7.AppCompat`, however, to enable usage of AppCompat Activity and UI Elements, use `PlatformBindingsCompatActivity` and `AndroidAppServices.UseAppCompatUI`.

### .NET Core

**.NET Core 2.0 Upwards**

Although the Core and Console Libraries are .NET Standard 1.4 Compliant, there is an issue in .NET Core that returns the wrong Directory for the Executing Assembly Directory, so .NET Core 2.0 is required.

### Xamarin.Forms

**Tested against Xamarin.Forms 2.5.0.77107**

Requires .NET Standard 1.4 support.

## Feedback and Requests

Please use [GitHub issues](https://github.com/WilliamABradley/PlatformBindingsFramework/issues) for bug reports and Platform/API Addition requests.

## Contributing
Want to help and flesh out this Library? Submit a PR!

Here are the [contribution guidelines](Contributing.md).
