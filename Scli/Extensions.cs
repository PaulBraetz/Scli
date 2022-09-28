using Fort;
using Scli.Command;

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
		public static Boolean TryRun(this ICommand command, String navigationKey)
		{
			navigationKey.ThrowIfDefault(nameof(navigationKey));

			if (command.NavigationKey == navigationKey)
			{
				try
				{
					command.Run();
				}
				catch (Exception ex)
				{
					ex.Print();
				}

				return true;
			}

			return false;
		}

		public static String GetNameString(this IParameter parameter)
		{
			return parameter.LongName != null ? $"{parameter.ShortName} or {parameter.LongName}" : parameter.ShortName;
		}
	}
}
