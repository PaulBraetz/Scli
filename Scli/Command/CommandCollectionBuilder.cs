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
		public CommandCollectionBuilder(T action)
		{
			_ = Append(action);
		}
		public CommandCollectionBuilder(IEnumerable<T> actions)
		{
			_ = Append(actions);
		}
		public CommandCollectionBuilder(UInt32 startAtKey, ISet<UInt32>? keysTaken = null)
		{
			_keysTaken = keysTaken ?? new HashSet<UInt32>();
			_lastKey = startAtKey;
		}

		private readonly ICollection<T> _actions = new List<T>();
		private readonly ICollection<Func<String, T>> _factories = new List<Func<String, T>>();
		private readonly ISet<UInt32> _keysTaken = new HashSet<UInt32>();
		private UInt32 _lastKey = 0;

		public CommandCollectionBuilder<T> Append(Func<String, T> factory)
		{
			factory.ThrowIfDefault(nameof(factory));
			_factories.Add(factory);
			return this;
		}
		public CommandCollectionBuilder<T> Append(T action)
		{
			action.ThrowIfDefault(nameof(action));
			_actions.Add(action);
			return this;
		}
		public CommandCollectionBuilder<T> Append(IEnumerable<T> actions)
		{
			actions.ThrowIfDefault(nameof(actions));

			foreach (var action in actions)
			{
				_ = Append(action);
			}

			return this;
		}

		public IEnumerable<T> Build()
		{
			var keysTaken = new HashSet<UInt32>(_actions.Select(c => (success: UInt32.TryParse(c.NavigationKey, out var i), index: i))
				.Where(t => t.success)
				.Select(t => t.index)
				.Concat(_keysTaken));

			var key = Math.Max(keysTaken.OrderBy(i => i).FirstOrDefault(), _lastKey);

			var actions = _factories.Select(f =>
				{
					while (keysTaken.Contains(key))
					{
						key++;
					}

					var navigationKey = key.ToString();
					var action = f.Invoke(navigationKey);
					if (action.NavigationKey == navigationKey)
					{
						_ = keysTaken.Add(key);
					}

					return action;
				}).Concat(_actions)
				.Select(c => (action: c, index: UInt32.TryParse(c.NavigationKey, out var i) ? i : UInt32.MaxValue))
				.OrderBy(t => t.index)
				.Select(t => t.action)
				.ToArray();

			_lastKey = key;
			foreach (var keyTaken in keysTaken)
			{
				_ = _keysTaken.Add(keyTaken);
			}

			return actions;
		}

		public CommandCollectionBuilder<T> Build(out IEnumerable<T> actions)
		{
			actions = Build();
			return this;
		}
		public CommandCollectionBuilder<T> Reset()
		{
			_factories.Clear();
			_actions.Clear();
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
