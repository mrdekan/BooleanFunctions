﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanFunctions
{
    internal class BooleanFunction
    {
        #region Variables
        private List<string> variables = new List<string>();
        private const int ASCII_LETTERS_START = 65;
        #endregion
        #region Calculation
        public string Calculate(string value, Dictionary<string, int> arguments)
        {
            variables.Clear();
            foreach (var variable in arguments.Keys)
            {
                variables.Add(variable.ToString());
            }
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
        private string cutHooks(string value, int pos)
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
        //test from presentation
        //!a!b!c!d+!a!b!cd+!a!bc!d+!a!bcd+!ab!cd+!abcd+a!b!c!d+a!bc!d+ab!c!d+a!bcd+ab!cd
        public string GetMDNF(Dictionary<int, string> implicants)
        {
            #region Calculating cycles
            Cycle cycle = new Cycle();
            cycle.SetFromImplicants(implicants);
            while (!cycle.LastCycle)
                cycle.ToNextCycle();
            List<string> core = cycle.GetCore();
            bool[,] matrix = cycle.GetMatrix();
            #endregion
            #region Finding indexes of core
            List<int> coreIndexes = new List<int>();
            List<int> coreCover = new List<int>();
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
                List<List<string>> BeforeMultiplying = new List<List<string>>();
                List<List<string>> AfterMultiplying = new List<List<string>>();
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
                //char[] min = BeforeMultiplying[0].OrderBy(s => s.Length).FirstOrDefault().ToCharArray();
                for (int i = 0; i < min.Length; i++)
                    core.Add(cycle.Xvalues[(int)min[i] - ASCII_LETTERS_START]);
            }
            List<string> MDNF = new List<string>();
            foreach(var c in core)
            {
                char[] temp = c.ToCharArray();
                StringBuilder tempRes = new StringBuilder();
                for(int i = 0; i < temp.Length; i++)
                    tempRes.Append(temp[i] == '-' ? "" : (temp[i] == '1' ? variables[i] : "!" + variables[i]));
                MDNF.Add(tempRes.ToString());
            }
            #endregion
            return String.Join('+',MDNF);
        }
    }
}