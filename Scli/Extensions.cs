using Fort;
using Scli.Command;
using Scli.Menu;

namespace Scli
{
	public static class Extensions
	{
		public static void Print<T>(this T value, Func<T, String>? transform = null)
		{
			Console.WriteLine((transform ?? (v => v?.ToString() ?? String.Empty))(value));
		}
		public static void Print<T>(this T value)
			where T : Exception
		{
			value?.Print(e => $"an error has occured: {value}");
		}
		public static Boolean TryRun(this ICommand action, String navigationKey)
		{
			navigationKey.ThrowIfDefault(nameof(navigationKey));

			if (action.NavigationKey == navigationKey)
			{
				try
				{
					action.Run();
				}
				catch (Exception ex)
				{
					ex.Print();
				}

				return true;
			}

			return false;
		}
		public static CommandCollectionBuilder<ICommand> Append(this CommandCollectionBuilder<ICommand> builder, String name, Action action)
		{
			return builder.Append(k => new Strategy(name, action, k));
		}
		public static CommandCollectionBuilder<IMenu> Append(this CommandCollectionBuilder<IMenu> builder, String name, IEnumerable<IMenu>? children = null, IEnumerable<ICommand>? actions = null, String backName = "Back")
		{
			return builder.Append(k => new Menu.Menu(name, children ?? Array.Empty<IMenu>(), actions ?? Array.Empty<ICommand>(), k, backName));
		}
		public static CommandCollectionBuilder<ICommand> AppendConfirmation(this CommandCollectionBuilder<ICommand> builder, String name, Action action, String prompt = "Please Confirm", String confirm = "y", String deny = null)
		{
			return builder.Append(k => new Confirmation(name, action, prompt, confirm, deny, k));
		}

		public static String GetNameString(this IParameter parameter)
		{
			return parameter.LongName != null ? $"{parameter.ShortName} or {parameter.LongName}" : parameter.ShortName;
		}
	}
}
