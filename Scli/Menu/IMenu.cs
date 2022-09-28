using Scli.Command;

namespace Scli.Menu
{
	public interface IMenu : ICommand
	{
		IEnumerable<ICommand> Actions { get; }
		IEnumerable<IMenu> Children { get; }
		void Exit();
	}
}
