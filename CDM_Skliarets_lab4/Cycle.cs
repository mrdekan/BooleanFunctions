using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanFunctions
{
	internal class Cycle
	{
		#region Variables
		private List<List<RowInCyclesSection>> CycleList = new List<List<RowInCyclesSection>>();
		private List<List<RowInCyclesSection>> NextCycleList = new List<List<RowInCyclesSection>>();
		public List<string> Xvalues = new List<string>();
		public List<string> Implicants = new List<string>();
		public bool LastCycle { get; private set; }
		#endregion
		#region McCluskey method
		private int CheckDifference(string a, string b, ref string res)
		{
			int symbols = 0;
			StringBuilder str = new StringBuilder();
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					symbols++;
					str.Append('-');
				}
				else str.Append(a[i]);
			}
			res = str.ToString();
			return symbols;
		}
		private void AddXValues()
		{
			for (int i = 0; i < CycleList.Count; i++)
				for (int j = 0; j < CycleList[i].Count; j++)
					if (CycleList[i][j].type == 'X' && !Xvalues.Contains(new string(CycleList[i][j].impl)))
						Xvalues.Add(new string(CycleList[i][j].impl));
		}
		public void SetFromImplicants(Dictionary<int, string> implicants)
		{
			int numOfElems = implicants.Count, section = 0, pos = 0, secpos = 0;
			while (numOfElems > 0)
			{
				pos = 0;
				foreach (int implicant in implicants.Keys)
				{
					if (!Implicants.Contains(implicants[implicant]))
						Implicants.Add(implicants[implicant]);
					if (implicants[implicant].Count(c => c == '1') == section)
					{
						if (pos == 0)
						{
							CycleList.Add(new List<RowInCyclesSection>());
							secpos++;
						}
						CycleList.ElementAt(secpos - 1).Add(new RowInCyclesSection
						{
							impl = implicants[implicant],
							type = 'X'
						});
						numOfElems--;
						pos++;
					}
				}
				section++;
			}
		}
		private bool ContainsInList(List<RowInCyclesSection> list, string key)
		{
			for (int i = 0; i < list.Count; i++) if (list[i].impl == key) return true;
			return false;
		}
		public void ToNextCycle()
		{
			bool ok = false;
			string temp;
			int iterations = CycleList.Count - 1;
			for (int i = 0; i < iterations; i++)
			{
				NextCycleList.Add(new List<RowInCyclesSection>());
				for (int j = 0; j < CycleList[i].Count; j++)
				{
					for (int k = 0; k < CycleList[i + 1].Count; k++)
					{
						temp = "";
						if (CheckDifference(CycleList[i][j].impl, CycleList[i + 1][k].impl, ref temp) == 1)
						{
							ok = true;
							CycleList[i][j].type = 'V';
							CycleList[i + 1][k].type = 'V';
							if (!ContainsInList(NextCycleList.ElementAt(i), temp))
								NextCycleList.ElementAt(i).Add(new RowInCyclesSection
								{
									impl = temp,
									type = 'X'
								});
						}
					}
				}
			}
			AddXValues();
			LastCycle = !ok;
			CycleList = NextCycleList;
			NextCycleList = new List<List<RowInCyclesSection>>();
		}
        #endregion
        #region After cycles
        private bool ImplicantAndXvalue(string a, string b)
        {
			for(int i = 0; i < a.Length; i++)
            {
				if (a[i] == '-') continue;
				if (a[i] != b[i]) return false;
            }
			return true;
        }
		public bool[,] GetMatrix()
		{
			bool[,] matrix = new bool[Implicants.Count, Xvalues.Count];
			for (int x = 0; x < Implicants.Count; x++)
				for (int y = 0; y < Xvalues.Count; y++)
					matrix[x, y] = ImplicantAndXvalue(Xvalues[y], Implicants[x]);
			return matrix;
		}
		public List<string> GetCore()
		{
			bool[,] matrix = GetMatrix();
			List<string> res = new List<string>();
			for (int x = 0; x < matrix.GetLength(0); x++)
			{
				int count = 0, pos = 0;
				for(int y = 0; y < matrix.GetLength(1); y++)
				{
					if (matrix[x, y])
					{
						count++;
						pos = y;
					}
				}
				if (count == 1 && !res.Contains(Xvalues[pos])) res.Add(Xvalues[pos]);
			}
			return res;
		}
        #endregion
    }
}
