using Fort;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scli
{
	public sealed class MultipleFlagsException : Exception
	{
		public MultipleFlagsException(IParameter definition) : base(GetMessage(definition))
		{
			definition.ThrowIfDefault(nameof(definition));

			Definition = definition;
		}

		public IParameter Definition { get; }

		private static String GetMessage(IParameter definition)
		{
			definition.ThrowIfDefault(nameof(definition));

			return $"{definition.GetNameString()} was supplied multiple times";
		}
	}
}
