using Fort;
using Scli.Command;

namespace Scli.Menu
{
	public sealed class Menu : ExitableMenuBase
	{
		public Menu(String name, IEnumerable<IMenu> children, IEnumerable<ICommand> commands, String navigationKey = "-1", String backName = "Back") : base(name, backName)
		{
			children.ThrowIfDefault(nameof(children));
			commands.ThrowIfDefault(nameof(commands));

			Children = new CommandCollectionBuilder<IMenu>(Children).Append(children).Build();
			Actions = new CommandCollectionBuilder<ICommand>(Actions).Append(commands).Build();
			NavigationKey = navigationKey;
		}
	}
}
