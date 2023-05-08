using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanFunctions
{
	struct RowInSection
	{
		public char[] arr { get; set; }
		public string numbers { get; set; }
		public char type { get; set; }
	}
	internal class Cycle
	{
		List<List<RowInSection>> CycleList = new List<List<RowInSection>>();
		List<List<RowInSection>> NextCycleList = new List<List<RowInSection>>();
		List<string> Xvalues = new List<string>();
		public bool LastCycle { get; private set; }
		public Cycle GetNext()
		{
			Cycle NextCycle = new Cycle();
			LastCycle = false;
			return NextCycle;
		}
		private int CheckDifference(char[] a, char[] b, ref char[] res)
		{
			int symbols = 0;
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					symbols++;
					res[i] = '-';
				}
				else res[i] = a[i];
			}
			return symbols;
		}
		private void AddXValues()
		{
			for (int i = 0; i < CycleList.Count; i++)
			{
				for (int j = 0; j < CycleList[i].Count; j++)
				{
					if (CycleList[i][j].type == 'X' && !Xvalues.Contains(new string(CycleList[i][j].arr)))
					{
						Xvalues.Add(new string (CycleList[i][j].arr));
						CycleList[i].Remove(CycleList[i][j]);
						j--;
						if (CycleList[i].Count == 0)
						{
							CycleList.Remove(CycleList[i]);
						}
					}
					else
					{
						CycleList[i][j] = new RowInSection
						{
							arr = CycleList[i][j].arr,
							numbers = CycleList[i][j].numbers,
							type = 'X'
						};
					}
					if (CycleList.Count == 0) return;
				}
			}
		}
		public void SetFromImplicants(Dictionary<int, string> implicants)
		{
			int numOfElems = implicants.Count, section = 0, pos = 0, secpos = 0;
			while (numOfElems > 0)
			{
				pos = 0;
				foreach (int implicant in implicants.Keys)
				{
					if (implicants[implicant].Count(c => c == '1') == section)
					{
						if (pos == 0)
						{
							CycleList.Add(new List<RowInSection>());
							secpos++;
						}
						CycleList.ElementAt(secpos - 1).Add(new RowInSection
						{
							arr = implicants[implicant].ToCharArray(),
							numbers = implicant.ToString(),
							type = 'X'
						});
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
			int iterations = CycleList.Count - 1;
			for (int i = 0; i < iterations; i++)
			{
				NextCycleList.Add(new List<RowInSection>());
				for (int j = 0; j < CycleList[i].Count; j++)
				{
					for (int k = 0; k < CycleList[i + 1].Count; k++)
					{
						char[] tempArr = new char[CycleList[i][j].arr.Length];
						if (CheckDifference(CycleList[i][j].arr, CycleList[i + 1][k].arr, ref tempArr) == 1)
						{
							ok = true;
							CycleList[i][j] = new RowInSection
							{
								arr = CycleList[i][j].arr,
								numbers = CycleList[i][j].numbers,
								type = 'V'
							};
							CycleList[i + 1][k] = new RowInSection
							{
								arr = CycleList[i + 1][k].arr,
								numbers = CycleList[i + 1][k].numbers,
								type = 'V'
							};
							NextCycleList.ElementAt(i).Add(new RowInSection
							{
								arr = tempArr,
								numbers = CycleList[i][j].numbers + "," + CycleList[i + 1][k].numbers,
								type = 'X'
							});
						}
					}
				}
			}
			AddXValues();
			LastCycle = !ok;
			CycleList = NextCycleList;
			NextCycleList = new List<List<RowInSection>>();
		}
		public string GetInString()
		{
			return string.Join('\n', Xvalues);
		}
	}
}
