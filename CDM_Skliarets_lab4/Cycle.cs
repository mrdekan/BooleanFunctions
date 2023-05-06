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
		//List<List<RowInSection>> NextCycleList = new List<List<RowInSection>>();
		public Cycle GetNext()
		{
			Cycle NextCycle = new Cycle();

			return NextCycle;
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
						CycleList.ElementAt(secpos-1).Add(new RowInSection
						{
							arr = implicants[implicant].ToCharArray(),
							numbers = implicant.ToString(),
							type = 'V'
						});
						numOfElems--;
						pos++;
					}
				}
				section++;
			}
		}
	}
}
