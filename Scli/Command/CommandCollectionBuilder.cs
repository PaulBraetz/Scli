using Fort;

namespace Scli.Command
{
	public sealed class CommandCollectionBuilder<T>
		where T : ICommand
	{
		public CommandCollectionBuilder()
		{

		}
		public CommandCollectionBuilder(Func<String, T> factory)
		{
			_ = Append(factory);
		}
		public CommandCollectionBuilder(T command)
		{
			_ = Append(command);
		}
		public CommandCollectionBuilder(IEnumerable<T> commands)
		{
			_ = Append(commands);
		}
		public CommandCollectionBuilder(UInt32 startAtKey, ISet<UInt32>? keysTaken = null)
		{
			_keysTaken = keysTaken ?? new HashSet<UInt32>();
			_lastKey = startAtKey;
		}

		private readonly ICollection<T> _commands = new List<T>();
		private readonly ICollection<Func<String, T>> _factories = new List<Func<String, T>>();
		private readonly ISet<UInt32> _keysTaken = new HashSet<UInt32>();
		private UInt32 _lastKey = 0;

		public CommandCollectionBuilder<T> Append(Func<String, T> factory)
		{
			factory.ThrowIfDefault(nameof(factory));
			_factories.Add(factory);
			return this;
		}
		public CommandCollectionBuilder<T> Append(T command)
		{
			command.ThrowIfDefault(nameof(command));
			_commands.Add(command);
			return this;
		}
		public CommandCollectionBuilder<T> Append(IEnumerable<T> commands)
		{
			commands.ThrowIfDefault(nameof(commands));

			foreach (var command in commands)
			{
				_ = Append(command);
			}

			return this;
		}

		public IEnumerable<T> Build()
		{
			var keysTaken = new HashSet<UInt32>(_commands.Select(c => (success: UInt32.TryParse(c.NavigationKey, out var i), index: i))
				.Where(t => t.success)
				.Select(t => t.index)
				.Concat(_keysTaken));

			var key = Math.Max(keysTaken.OrderBy(i => i).FirstOrDefault(), _lastKey);

			var commands = _factories.Select(f =>
				{
					while (keysTaken.Contains(key))
					{
						key++;
					}

					var navigationKey = key.ToString();
					var command = f.Invoke(navigationKey);
					if (command.NavigationKey == navigationKey)
					{
						_ = keysTaken.Add(key);
					}

					return command;
				}).Concat(_commands)
				.Select(c => (command: c, index: UInt32.TryParse(c.NavigationKey, out var i) ? i : UInt32.MaxValue))
				.OrderBy(t => t.index)
				.Select(t => t.command)
				.ToArray();

			_lastKey = key;
			foreach (var keyTaken in keysTaken)
			{
				_ = _keysTaken.Add(keyTaken);
			}

			return commands;
		}

		public CommandCollectionBuilder<T> Build(out IEnumerable<T> commands)
		{
			commands = Build();
			return this;
		}
		public CommandCollectionBuilder<T> Reset()
		{
			_factories.Clear();
			_commands.Clear();
			_keysTaken.Clear();
			_lastKey = 0;

			return this;
		}

		public CommandCollectionBuilder<TNext> Next<TNext>()
			where TNext : ICommand
		{
			var next = new CommandCollectionBuilder<TNext>(_lastKey, _keysTaken);

			return next;
		}
	}
}
