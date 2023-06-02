using System.Text;

namespace BooleanFunctions
{
    internal class BooleanFunction
    {
        #region Variables
        private List<string> variables = new List<string>();
        private const int ASCII_LETTERS_START = 65;
        #endregion
        #region Main calculation
        public string Calculate(string value, Dictionary<string, int> arguments)
        {
            variables.Clear();
            foreach (var variable in arguments.Keys)
                variables.Add(variable.ToString());
            while (value.Contains('('))
            {
                int pos = value.IndexOf('(');
                string temp = cutTemples(value, pos);
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
        private string cutTemples(string value, int pos)
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
		#endregion
		public string GetMDNF(Dictionary<int, string> implicants)
        {
            #region Calculating cycles
            Cycle cycle = new();
            cycle.SetFromImplicants(implicants);
            while (!cycle.LastCycle)
                cycle.ToNextCycle();
            List<string> core = cycle.GetCore();
            bool[,] matrix = cycle.GetMatrix();
            #endregion
            #region Finding indexes of core
            List<int> coreIndexes = new();
            List<int> coreCover = new();
            foreach (string index in core)
            {
                int temp = cycle.Xvalues.IndexOf(index);
                if (!coreIndexes.Contains(temp))
                {
                    coreIndexes.Add(temp);
                    for (int i = 0; i < cycle.Implicants.Count; i++)
                        if (matrix[i, temp] && !coreCover.Contains(i)) coreCover.Add(i);
                }
            }
            #endregion
            #region Preparing matrix for Petrick's method
            bool[,] newMatrix = new bool[cycle.Implicants.Count - coreCover.Count, cycle.Xvalues.Count - coreIndexes.Count];
            for (int y = 0, posY = 0; y < matrix.GetLength(1); y++)
            {
                if (coreIndexes.Contains(y)) continue;
                for (int x = 0, posX = 0; x < matrix.GetLength(0); x++)
                {
                    if (!coreCover.Contains(x))
                    {
                        newMatrix[posX, posY] = matrix[x, y];
                        posX++;
                    }
                }
                posY++;
            }
            #endregion
            #region Petrick's method
            if (newMatrix.GetLength(0) != 0)
            {
                List<List<string>> BeforeMultiplying = new();
                List<List<string>> AfterMultiplying = new();
                for (int x = 0; x < newMatrix.GetLength(0); x++)
                {
                    BeforeMultiplying.Add(new List<string>());
                    for (int y = 0; y < newMatrix.GetLength(1); y++)
                        if (newMatrix[x, y]) BeforeMultiplying[x].Add(((char)(y + ASCII_LETTERS_START)).ToString());
                }
                while (BeforeMultiplying.Count != 1)
                {
                    for (int i = 0, pos = 0; i < BeforeMultiplying.Count; i += 2, pos++)
                    {
                        AfterMultiplying.Add(new List<string>());
                        for (int j = 0; j < BeforeMultiplying[i].Count; j++)
                            for (int k = 0; k < BeforeMultiplying[i + 1].Count; k++)
                                AfterMultiplying[pos].Add(BeforeMultiplying[i][j].ToString() + (BeforeMultiplying[i + 1][k] == BeforeMultiplying[i][j] ? "" : BeforeMultiplying[i + 1][k]));
                    }
                    if (BeforeMultiplying.Count % 2 == 1)
                    {
                        AfterMultiplying.Add(new List<string>());
                        foreach (var str in BeforeMultiplying[BeforeMultiplying.Count - 1])
                            AfterMultiplying[AfterMultiplying.Count - 1].Add(str);
                    }
                    BeforeMultiplying = AfterMultiplying;
                    AfterMultiplying = new List<List<string>>();
                }
                char[] min = BeforeMultiplying[0][0].ToCharArray();
                foreach (string s in BeforeMultiplying[0])
                    if (s.Length < min.Length) min = s.ToCharArray();
                for (int i = 0; i < min.Length; i++)
                    core.Add(cycle.Xvalues[min[i] - ASCII_LETTERS_START]);
            }
            List<string> MDNF = new();
            foreach (var c in core)
            {
                char[] temp = c.ToCharArray();
                StringBuilder tempRes = new StringBuilder();
                for (int i = 0; i < temp.Length; i++)
                    tempRes.Append(temp[i] == '-' ? "" : (temp[i] == '1' ? variables[i] : "!" + variables[i]));
                MDNF.Add(tempRes.ToString());
            }
            #endregion
            return String.Join('+', MDNF);
        }
        public bool IsSelfDual(List<int> results)
        {
            int[] dual = new int[results.Count];
			results.CopyTo(dual);
            for (int i = 0; i < dual.Length; i++) dual[i] = 1 - dual[i];
            Array.Reverse(dual);
            return results.SequenceEqual(dual);
        }
        public string GetZhegalkinPolynomial(List<int> results, List<string> variables, List<List<string>> binTable)
        {
            List<int> temp = new();
			List<int> leftElemsInTriangle = new List<int>{results[0]};
			while (results.Count > 1)
            {
                for (int i = 0; i < results.Count - 1; i++)
                    temp.Add((results[i] + results[i + 1]) % 2);
				results = temp;
                temp = new List<int>();
                leftElemsInTriangle.Add(results[0]);
            }
            List<string> res = new();
            for (int i = 0; i < leftElemsInTriangle.Count; i++)
            {
                if (leftElemsInTriangle[i] != 0)
                {
                    StringBuilder tempStr = new();
                    for (int j = binTable.Count - 1; j >= 0; j--)
                        if (binTable[j][i] != "0") tempStr.Append(variables[binTable.Count - j - 1]);
                    if (tempStr.Length == 0) tempStr.Append('1'); //for 000..0 iteration
                    res.Add(tempStr.ToString());
                }
            }
            return String.Join('⨁', res);
        }
        private int[] getImplicantsSet(List<List<string>> binTable, int pos)
        {
            int[] res = new int[binTable.Count];
            for (int i = 0; i < res.Length; i++) res[i] = int.Parse(binTable[i][pos]);
            return res;
        }
        private bool isLessThan(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++) if (a[i] > b[i]) return false;
            return true;
        }
        public bool IsMonotone(List<int> results, List<List<string>> binTable)
        {
            for (int i = 0; i < results.Count-1; i++)
                for (int j = i + 1; j < results.Count; j++)
                    if (results[i] > results[j] && isLessThan(getImplicantsSet(binTable, i), getImplicantsSet(binTable, j))) return false;
            return true;
        }
    }
}
