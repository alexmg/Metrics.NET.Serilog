#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Metrics.Serilog/bin") + Directory(configuration);
var testDir = Directory("./test/Metrics.Serilog.Tests/bin") + Directory(configuration);
var nugetPackagesDir = Directory("./artefacts");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(testDir);
    CleanDirectory(nugetPackagesDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./Metrics.Serilog.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./Metrics.Serilog.sln", settings => settings.SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./test/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings { NoResults = true });
});

var nuGetPackSettings = new NuGetPackSettings
{
    OutputDirectory = nugetPackagesDir
};

Task("Pack-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    CreateDirectory(nugetPackagesDir);
    NuGetPack("./src/Metrics.Serilog/Metrics.Serilog.csproj", nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack-NuGet-Packages");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);