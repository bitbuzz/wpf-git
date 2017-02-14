using System.Collections.Generic;

namespace wpf_git
{
	public static class Calculator
	{
		public static int Add(List<int> values)
		{
			int total = 0;
			foreach(var i in values)
			{
				total += i;
			}
			return total;
		}

		public static int Subtract(List<int> values)
		{
			int total = 0;
			foreach (var i in values)
			{
				total -= i;
			}
			return total;
		}

		public static int Multiply(List<int> values)
		{
			int total = 0;
			foreach (var i in values)
			{
				total *= i;
			}
			return total;
		}
	}
}
