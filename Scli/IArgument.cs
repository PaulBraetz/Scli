using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scli
{
	public interface IArgument
	{
		String? Value { get; }
		IParameter Definition { get; }
	}
}
