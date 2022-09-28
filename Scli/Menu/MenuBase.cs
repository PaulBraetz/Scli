using Scli.Command;
using System.Text;

namespace Scli.Menu
{
	public abstract class MenuBase : CommandBase, IMenu
	{
		protected MenuBase(string name) : base(name)
		{
		}

		private bool IsExitRequested { get; set; }

		public IEnumerable<ICommand> Actions { get; protected init; } = Array.Empty<ICommand>();

		public IEnumerable<IMenu> Children { get; protected init; } = Array.Empty<IMenu>();

		public void Exit()
		{
			IsExitRequested = true;
		}
		public override void Run()
		{
			IsExitRequested = false;
			while (!IsExitRequested)
			{
				var navigation = GetMenuNavigation();
				var input = Read(navigation);
				var ran = Children.Any(c => c.TryRun(input)) ||
						  Actions.Any(c => c.TryRun(input));

				if (!ran)
				{
					Console.WriteLine($"invalid input: \"{input}\"");
				}
			}
		}

		protected virtual string GetMenuNavigation()
		{
			var builder = new StringBuilder("-- ")
				.Append(Name)
				.Append(" --");

			if (Children?.Any() ?? false)
			{
				_ = builder.Append("\nNavigation:\n").Append(string.Join("\n", Children.Select(c => c.GetSelfNavigation())));
			}

			if (Actions?.Any() ?? false)
			{
				_ = builder.Append("\nActions:\n").Append(string.Join("\n", Actions.Select(c => c.GetSelfNavigation())));
			}

			var result = builder.ToString();

			return result;
		}
	}
}

