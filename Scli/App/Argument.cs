using Fort;

namespace Scli
{
	public static partial class Initialization
	{
		private partial class ParameterCollection
		{
			private readonly partial struct ArgumentCollection
			{
				private readonly struct Argument : IArgument, IEquatable<Argument>
				{
					public Argument(String? value, IParameter definition)
					{
						definition.ThrowIfDefault(nameof(definition));

						if (!definition.Validator.Invoke(value))
						{
							throw new InvalidValueException(value, definition);
						}

						Value = value;
						Definition = definition;
					}

					public readonly String? Value { get; }
					public readonly IParameter Definition { get; }

					public override Boolean Equals(Object? obj)
					{
						return obj is Argument arg && Equals(arg);
					}

					public Boolean Equals(Argument other)
					{
						return Value == other.Value &&
							   EqualityComparer<IParameter>.Default.Equals(Definition, other.Definition);
					}

					public override Int32 GetHashCode()
					{
						return HashCode.Combine(Value, Definition);
					}

					public static Boolean operator ==(Argument left, Argument right)
					{
						return left.Equals(right);
					}

					public static Boolean operator !=(Argument left, Argument right)
					{
						return !(left == right);
					}

					public override String ToString()
					{
						String valueString = Helpers.GetValueString(Value);
						return $"{Definition.ShortName} = {valueString}";
					}
				}
			}
		}
	}
}
