using System.Text;

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
					if (symbols > 1) return symbols;
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
					if (CycleList[i][j].type == 'X' && !Xvalues.Contains(CycleList[i][j].impl))
						Xvalues.Add(CycleList[i][j].impl);
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
						CycleList.ElementAt(secpos - 1).Add(new RowInCyclesSection(implicants[implicant]));
						numOfElems--;
						pos++;
					}
				}
				section++;
			}
		}
		public void ToNextCycle()
		{
			bool ok = false;
			string temp;
			for (int i = 0; i < CycleList.Count - 1; i++)
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
							if (NextCycleList[i].Count(el => el.impl == temp) == 0)
								NextCycleList[i].Add(new RowInCyclesSection(temp));
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
				if (a[i] != '-' && a[i] != b[i]) return false;
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
