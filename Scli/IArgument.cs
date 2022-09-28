namespace Scli
{
	public interface IArgument
	{
		String? Value { get; }
		IParameter Definition { get; }
	}
}
