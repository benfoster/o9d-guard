
// Install modules
#module nuget:?package=Cake.DotNetTool.Module&version=0.4.0

// Install .NET Core Global tools.
#tool "dotnet:?package=dotnet-reportgenerator-globaltool&version=4.8.5"
#tool "dotnet:?package=coveralls.net&version=3.0.0"

// Install addins 
#addin nuget:?package=Cake.Coverlet&version=2.5.1

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsPath = "./artifacts";
var tempPath = "./artifacts/temp"; 
var packFiles = "./src/**/*.csproj";
var testFiles = "./test/**/*.csproj";

uint coverageThreshold = 80;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
   Information($"Building Guard with configuration {configuration}");
});

Teardown(ctx =>
{
   if (DirectoryExists(tempPath))
   {
        DeleteDirectory(tempPath, new DeleteDirectorySettings 
        {
            Recursive = true,
            Force = true
        });
   }
   
   Information("Finished running build");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => 
    {
        CleanDirectories(artifactsPath);
    });

Task("Build")
   .Does(() => 
   {
      DotNetCoreBuild("Guard.sln", new DotNetCoreBuildSettings 
      {
         Configuration = configuration
      });
   });

Task("Test")
   .Does(() => 
   {
        var testSettings = new DotNetCoreTestSettings 
        {
            NoBuild = true,
            Configuration = configuration
        };

        string GenerateCoverageFileName(FilePath file)
            => file.GetFilenameWithoutExtension() + ".opencover.xml";
      
        foreach (var project in GetFiles(testFiles))
        {
            // https://github.com/Romanx/Cake.Coverlet
            var coverletSettings = new CoverletSettings 
            {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.opencover,
                CoverletOutputDirectory = tempPath,
                CoverletOutputName = GenerateCoverageFileName(project),
                Threshold = coverageThreshold,
            };
            
            DotNetCoreTest(project.ToString(), testSettings, coverletSettings);
        }
   });


Task("Pack")
    .Does(() => 
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = artifactsPath,
            NoBuild = true
        };

        foreach (var file in GetFiles(packFiles))
        {
            DotNetCorePack(file.ToString(), settings);
        }
    });

Task("GenerateReports")
    .Does(() => 
    {
        ReportGenerator(GetFiles($"{tempPath}/*.xml"), artifactsPath, new ReportGeneratorSettings
        {
            ArgumentCustomization = args => args.Append("-reporttypes:lcov;HTMLSummary;TextSummary;")
        });
    });

Task("UploadCoverage")
    .Does(() => 
    {

    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Pack")
    .IsDependentOn("GenerateReports");

RunTarget(target);

