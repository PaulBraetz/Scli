using Fort;
using System.Diagnostics;

namespace Scli.Command
{
	[DebuggerDisplay("{GetSelfNavigation()}")]
	public abstract class CommandBase : ICommand
	{
		protected CommandBase(string name)
		{
			name.ThrowIfDefaultOrEmpty(nameof(name));

			Name = name;
		}

		protected string Name { get; }

		private String _navigationKey = "-1";
		public string NavigationKey
		{
			get => _navigationKey;
			protected init
			{
				value.ThrowIfDefaultOrEmpty(nameof(NavigationKey));
				_navigationKey = value;
			}
		}

		protected string Read()
		{
			var result = Console.ReadLine() ?? string.Empty;

			return result;
		}
		protected string Read(string prompt)
		{
			Console.WriteLine(prompt);
			var result = Console.ReadLine() ?? string.Empty;

			return result;
		}
		private T Read<T>(Func<String, T> parser, Func<String> reader)
		{
			parser.ThrowIfDefault(nameof(parser));
			reader.ThrowIfDefault(nameof(reader));

			T? result = default;
			string? input = null;
			while (input == null)
			{
				try
				{
					input = reader.Invoke();
					result = parser(input);
				}
				catch (Exception ex)
				{
					ex.Print();
					input = null;
				}
			}

			return result!;
		}

		protected T Read<T>(string prompt, Func<string, T> parser)
		{
			var result = Read(parser, () => Read(prompt));

			return result;
		}
		protected T Read<T>(Func<string, T> parser)
		{
			var result = Read(parser, () => Read());

			return result;
		}

		public string GetSelfNavigation()
		{
			var selfNavigation = $"{NavigationKey} -> {Name}";

			return selfNavigation;
		}

		public abstract void Run();
	}
}

