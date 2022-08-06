namespace Scli
{
	public static class Helpers
	{
		public static String GetValueString(String? value)
		{
			return value != null ? @$"""{value}""" : "null";
		}
	}
}
