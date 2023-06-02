using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanFunctions
{
    static class Results
    {
        public static string PDNF { get; set; } = "";
        public static string PCNF { get; set; } = "";
        public static string MDNF { get; set; } = "";
        public static string Zhegalkin { get; set; } = "";
        public static bool StoreOne { get; set; }
        public static bool StoreZero { get; set; }
        public static bool SelfDual { get; set; }
        public static bool Monotone { get; set; }
        public static string Elapsed { get; set; } = "";
        public static string GetString()
        {
            StringBuilder res = new StringBuilder();
            if (PDNF != "") res.Append("PDNF: " + PDNF + "\r\n");
            if (PCNF != "") res.Append("PCNF: " + PCNF + "\r\n");
            res.Append("MDNF: " + MDNF + "\r\n");
            res.Append("Zhegalkin polynomial: " + Zhegalkin + "\r\n");
            res.Append((StoreOne ? "The function stores 1." : "The function does not store 1.") + "\r\n");
            res.Append((StoreZero ? "The function stores 0." : "The function does not store 0.") + "\r\n");
            res.Append((SelfDual ? "The function is self-dual." : "The function is not self-dual.") + "\r\n");
            res.Append((Monotone ? "The function is monotone." : "The function is not monotone.") + "\r\n");
            res.Append(Elapsed + "\r\n");
            return res.ToString();
        }
        public static void ClearFields()
        {
            PDNF = "";
            PCNF = "";
            MDNF = "";
            Zhegalkin = "";
            StoreOne = false;
            StoreZero = false;
            SelfDual = false;
            Monotone = false;
            Elapsed = "";
        }
    }
}
