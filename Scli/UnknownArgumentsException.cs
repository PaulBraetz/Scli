using Fort;

namespace Scli
{
	public sealed class UnknownArgumentsException : Exception
	{
		public UnknownArgumentsException(IEnumerable<String> arguments)
		{
			arguments.ThrowIfDefaultOrEmpty(nameof(arguments));

			Arguments = arguments;
			_message = $"Unknown arguments: {String.Join(", ", arguments)}";
		}

		public IEnumerable<String> Arguments { get; }

		private readonly String _message;

		public override String Message => _message;

		public override String ToString()
		{
			return Message;
		}
	}
}
