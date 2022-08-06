namespace Scli
{
	public interface IArgumentCollection : IEnumerable<IArgument>
	{
		public Boolean TryGet(String shortName, out IArgument? argument);
		public Boolean TryGet<TValue>(String shortName, Func<String?, TValue> parser, out TValue? value);
	}
}
