using Fort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scli.Command
{
	public class Confirmation : Strategy
	{
		public Confirmation(String name, Action action, String prompt = "Please Confirm", String confirm = "y", String? deny = null, String navigationKey = "-1") : base(name, action, navigationKey)
		{
			prompt.ThrowIfDefault(nameof(prompt));
			confirm.ThrowIfDefault(nameof(confirm));

			_prompt = $"{prompt} ({(deny == null?$"{confirm} to confirm":$"{confirm}/{deny}")})";
			_confirm = confirm;
		}
		private readonly String _prompt;
		private readonly String _confirm;

		public override void Run()
		{
			if(Read(_prompt) == _confirm)
			{
				base.Run();
			}
		}
	}
}
