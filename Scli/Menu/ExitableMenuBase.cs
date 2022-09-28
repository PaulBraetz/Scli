using Fort;
using Scli.Command;

namespace Scli.Menu
{
	public abstract class ExitableMenuBase : MenuBase
	{
		protected ExitableMenuBase(String name, String backName = "Back") : base(name)
		{
			backName.ThrowIfDefaultOrEmpty(nameof(backName));

			var exit = new Exit(backName, this, 0.ToString());

			Children = new CommandCollectionBuilder<IMenu>(Children).Append(exit).Build();
		}
	}
}
