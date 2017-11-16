# Contributing to the Platform Bindings Framework

The Platform Bindings Framework is intended to unify Lower Level App Functionality and simplify access to common tasks, such as Prompting the User with a Modal Dialog, Picking and Displaying Files/Folders, Saving/Loading Settings, Navigation and more.

If you can think of a great API Addition to the Framework, Create an Issue, specifiy whether you would want to help implement on a single Platform, All Platforms, or just keep it as a suggestion.

### Guidelines
 - [Questions / Feedback](#questions-feedback)
 - [Submitting a pull request](#submitting-a-pull-request)
 - [Want a Platform Supported?](#want-a-platform-supported)
 - [Implementating a Platform](#implementating-a-platform)
 - [Want to add a new API Feature?](#want-to-add-a-new-api-feature)
 - [Conventions](#conventions)
 - [Documentation](#documentation)

## Questions / Feedback

If you have any Questions or Feedback about how to use the Platform Bindings Framework, the best place for the time being, is by creating an GitHub Issue.

## Submitting a pull request

For every contribution you must:

* Follow the set out [Conventions](#conventions)
* Test your implementation with the Minimum and Maximum supported SDKs for each modified platform (if applicable).
* Create Documentation / Platform Remarks, (if applicable).

The PR will be vaildated before being merged.

## Want a Platform Supported?

If you see a Platform that hasn't been added yet, and you want to Promote it being added, create an Issue suggesting so, and I would consider it based on Thumbs Up/Time Availability.

It would likely be easier/quicker if you implemented the Platform yourself.

## Implementating a Platform

If you would like to implement a Platform Partially/Fully, or Implement part of a Platform that has missing implementations, submit an issue to Identify that you are working on these issues, then reference the Issue in a Pull Request.

## Want to add a new API Feature?

If you have an idea for some more Universal functionality in this Framework, or want to suggest a feature, create an Issue. I will then determine the viability of the feature. 

I don't want to bloat the Framework, but if things get too large, I will consider a new Packaging Strategy the breaks the framework into components.

## Conventions

### Namespaces

Namespaces for each file should generally follow the folder structure. Platform names should be absent from Namespaces.

### Classes

When creating an inherited Framework class, it needs to be Prefixed with the Platform Name or Nickname for Platform Specific Classes, or what additional functionality it provides.

E.g. The File Container returned from the PlatformBindings-Android implementation of the FilePicker, is an encapsulated DocumentFile from the Android Storage Access Framework. 

It's class is therefore called `AndroidSAFFileContainer`.
The enscapsulation of UWP's StorageFolder is called `UWPFolderContainer`.
The encapsulation of the SmbFile is called `SMBFileContainer`.

These Classes are more likely to be used as their abstractions, `FileContainer` and `FolderContainer`. However, it is good practice to Prefix them so they are can be understood from a glance.

Abstract Classes that are in the Public API should not be affixed with `Base`, as the User will be interacting with them. E.g. `UIBindings` instead of `UIBindingsBase`.

### Method Names

Most of the Implementation of the Platform Bindings Framework follows the provided interfaces, however, if you are writing a Platform Specific function inside an Inherited class, then ensure that the purpose of the function can be understood from the name of the method.

### API Function Support

Added Platform Specific methods are unlikely to be used from outside of the internal API, as it would require casting and Type checking. If you want a Platform specific function to be added to the Framework, create an Issue requesting it.

A Specific API can be added this way for things such as Saving Folder/File Access Permissions for future usage, such as with UWP and Android, but this might not be required for most platforms. To see if this is supported/required for the current platform, you need to check the value of `AppServices.IO.RequiresFutureAccessToken`. Then you can call `AppServices.IO.GetFutureAccessToken(FileSystemContainer)`.

### Other

* Use Exceptions instead of booleans to signify Success/Failure of a function.

* Try to avoid using optional Parameters, this can cause issues with Android due to the Java Interaction. Instead create multiple Constructors/Methods with the same name and different parameters instead.

* Don't seal any classes and try to use virtual methods/properties where appropriate, Platform Bindings Framework functions should be easily overridable by users for custom or additional behaviour.

* If you want, you could also develop some Unit Tests or "Test Apps" Tests to ensure the functionality you have added works properly.

## Documentation

If you are implementing a Platform Function, and it might require additonal User Initialisation or implementation, be sure to document this in the Platform Remarks in `_docs/Platform/{Platform}/`.

This is important for things, such as requiring usage of `PlatformBindingsActivity` when using PlatformBindings-Android.