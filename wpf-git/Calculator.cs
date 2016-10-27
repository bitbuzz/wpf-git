using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_git
{
	public class Calculator
	{
		public int Add(List<int> values)
		{
			int total = 0;
			foreach(var i in values)
			{
				total += i;
			}
			return total;
		}
	}
}
