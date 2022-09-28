using Fort;
using Scli.Menu;

namespace Scli.Menu
{
	public sealed class Exit : MenuBase
	{
		public Exit(String name, IMenu menu, String navigationKey = "-1") : base(name)
		{
			menu.ThrowIfDefault(nameof(menu));

			_menu = menu;
			NavigationKey = navigationKey;
		}

		private readonly IMenu _menu;

		public override void Run()
		{
			_menu.Exit();
		}
	}
}
