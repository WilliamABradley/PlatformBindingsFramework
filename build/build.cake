#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=GitVersion.CommandLine"

using System;
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var Solution = "..\\PlatformBindingsFramework.sln";
var PackgeOutput = ".\\nupkg";

var CoreProj = "..\\PlatformBindings-Core\\PlatformBindings-Core.csproj";
var ConsoleProj = "..\\PlatformBindings-Console\\PlatformBindings-Console.csproj";
var UWPProj = ".\\PlatformBindings-UWP.nuspec";
var AndroidProj = ".\\PlatformBindings-Android.nuspec";

var RootFolder = "../";

var DotNetProjs = new string[] { CoreProj, ConsoleProj };
var NugetProjs = new string[] { UWPProj, AndroidProj };

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    Information("\nCleaning Solution");
    MSBuild(Solution, configurator =>
        configurator.SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Quiet)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
            .WithTarget("Clean"));

        Information("\n");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(Solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    Information("\nBuilding Solution");
    MSBuild(Solution, configurator =>
        configurator.SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Quiet)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
            .WithTarget("Build")
            .WithProperty("GenerateSolutionSpecificOutputFolder", "true")
            .WithProperty("GenerateLibraryLayout", "true"));
});

Task("Nuget-Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("\n Building DotNetCore Packages");
    var dotPackSettings = new DotNetCorePackSettings
    {
        OutputDirectory = PackgeOutput,
        ArgumentCustomization = args => args.Append("--include-symbols")
    };
    
    foreach(var proj in DotNetProjs)
    {
        DotNetCorePack(proj, dotPackSettings);
    }

    Information("\n Building Nuget Packages");
    var nuGetPackSettings = new NuGetPackSettings
    {
        OutputDirectory = PackgeOutput,
        Symbols = true,
        Properties = new Dictionary<string, string>
        {
            { "root", RootFolder }
        }
    };

    foreach(var proj in NugetProjs)
    {
        NuGetPack(proj, nuGetPackSettings);
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Nuget-Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
