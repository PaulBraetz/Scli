using Fort;
using Scli.Command;

namespace Scli.Menu
{
	public sealed class Menu : ExitableMenuBase
	{
		public Menu(String name, IEnumerable<IMenu> children, IEnumerable<ICommand> actions, String navigationKey = "-1", String backName = "Back") : base(name, backName)
		{
			children.ThrowIfDefault(nameof(children));
			actions.ThrowIfDefault(nameof(actions));

			Children = new CommandCollectionBuilder<IMenu>(Children).Append(children).Build();
			Actions = new CommandCollectionBuilder<ICommand>(Actions).Append(actions).Build();
			NavigationKey = navigationKey;
		}
	}
}
