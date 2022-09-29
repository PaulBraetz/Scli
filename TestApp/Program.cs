using Scli;
using Scli.Command;
using Scli.Menu;

namespace TestApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			new MainMenu().Run();
		}
	}

	internal sealed class MainMenu : ExitableMenuBase
	{
		public MainMenu() : base("Main Menu", "Exit")
		{
			new CommandCollectionBuilder<IMenu>(10)
				.Append(Children)
				.Append("SubMenu 1", Array.Empty<IMenu>(), Array.Empty<ICommand>())
				.Append("SubMenu 2", Array.Empty<IMenu>(), Array.Empty<ICommand>())
				.Append("SubMenu 3", Array.Empty<IMenu>(), Array.Empty<ICommand>())
				.Build(out var children)
				.Next<ICommand>()
				.Append(Actions)
				.AppendConfirmation("Echo", () => Console.WriteLine(Read("input: ")), "Do you really want to do this?")
				.Append("ToLower", () => Console.WriteLine(Read("input: ").ToLowerInvariant()))
				.Append("ToUpper", () => Console.WriteLine(Read("input: ").ToUpperInvariant()))
				.Build(out var actions);

			Children = children;
			Actions = actions;
		}
	}
}