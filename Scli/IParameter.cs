namespace Scli
{
	public interface IParameter
	{
		public String ShortName { get; }
		public String? LongName { get; }
		public String? Description { get; }
		public Func<String?, Boolean> Validator { get; }
	}
}
