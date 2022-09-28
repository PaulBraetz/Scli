using Fort;
using System.Collections;

namespace Scli
{
	public static partial class Initialization
	{
		private partial class ParameterCollection
		{
			private readonly partial struct ArgumentCollection : IArgumentCollection
			{
				public ArgumentCollection(String[] cliArgs, IParameterCollection parameters)
				{
					cliArgs.ThrowIfDefaultOrNot(args => !args.Contains(default), $"{nameof(cliArgs)} may not contain a null value.", nameof(cliArgs));
					parameters.ThrowIfDefaultOrNot(defs => !defs.Contains(default!), $"{nameof(parameters)} may not contain a default value.", nameof(parameters));

					var cliArgsList = cliArgs.ToList();
					var args = new List<IArgument>();

					foreach (var parameter in parameters)
					{
						try
						{
							var name = cliArgsList.SingleOrDefault(a => a == parameter.ShortName || a == parameter.LongName);

							if (name != null)
							{
								var index = cliArgsList.IndexOf(name);
								var value = cliArgsList.Count > index + 1 ? cliArgsList[index + 1] : null;
								var isValidValue = value != null && !value.StartsWith('-');

								IArgument arg = isValidValue ?
									new Argument(value, parameter) :
									new Argument(null, parameter);

								args.Add(arg);
								_ = cliArgsList.Remove(name);
								if (isValidValue)
								{
									_ = cliArgsList.Remove(value!);
								}
							}
						}
						catch (InvalidOperationException)
						{
							throw new MultipleFlagsException(parameter);
						}
					}

					if (cliArgsList.Count > 0)
					{
						throw new UnknownArgumentsException(cliArgsList);
					}

					_arguments = args.AsReadOnly();
				}

				private readonly IReadOnlyCollection<IArgument> _arguments;
				public override String ToString()
				{
					return $"Arguments:\n\t{String.Join("\n\t", _arguments)}";
				}

				public Boolean TryGet(String shortName, out IArgument? argument)
				{
					shortName.ThrowIfDefault(nameof(shortName));

					argument = _arguments.SingleOrDefault(a => a.Definition.ShortName == $"-{shortName}");

					return argument != null;
				}

				public Boolean TryGet<TValue>(String shortName, Func<String?, TValue> parser, out TValue? value)
				{
					if (TryGet(shortName, out var argument))
					{
						value = parser.Invoke(argument!.Value);
						return true;
					}

					value = default;
					return false;
				}

				public IEnumerator<IArgument> GetEnumerator()
				{
					return _arguments.GetEnumerator();
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}
			}
		}
	}
}
