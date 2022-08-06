# Scli #

Scli provides a primitive parser for CLI arguments.

## Versioning ##

Scli uses [Semantic Versioning 2.0.0](https://semver.org/).

## Installation ##

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.Scli

Package Manager: `Install-Package RhoMicro.Scli -Version 1.0.0`

.Net CLI: `dotnet add package RhoMicro.Scli --version 1.0.0`

## How To Use ##

Define possible parameters using an `IParameterCollection`.
```cs
using Scli;

IParameterCollection parameters = Initialization.GetParameters();
parameters.TryAdd("r", description: "This is a description for the \"-r\" parameter.");
parameters.TryAdd("s", "second-parameter");
parameters.TryAdd("l");
parameters.TryAdd("b", validator: s => Boolean.TryParse(s, out Boolean _));
parameters.TryAdd("h", "help", "Show Help", s => s == null);

Console.WriteLine(parameters);
```

Receive a parsed `IArgumentCollection` using `IParameterCollection.MatchArguments`
```cs
try
{
	var arguments = parameters.MatchArguments(args);

	arguments.TryGet("b", Boolean.Parse, out Boolean parsedValue);

	Console.WriteLine(arguments);
}
catch (Exception ex)
{
	Console.WriteLine(ex);
}
```
