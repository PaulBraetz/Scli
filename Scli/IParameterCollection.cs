namespace Scli
{
	public interface IParameterCollection : IEnumerable<IParameter>
	{
		public IArgumentCollection MatchArguments(String[] arguments);
		public Boolean TryAdd(String shortName, String? longName = null, String? description = null, Func<String?, Boolean>? validator = null);
		public Boolean TryRemove(String shortName);
		public Boolean TryGet(String shortName, out IParameter? parameter);
	}
}
