# Overview

There are over 1600 lines of code. It is near-production quality but there should be
more logging, more unit tests, etc. If this was production code, I would have used an 
Azure Function to read the Volume Stream endpoint and implemented an Azure Web App
Service.

# Configuration

All environment configuration values are prefixed by TWITTER_API_. The required
environment variables for the application are: 
* TWITTER_API_BEARER_TOKEN: the bearer token provided by Twitter that allows access to the Twitter API V2.

# Coding Style

The .editorconfig to enforce a coding style was downloaded from the 
[dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/.editorconfig) project.

# Framework/Tools

.NET 6, C# 10, Visual Studio 2022

# Projects

* Twitter.VolumeStream.Tests: xUnit unit tests
* Twitter.VolumeStream.Demo: class library that implements the challenge
* Twitter.VolumeStream.Demo: console application that displays tweet count and top hashtags every 10 seconds.
