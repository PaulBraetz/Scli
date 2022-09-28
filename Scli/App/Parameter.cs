using Fort;

namespace Scli
{
	public static partial class Initialization
	{
		private sealed partial class ParameterCollection
		{
			private readonly struct Parameter : IParameter, IEquatable<Parameter>
			{
				public Parameter(String shortName, Func<String?, Boolean> validator, String? longName = null, String? decription = null)
				{
					shortName.ThrowIfNot(isValidShortName, $"Invalid name {shortName}", nameof(shortName));
					validator.ThrowIfDefault(nameof(validator));
					longName.ThrowIfNot(isValidLongName, $"Invalid name {longName}", nameof(longName));

					ShortName = $"-{shortName}";
					LongName = longName != null ? $"--{longName}" : longName;
					Validator = validator;
					Description = decription;

					static Boolean isValidShortName(String? name)
					{
						return name != null && name.Length > 0 && name[0] != '-';
					}

					static Boolean isValidLongName(String? name)
					{
						return name == null || isValidShortName(name);
					}
				}

				public readonly String ShortName { get; }
				public readonly String? LongName { get; }
				public readonly String? Description { get; }
				public readonly Func<String?, Boolean> Validator { get; }

				public override Boolean Equals(Object? obj)
				{
					return obj is Parameter definition && Equals(definition);
				}

				public Boolean Equals(Parameter other)
				{
					return ShortName == other.ShortName;
				}

				public override Int32 GetHashCode()
				{
					return HashCode.Combine(ShortName);
				}

				public static Boolean operator ==(Parameter left, Parameter right)
				{
					return left.Equals(right);
				}

				public static Boolean operator !=(Parameter left, Parameter right)
				{
					return !(left == right);
				}

				public override String ToString()
				{
					var nameString = this.GetNameString();
					var descriptionString = Description != null ? $": {Description}" : Description;
					return $"{nameString}{descriptionString}";
				}
			}
		}
	}
}
