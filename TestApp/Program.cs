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
				.Append(k => new Menu("SubMenu 1", Array.Empty<IMenu>(), Array.Empty<ICommand>(), k))
				.Append(k => new Menu("SubMenu 2", Array.Empty<IMenu>(), Array.Empty<ICommand>(), k))
				.Append(k => new Menu("SubMenu 3", Array.Empty<IMenu>(), Array.Empty<ICommand>(), k))
				.Build(out var children)
				.Next<ICommand>()
				.Append(Actions)
				.Append(k => new Strategy("Echo", () => Console.WriteLine(Read("input: ")), k))
				.Append(k => new Strategy("ToLower", () => Console.WriteLine(Read("input: ").ToLowerInvariant()), k))
				.Append(k => new Strategy("ToUpper", () => Console.WriteLine(Read("input: ").ToUpperInvariant()), k))
				.Build(out var actions);

			Children = children;
			Actions = actions;
		}
	}
}