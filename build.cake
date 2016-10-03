#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var projectName = "Metrics.Serilog";

// Define directories.
var sourceDir = Directory("./src");
var projectDir = sourceDir + Directory(projectName);
var buildDir = projectDir + Directory("bin") + Directory(configuration);
var testDir = Directory("./test/" + projectName + ".Tests/bin") + Directory(configuration);
var artifactsDir = Directory("./artifacts");

// Define files.
var solutionFile = File("./" + projectName + ".sln");
var projectFile = projectDir + File(projectName + ".csproj");

// Get environmental information.
var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

// Define version information.
var build = isRunningOnAppVeyor ? AppVeyor.Environment.Build.Number : 1;
var version = "0.1." + build;
var packageVersion = version + "-alpha";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    Information("Target: " + target);
    Information("Configuration: " + configuration);
    Information("Local Build: " + local);
    Information("AppVeyor Build: " + isRunningOnAppVeyor);
    Information("Pull Request: " + isPullRequest);
    Information("Build Number: " + build);
    Information("Version: " + version);
    Information("Package Version: " + packageVersion);
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    foreach (var dir in new[] {buildDir, testDir, artifactsDir})
    {
        Information("Cleaning: " + dir);
        CleanDirectory(dir);
    }
});

Task("Update-Version")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var versionFile = projectDir + Directory("Properties") + File("VersionInfo.cs");

    Information("Updating version file: " + versionFile);

    CreateAssemblyInfo(versionFile, new AssemblyInfoSettings {
        Version = version,
        FileVersion = version,
        InformationalVersion = packageVersion
    });
});

Task("Update-AppVeyor-Version")
    .IsDependentOn("Update-Version")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(packageVersion);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Update-AppVeyor-Version")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionFile, settings => settings.SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./test/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings { NoResults = true });
});

Task("Pack-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings
    {
        OutputDirectory = artifactsDir,
        Properties = new Dictionary<string, string> {{ "Configuration", configuration }}
    };

    CreateDirectory(artifactsDir);
    NuGetPack(projectFile, nuGetPackSettings);
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Pack-NuGet-Packages")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    var artifacts = GetFiles(artifactsDir.Path + "/**/*.nupkg");
    foreach(var artifact in artifacts)
    {
        AppVeyor.UploadArtifact(artifact);
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Upload-AppVeyor-Artifacts");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
