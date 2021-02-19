# Guard

[![NuGet](https://img.shields.io/nuget/v/O9d.Guard.svg)](https://www.nuget.org/packages/O9d.Guard)
[![NuGet](https://img.shields.io/nuget/vpre/O9d.Guard?label=Pre-release)](https://www.nuget.org/packages/O9d.Guard)
[![NuGet](https://img.shields.io/nuget/dt/O9d.Guard.svg)](https://www.nuget.org/packages/O9d.Guard)
[![License](https://img.shields.io/:license-mit-blue.svg)](https://benfoster.mit-license.org/)

![Build](https://github.com/benfoster/o9d-guard/workflows/Build/badge.svg)
[![Coverage Status](https://coveralls.io/repos/github/benfoster/o9d-guard/badge.svg?branch=main)](https://coveralls.io/github/benfoster/o9d-guard?branch=main)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=benfoster_o9d-guard&metric=alert_status)](https://sonarcloud.io/dashboard?id=benfoster_o9d-guard)
[![GuardRails badge](https://api.guardrails.io/v2/badges/benfoster/o9d-guard.svg?token=461e73c50b8d8bfaf110ed2086379a8308a4fb8dd342334e79dcadd2dccf0f83&provider=github)](https://dashboard.guardrails.io/gh/benfoster/65586)
[![CodeScene Code Health](https://codescene.io/projects/12974/status-badges/code-health)](https://codescene.io/projects/12974)

Guard is a guard/assertions library for .NET that simplifies argument checking. 

**Without Guard:**

```c#
public Customer(string name, PhoneNumber phone)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        throw new ArgumentException("Name must be provided required", nameof(name));
    }

    Name = name;
    Phone = phone ?? throw new ArgumentNullException(nameof(phone));
}   
```

**With Guard:**

```c#
public Customer(string name, PhoneNumber phone)
{
    Name = name.NotNullOrWhiteSpace(nameof(name));
    Phone = phone.NotNull(nameof(phone));
}
```

## Quick Start

Add the O9d.Guard package from [NuGet](https://www.nuget.org/packages/O9d.Guard)

```
dotnet add package O9d.Guard
```

If you want to use a pre-release package, include the `--prerelease` option.

Import the `O9d.Guard` namespace and start using the extension to validate arguments.

## Building locally 

This project uses [Cake](https://cakebuild.net/) to build, test and publish packages. 

Run `build.sh` (Mac/Linux) or `build.ps1` (Windows) To build and test the project. 

This will output NuGet packages and coverage reports in the `artifacts` directory.

## Contributing

To contribute to O9d.Guard, fork the repository and raise a PR. If your change is substantial please [open an issue](https://github.com/benfoster/o9d-guard/issues) first to discuss your objective.

---

## Project Motivation

The main goal of this project was to demonstrate a number of best practices and tools for building and distributing .NET libraries.

### Code Structure

The project follows the standard .NET folder structure with source code nested under the `src` directory and tests under `test`. 

A common convention in .NET is to prefix libraries with the organisation or product name, for example `Microsoft.AspNet.Identity.EntityFramework`. This can create a lot of noise on disk so I prefer to follow [more recent conventions](https://github.com/dotnet/aspnetcore), using project properties to specify the extended namespace and package names. For common values we can make use of [Directory.Build.Props](https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2019) which saves duplicating this information in every csproj file.

This [C# Extensions plugin](https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions) honours the `RootNamespace` property when creating new types.

I'm targeting both .NET Standard and .NET 5.0 to make the most of the available frameworks, as per [these recommendations](https://devblogs.microsoft.com/dotnet/the-future-of-net-standard/).

### Testing

The project uses [XUnit](https://xunit.net/) for testing which seems to be the defacto choice in .NET today. Coupled with the [.NET Core Test Explorer Extension](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer) this creates quite a nice experience in VS Code. 

#### Test Coverage

Code coverage is recorded using [Coverlet](https://github.com/coverlet-coverage/coverlet). Unfortunately the [Coverlet Collector](https://www.nuget.org/packages/coverlet.collector) package that is included in the XUnit dotnet template does not support all the options needed to customise format and output location. As such I'm using the [MSBuild package](https://www.nuget.org/packages/coverlet.msbuild/) instead.

Each test project generates an OpenCover coverage file which is then merged using [Report Generator](https://github.com/danielpalme/ReportGenerator) into a single `lcov` file in `artifacts`. This means we can then use the [Coverage Gutters Extension](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters) to get code coverage visualisations within VS Code. A HTML and Text based report is generated for local use.

Code coverage is uploaded to both [Coveralls](https://coveralls.io/) and [Sonar Cloud](https://sonarcloud.io/) which are both free for Open Source projects. Each tool has a slightly different way of measuring code coverage:

- Coveralls measures the difference in total code coverage as the result of a change
- Sonar measures the code coverage within a change (a total coverage gate can only be added to long life branches)

Coverage checks are enabled for both PRs on every PR. Personally I prefer the Coveralls approach as it provides a clearer indication of a PR's impact on the overall coverage of the project.

### Versioning

Versioning is an important part of any library and my preference for a number of years has been to use [GitVersion](https://github.com/GitTools/GitVersion) to semantically version projects based on Git and a number of conventions. This avoids human error and is tightly integrated with most typicaly git workflows. 

GitVersion is like the swiss army knife of versioning tools and I've found that the more I've adopted a GitHub-flow way of working with PRs merged directly into master/main, the less I've needed advanced features. Ultimately I just want a way of auto-generating pre-release versions and defining release versions when needed.

[MinVer](https://github.com/adamralph/minver) fits these requirements perfectly. It installs as a [NuGet package](https://www.nuget.org/packages/MinVer/) into the projects you wish to version and even offers support for mono-repos using tag prefixes.

The MinVer [GitHub Page](https://github.com/adamralph/minver) clearly defines how versions are calculated but in short the _height_ since the last tag is added to the next 



- CI
- Security
- GitHub Repo setup




