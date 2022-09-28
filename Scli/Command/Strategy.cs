using Fort;

namespace Scli.Command
{
	public sealed class Strategy : CommandBase
	{
		public Strategy(String name, Action strategy, String navigationKey = "-1") : base(name)
		{
			strategy.ThrowIfDefault(nameof(strategy));

			_strategy = strategy;
			NavigationKey = navigationKey;
		}

		private readonly Action _strategy;

		public override void Run()
		{
			_strategy.Invoke();
		}
	}
}
