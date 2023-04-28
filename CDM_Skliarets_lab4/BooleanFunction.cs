using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanFunctions
{
	internal class BooleanFunction
	{
		//test string b+(d+c*b*(!a+!e)+a+d)
		public string Calculate(string value, Dictionary<string, int> arguments)
		{
			while (value.Contains('('))
			{
				int pos = value.IndexOf('(');
				string temp = cutHooks(value, pos);
				value = value.Remove(pos, temp.Length + 2).Insert(pos, Calculate(temp, arguments));
			}
			char[] arr = value.ToCharArray();
			for (int i = 0; i < arr.Length; i++)
			{
				if (arguments.ContainsKey(arr[i].ToString()))
					arr[i] = arguments[arr[i].ToString()].ToString()[0];
			}
			value = new string(arr);
			value = value
				.Replace("!!", "")
				.Replace("!0", "1")
				.Replace("!1", "0");
			while (value.Contains("*"))
				value = value
					.Replace("1*0", "0")
					.Replace("0*1", "0")
					.Replace("1*1", "1")
					.Replace("0*0", "0");
			while (value.Contains("+"))
				value = value
					.Replace("1+0", "1")
					.Replace("0+1", "1")
					.Replace("1+1", "1")
					.Replace("0+0", "0");
			return value;
		}
		public string cutHooks(string value, int pos)
		{
			StringBuilder str = new StringBuilder();
			int hooks = 1;
			for (pos++; pos < value.Length; pos++)
			{
				if (value[pos] == '(') hooks++;
				else if (value[pos] == ')') hooks--;
				if (hooks == 0) break;
				str.Append(value[pos]);
			}
			return str.ToString();
		}
	}
}
