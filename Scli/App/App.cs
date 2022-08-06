using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scli
{
	public static partial class Initialization
	{
		public static IParameterCollection GetParameters()
		{
			return new ParameterCollection();
		}
	}
}
