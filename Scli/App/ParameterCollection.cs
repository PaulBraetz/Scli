using Fort;
using System.Collections;
using System.Text;

namespace Scli
{
	public static partial class Initialization
	{
		private sealed partial class ParameterCollection : IParameterCollection
		{
			private readonly ISet<String?> _names = new HashSet<String?>();
			private readonly ICollection<IParameter> _parameters = new List<IParameter>();

			public Boolean TryAdd(String shortName, String? longName = null, String? description = null, Func<String?, Boolean>? validator = null)
			{
				validator ??= s => true;
				var parameter = new Parameter(shortName, validator, longName, description);

				if (_names.Contains(parameter.ShortName))
				{
					return false;
				}

				if (longName != null)
				{
					if (_names.Contains(parameter.LongName))
					{
						return false;
					}

					_ = _names.Add(parameter.LongName);
				}

				_ = _names.Add(parameter.ShortName);

				_parameters.Add(parameter);

				return true;
			}

			public Boolean TryRemove(String shortName)
			{
				var parameter = _parameters.FirstOrDefault(p => p.ShortName == $"-{shortName}");
				if (parameter == null)
				{
					return false;
				}

				_ = _names.Remove(parameter.ShortName);
				_ = _names.Remove(parameter.LongName);

				_ = _parameters.Remove(parameter);

				return true;
			}

			public Boolean TryGet(String shortName, out IParameter? parameter)
			{
				shortName.ThrowIfDefault(nameof(shortName));

				parameter = _parameters.SingleOrDefault(p => p.ShortName == shortName);

				return parameter != null;
			}

			public IEnumerator<IParameter> GetEnumerator()
			{
				return _parameters.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
			public override String ToString()
			{
				var rightPadding = _parameters.Select(p => p.GetNameString().Length).OrderBy(p => p).Last();
				var nameHeader = "Name";
				var arrow = "    ";

				rightPadding = Math.Clamp(rightPadding, nameHeader.Length, Int32.MaxValue);

				var builder = new StringBuilder("Parameters:\n\t");

				_ = builder.Append(nameHeader.PadRight(rightPadding + arrow.Length))
					.Append("Description");

				foreach (var parameter in _parameters)
				{
					_ = builder.Append("\n\t");
					if (parameter.Description != null)
					{
						_ = builder.Append(parameter.GetNameString().PadRight(rightPadding))
							.Append(arrow)
							.Append(parameter.Description);
					}
					else
					{
						_ = builder.Append(parameter.GetNameString());
					}
				}

				return builder.ToString();
			}

			public IArgumentCollection MatchArguments(String[] arguments)
			{
				return new ArgumentCollection(arguments, this);
			}
		}
	}
}
