using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scli
{
	public static class Extensions
	{
		public static String GetNameString(this IParameter parameter)
		{
			return parameter.LongName != null ? $"{parameter.ShortName} or {parameter.LongName}" : parameter.ShortName;
		}
	}
}
