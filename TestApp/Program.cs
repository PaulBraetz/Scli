
using Scli;

IParameterCollection parameters = Initialization.GetParameters();
parameters.TryAdd("r", description: "This is a description for the \"-r\" parameter.");
parameters.TryAdd("s", "second-parameter");
parameters.TryAdd("l");
parameters.TryAdd("b", validator: s => Boolean.TryParse(s, out Boolean _));
parameters.TryAdd("h", "help", "Show Help", s => s == null);

Console.WriteLine(parameters);

try
{
	var arguments = parameters.MatchArguments(args);

	Console.WriteLine(arguments);
}
catch (Exception ex)
{
	Console.WriteLine(ex);
}