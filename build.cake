// Install modules
#module nuget:?package=Cake.DotNetTool.Module&version=0.4.0

// Install .NET Core Global tools.
#tool "dotnet:?package=dotnet-reportgenerator-globaltool&version=4.8.5"
#tool "dotnet:?package=coveralls.net&version=3.0.0"
#tool "dotnet:?package=dotnet-sonarscanner&version=5.0.4"
#tool nuget:?package=docfx.console&version=2.56.6

// This is using an unofficial build of kudusync so that we can have a .Net Global tool version.  This was generated from this PR: https://github.com/projectkudu/KuduSync.NET/pull/27
#tool dotnet:https://www.myget.org/F/cake-contrib/api/v3/index.json?package=KuduSync.Tool&version=1.5.4-g3916ad7218

// Install addins 
#addin nuget:?package=Cake.Coverlet&version=2.5.1
#addin nuget:?package=Cake.Sonar&version=1.1.25
#addin nuget:?package=Cake.DocFx&version=0.13.1
#addin nuget:?package=Cake.Git&version=1.0.0
#addin nuget:?package=Cake.Kudu&version=1.0.0

 #r "System.Text.Json"
 #r "System.IO"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsPath = "./artifacts";
var coveragePath = "./artifacts/coverage"; 
var packFiles = "./src/**/*.csproj";
var testFiles = "./test/**/*.csproj";
var packages = "./artifacts/*.nupkg";
DirectoryPath sitePath = "./artifacts/_site";

var coverallsToken = EnvironmentVariable("COVERALLS_TOKEN");
var sonarToken = EnvironmentVariable("SONAR_TOKEN");
var gitHubToken = EnvironmentVariable("GITHUB_TOKEN");

uint coverageThreshold = 50;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
   Information($"Building Guard with configuration {configuration}");
});

Teardown(ctx =>
{
   if (DirectoryExists(coveragePath))
   {
        DeleteDirectory(coveragePath, new DeleteDirectorySettings 
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

Task("SonarBegin")
    .WithCriteria(!string.IsNullOrEmpty(sonarToken))
    .Does(() => 
    {
        SonarBegin(new SonarBeginSettings 
        {
            Key = "benfoster_o9d-guard",
            Organization = "benfoster",
            Url = "https://sonarcloud.io",
            Exclusions = "test/**",
            OpenCoverReportsPath = $"{coveragePath}/*.xml",
            Login = sonarToken,
            VsTestReportsPath = $"{artifactsPath}/*.TestResults.xml",
        });
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
        foreach (var project in GetFiles(testFiles))
        {
            var projectName = project.GetFilenameWithoutExtension();
            
            var testSettings = new DotNetCoreTestSettings 
            {
                NoBuild = true,
                Configuration = configuration,
                Loggers = { $"trx;LogFileName={projectName}.TestResults.xml" },
                ResultsDirectory = artifactsPath
            };
            
            // https://github.com/Romanx/Cake.Coverlet
            var coverletSettings = new CoverletSettings 
            {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.opencover,
                CoverletOutputDirectory = coveragePath,
                CoverletOutputName = $"{projectName}.opencover.xml",
                Threshold = coverageThreshold
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
        ReportGenerator(GetFiles($"{coveragePath}/*.xml"), artifactsPath, new ReportGeneratorSettings
        {
            ArgumentCustomization = args => args.Append("-reporttypes:lcov;HTMLSummary;TextSummary;")
        });
    });

Task("UploadCoverage")
    .WithCriteria(!string.IsNullOrEmpty(coverallsToken) && BuildSystem.IsRunningOnGitHubActions)
    .Does(() => 
    {
        var workflow = BuildSystem.GitHubActions.Environment.Workflow;

        Dictionary<string, object> @event = default;
        if (workflow.EventName == "pull_request")
        {
            string eventJson = System.IO.File.ReadAllText(workflow.EventPath); 
            @event = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(eventJson);
        }

        var args = new ProcessArgumentBuilder()
                    .Append($"--repoToken {coverallsToken}")
                    .Append("--lcov")
                    .Append("--useRelativePaths")
                    .Append("-i ./artifacts/lcov.info")
                    .Append($"--commitId {workflow.Sha}") 
                    .Append($"--commitBranch {workflow.Ref}")
                    .Append($"--serviceNumber {workflow.RunNumber}")
                    .Append($"--jobId {workflow.RunId}");
                    //.Append("--serviceName github")
                    //.Append("--dryrun");

        if (BuildSystem.IsPullRequest)
        {
            args.Append($"--pullRequest {@event["number"].ToString()}");
        }

        var settings = new ProcessSettings { Arguments = args };

        // We have to start the process manually since the cake addin forces us to provide 
        // a format enum which currently doesn't include lcov
        if (StartProcess(
            Context.Tools.Resolve("csmacnz.Coveralls")
                ?? Context.Tools.Resolve("csmacnz.coveralls.exe")
                ?? throw new Exception("Failed to resolve Coveralls shim."),
            settings
            ) != 0)
        {
            throw new Exception("Failed to execute Coveralls.");
        }
    });

Task("PublishPackages")
    .Does(() => 
    {
        // // Resolve the API key.
        var apiKey = EnvironmentVariable("NUGET_API_KEY");
        if(string.IsNullOrEmpty(apiKey)) {
            throw new InvalidOperationException("Could not resolve NuGet API key.");
        }

        // Resolve the API url.
        var apiUrl = EnvironmentVariable("NUGET_API_URL");
        if(string.IsNullOrEmpty(apiUrl)) {
            throw new InvalidOperationException("Could not resolve NuGet API url.");
        }

        foreach(var package in GetFiles(packages))
        {
            DotNetCoreNuGetPush(package.ToString(), new DotNetCoreNuGetPushSettings {
                ApiKey = apiKey,
                Source = apiUrl,
                SkipDuplicate = true
            });
        }
    });

Task("SonarEnd")
    .WithCriteria(!string.IsNullOrEmpty(sonarToken))
    .Does(() => 
    {
        SonarEnd(new SonarEndSettings
        {
            Login = sonarToken
        });
    });

Task("BuildDocs")
    .Does(() => 
    {
        DocFxBuild("./docs/docfx.json");
    });

Task("ServeDocs")
    .IsDependentOn("BuildDocs")
    .Does(() => 
    {
        using (var process = DocFxServeStart("./artifacts/_site"))
        {
            // Launch browser or other action based on the site
            process.WaitForExit();
        }
    });

Task("PublishDocs")
    .Does(() => 
    {
        // Get the current commit
        var sourceCommit = GitLogTip("./");
        var publishFolder = $"./artifacts/temp-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
        Information("Publishing Folder: {0}", publishFolder);
        Information("Getting publish branch...");
        GitClone("https://github.com/benfoster/o9d-guard.git", publishFolder, new GitCloneSettings { BranchName = "gh-pages" });

        Information("Sync output files...");
        
        Kudu.Sync(sitePath, publishFolder, new KuduSyncSettings {
            ArgumentCustomization = args => args.Append("--ignore").AppendQuoted(".git;CNAME")
        });

        if (GitHasUncommitedChanges(publishFolder))
        {
            GitAddAll(publishFolder);
            Information("Stage all changes...");

            if (GitHasStagedChanges(publishFolder))
            {
                Information("Commit all changes...");
                GitCommit(
                    publishFolder,
                    sourceCommit.Committer.Name,
                    sourceCommit.Committer.Email,
                    string.Format("Continuous Integration Publish: {0}\r\n{1}", sourceCommit.Sha, sourceCommit.Message)
                );

                Information("Pushing all changes...");
                // TODO validate token exists
                GitPush(publishFolder, EnvironmentVariable("GITHUB_TOKEN"), "x-oauth-basic", "gh-pages");
            }
        }
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Pack")
    .IsDependentOn("GenerateReports");

Task("CI")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Default")
    .IsDependentOn("UploadCoverage")
    .IsDependentOn("SonarEnd")
    .IsDependentOn("BuildDocs");

Task("Publish")
    .IsDependentOn("CI")
    .IsDependentOn("PublishPackages");

RunTarget(target);

