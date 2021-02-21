<img alt="Guard Icon" src="src/Guard/assets/icon.png" width="64px" />

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

## Docs

The Guard documentation is built using [DocFx](https://dotnet.github.io/docfx/). To build and serve the docs locally run:

```
./build.sh --target ServeDocs
```

This will serve the docs on http://localhost:8080.
