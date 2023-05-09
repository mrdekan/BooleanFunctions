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
		public string GetMDNF(Dictionary<int, string> implicants)
        {
			Cycle cycle = new Cycle();
			cycle.SetFromImplicants(implicants);
			int cyclas = 0;
			while (!cycle.LastCycle)
			{
				cycle.ToNextCycle();
				cyclas++;
			}
			List<string> core = cycle.GetCore();
			List<int> coreIndexes = new List<int>();
			List<int> coreCover = new List<int>();
			bool[,] matrix = cycle.GetMatrix();
			foreach(string index in core)
            {
				int temp = cycle.Xvalues.IndexOf(index);
				if (!coreIndexes.Contains(temp))
				{
					coreIndexes.Add(temp);
					for(int i = 0; i < cycle.Implicants.Count; i++)
						if (matrix[i, temp] && !coreCover.Contains(i)) coreCover.Add(i);
				}
            }
			bool[,] newMatrix = new bool[cycle.Implicants.Count-coreCover.Count,cycle.Xvalues.Count-coreIndexes.Count];
			
			for (int y = 0, posY=0; y < matrix.GetLength(1); y++)
			{
				if (coreIndexes.Contains(y)) continue;
				for (int x = 0, posX=0; x < matrix.GetLength(0); x++)
				{
                    if (!coreCover.Contains(x))
                    {
						newMatrix[posX, posY] = matrix[x, y];
						posX++;
                    }
				}
				posY++;
			}
			string res = "";
			for (int y = 0; y < newMatrix.GetLength(1); y++)
			{
				for (int x = 0; x < newMatrix.GetLength(0); x++)
				{
					res += newMatrix[x, y] ? "* " : "_ ";
				}
				res += "\n";
			}
			return res+String.Join(',',core);
        }
	}
}
