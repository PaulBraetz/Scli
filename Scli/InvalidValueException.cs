using Fort;

namespace Scli
{
	public sealed class InvalidValueException : Exception
	{
		public InvalidValueException(String? value, IParameter definition)
		{
			definition.ThrowIfDefault(nameof(definition));

			Value = value;
			Definition = definition;
			var valueString = Helpers.GetValueString(value);
			_message = $"{definition.GetNameString()} received invalid value {valueString}";
		}

		public String? Value { get; }
		public IParameter Definition { get; }
		private readonly String _message;

		public override String Message => _message;

		public override String ToString()
		{
			return Message;
		}
	}
}
