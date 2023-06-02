using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace BooleanFunctions
{
    public partial class Form1 : Form
    {
        #region Constants
        private const string ACTIONS = "∨∧*+|&";
        private const int MAX_RAND_ACTIONS = 15;
        private const int MAX_RAND_ARGS = 7;
        private const int MIN_RAND_ARGS = 3;
        private const int PROBABILITY_OF_OPENING_TEMPLE = 3;
        private const int PROBABILITY_OF_DENIAL = 4;
        private const int PROBABILITY_OF_CLOSING_TEMPLE = 2;
        private const int ASCII_LETTERS_START = 97;
        #endregion
        private BooleanFunction booleanFunction = new BooleanFunction();
        public Form1()
        {
            InitializeComponent();
            table.RowCount = 0;
            info.UseMnemonic = false;
            other_results.ReadOnly = true;
        }
        //heavy test
        //b∧(!d∧a∨d)∧a∧b∨f∨d∧t∧(g∨r)∨!e∨!o∧(f∨s∧!d)∨g∧c∨!c∧d∨z∨x∨y∧(!z∨!x)∨u
        //test from presentation
        //!x!y!z!t+!x!y!zt+!x!yz!t+!x!yzt+!xy!zt+!xyzt+x!y!z!t+x!yz!t+xy!z!t+x!yzt+xy!zt
        private void submit_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            List<string> variables = Variables(ParseInputString());
            List<List<string>> binTable = binaryTable(variables.Count - 1);
            Dictionary<string, int> arguments = new();
            Dictionary<int, string> implicants = new();
            string strInput = ToCorrectInput();
            if (strInput == "error") return;
            input.Text = strInput.Replace('+', '∨').Replace('*', '∧');
            table.Rows.Clear();
            table.Columns.Clear();
            table.Refresh();
            List<string[]> fullTable = new();
            fullTable.Add(variables.ToArray());
            string[] arr = new string[variables.Count()];
            List<int> results = new();
            List<string> pdnf = new();
            List<string> pcnf = new();
            for (int i = 0; i < binTable.ElementAt(0).Count; i++)
            {
                arguments.Clear();
                for (int j = 0; j < variables.Count - 1; j++)
                {
                    arr[j] = binTable.ElementAt(binTable.Count - j - 1).ElementAt(i);
                    arguments.Add(variables.ElementAt(j), int.Parse(arr[j]));
                }
                string res = booleanFunction.Calculate(strInput, arguments);
                results.Add(int.Parse(res));
                if (res == "1")
                {
                    StringBuilder impl = new();
                    for (int j = 0; j < arr.Length - 1; j++) impl.Append(arr[j]);
                    implicants.Add(i, impl.ToString());
                    pdnf.Add(ImplicantsToString(arguments, 0));
                }
                else
                    pcnf.Add(ImplicantsToString(arguments, 1));
                arr[arr.Length - 1] = res;
                string[] arrCopy = new string[arr.Length];
                arr.CopyTo(arrCopy, 0);
                fullTable.Add(arrCopy);
            }
            PrintTable(fullTable);
            other_results.Text = "";
            Results.ClearFields();
            variables.RemoveAt(variables.Count - 1);
            if (pdnf.Count != 0) Results.PDNF = $"f({String.Join(',', variables)})={String.Join('+', pdnf)}";
            if (pcnf.Count != 0) Results.PCNF = $"f({String.Join(',', variables)})={String.Join("", pcnf)}";
            FindMDNF(implicants, results);
            GetZhegalkin(results, variables, binTable);
            Results.StoreOne = results[results.Count - 1] == 1;
            Results.StoreZero = results[0] == 0;
            Results.SelfDual = !results.Contains(0) || !results.Contains(1) || !booleanFunction.IsSelfDual(results);
            Results.Monotone = !results.Contains(0) || !results.Contains(1) || booleanFunction.IsMonotone(results, binTable);
            stopwatch.Stop();
            Results.Elapsed = $"Done in {stopwatch.Elapsed}";
            other_results.Text = Results.GetString();
        }
        async void FindMDNF(Dictionary<int, string> implicants, List<int> results)
        {
            await Task.Run(() =>
            {
                if (!results.Contains(0)) Results.MDNF = "1";
                else if (!results.Contains(1)) Results.MDNF = "doesn't exist";
                else Results.MDNF = booleanFunction.GetMDNF(implicants);
                Invoke((MethodInvoker)(() =>
                {
                    other_results.Text = Results.GetString();
                }));
            });
        }
        async void GetZhegalkin(List<int> results, List<string> variables, List<List<string>> binTable)
        {
            await Task.Run(() =>
            {
                if (!results.Contains(0)) Results.Zhegalkin = "1";
                else if (!results.Contains(1)) Results.Zhegalkin = "0";
                else Results.Zhegalkin = booleanFunction.GetZhegalkinPolynomial(results, variables, binTable);
                Invoke((MethodInvoker)(() =>
                {
                    other_results.Text = Results.GetString();
                }));
            });
        }
        Func<char, bool> Letter = c => (c >= 97 && c <= 122);
        private string ImplicantsToString(Dictionary<string, int> arguments, int mode)
        {
            StringBuilder res = new();
            for (int i = 0; i < arguments.Keys.Count; i++)
                res.Append(((i == 0 || mode != 1) ? "" : "+") + (arguments.ElementAt(i).Value == mode ? $"!{arguments.Keys.ElementAt(i)}" : arguments.Keys.ElementAt(i)));
            if (mode == 1) return $"({res})";
            return res.ToString();
        }
        private List<string> Variables(string str)
        {
            List<string> letters = new();
            foreach (char c in str) if (Letter(c) && !letters.Contains(c.ToString())) letters.Add(c.ToString());
            letters.Sort();
            letters.Add("f()");
            return letters;
        }
        async public void PrintTable(List<string[]> arr)
        {
            await Task.Run(() =>
            {
                foreach (string[] arr2 in arr)
                {
                    for (int columns = 0; table.Columns.Count < arr2.Length; columns++)
                    {
                        Invoke((MethodInvoker)(() =>
                        {
                            table.Columns.Add("", "");
                            table.Columns[columns].Width = 27;
                        }));
                    }
                    Invoke((MethodInvoker)(() =>
                    {
                        table.Rows.Add(arr2);
                    }));
                }
            });
        }
        #region Working with input
        private string ToCorrectInput()
        {
            string strInput = ParseInputString()
                .Replace('∧', '*')
                .Replace('∨', '+')
                .Replace('&', '*')
                .Replace('|', '+');
            int templesBefore = 0, templesAfter = 0;
            CheckTemples(strInput, ref templesBefore, ref templesAfter);
            for (; templesBefore > 0; templesBefore--)
                strInput = "(" + strInput;
            for (; templesAfter > 0; templesAfter--)
                strInput += ")";
            while (strInput.Contains("()")) strInput = strInput.Replace("()", "");
            for (int i = 0; i < strInput.Length - 1; i++)
            {
                if (strInput[i] == '(' && ACTIONS.Contains(strInput[i + 1]) || ACTIONS.Contains(strInput[i]) && strInput[i + 1] == ')' || strInput[i] == '!' && ACTIONS.Contains(strInput[i + 1]))
                {
                    error_label.Text = "Incorrect value";
                    submit.Enabled = false;
                    return "error";
                }
                if (ACTIONS.Contains(strInput[i]) && ACTIONS.Contains(strInput[i + 1]))
                {
                    if (strInput[i] == strInput[i + 1])
                    {
                        strInput = strInput.Remove(i, 1);
                        i--;
                    }
                    else
                    {
                        error_label.Text = "Incorrect value";
                        submit.Enabled = false;
                    }
                }
            }
            for (int i = 0; i < strInput.Length - 1; i++)
                if (Letter(strInput[i]) && Letter(strInput[i + 1])
                    || Letter(strInput[i]) && strInput[i + 1] == '('
                    || Letter(strInput[i + 1]) && strInput[i] == ')'
                    || strInput[i] == ')' && strInput[i + 1] == '('
                    || Letter(strInput[i]) && strInput[i + 1] == '!'
                    || strInput[i] == ')' && strInput[i + 1] == '!')
                    strInput = strInput.Insert(i + 1, "*");
            while (ACTIONS.Contains(strInput[0])) strInput = strInput.Remove(0, 1);
            while (ACTIONS.Contains(strInput[strInput.Length - 1]) || strInput.EndsWith('!')) strInput = strInput.Remove(strInput.Length - 1, 1);
            return strInput;
        }
        private string ParseInputString()
        {
            return input.Text.Replace("()", "").Trim(' ').ToLower();
        }
        private void CheckTemples(string s, ref int before, ref int after)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(') after++;
                else if (s[i] == ')' && after <= 0) before++;
                else if (s[i] == ')') after--;
            }
        }
        private void input_TextChanged(object sender, EventArgs e)
        {
            error_label.Text = "";
            submit.Enabled = true;
            string s = ParseInputString();
            if (!Regex.IsMatch(s, "^[a-z!+*∨∧|&()]+$") && s != "")
            {
                error_label.Text = "Incorrect value";
                submit.Enabled = false;
            }
            else if (s == "")
            {
                error_label.Text = "The field cannot be empty";
                submit.Enabled = false;
            }
        }
        private void conjunction_Click(object sender, EventArgs e)
        {
            int temp = input.SelectionStart;
            input.Text = input.Text.Insert(temp, "∧");
            input.Focus();
            input.SelectionStart = temp + 1;
            input.SelectionLength = 0;
        }
        private void disjunction_Click(object sender, EventArgs e)
        {
            int temp = input.SelectionStart;
            input.Text = input.Text.Insert(temp, "∨");
            input.Focus();
            input.SelectionStart = temp + 1;
            input.SelectionLength = 0;
        }
        private void table_SelectionChanged(object sender, EventArgs e)
        {
            table.ClearSelection();
        }
        #endregion
        private List<List<string>> binaryTable(int variables)
        {
            List<List<string>> res = new();
            for (int i = 0; i < variables; i++) //n змінних = n списки в списку
                                                //(один вкладений список це стовпець з 0 та 1 під змінною в таблиці)
                                                //і перший список це стовпець під останньою змінною
            {
                res.Add(new List<string>()); //Додаємо список
                for (int j = 0; j < Math.Pow(2, variables) / Math.Pow(2, i); j++)
                    //Math.Pow(2, variables) / Math.Pow(2, i) == кількість елементів
                    //у списку поділена на кількість однакових символів поруч
                    for (int k = 0; k < Math.Pow(2, i); k++)
                        //Кількість однакових символів (0 / 1) поспіль на даній ітерації
                        res.ElementAt(i).Add((j % 2).ToString());
                //0 чи 1 в залежності від парне чи непарне j
            }
            return res;
        }
        private void random_Click(object sender, EventArgs e)
        {
            StringBuilder str = new();
            Random random = new();
            char[] arr = new char[random.Next(MIN_RAND_ARGS, MAX_RAND_ARGS)];
            string actions = "∨∧";
            int hooks = 0;
            for (int i = 0; i < arr.Length; i++) arr[i] = (char)(i + ASCII_LETTERS_START);
            if (random.Next(PROBABILITY_OF_DENIAL) == 0) str.Append('!');
            str.Append(arr[random.Next(arr.Length)]);
            int actionsNumber = random.Next(2, MAX_RAND_ACTIONS);
            for (int i = 0; i < actionsNumber; i++)
            {
                bool hookIsAvailable = true;
                str.Append(actions[random.Next(actions.Length)]);
                if (random.Next(PROBABILITY_OF_OPENING_TEMPLE) == 0 && i != actionsNumber - 1)
                {
                    str.Append('(');
                    hooks++;
                    hookIsAvailable = false;
                }
                if (random.Next(PROBABILITY_OF_DENIAL) == 0) str.Append('!');
                str.Append(arr[random.Next(arr.Length)]);
                if (hooks > 0 && random.Next(PROBABILITY_OF_CLOSING_TEMPLE) == 0 && hookIsAvailable)
                {
                    str.Append(')');
                    hooks--;
                }
            }
            for (; hooks > 0; hooks--)
                str.Append(')');
            input.Text = str.ToString();
        }
        private void input_binary_KeyDown(object sender, KeyEventArgs e) //Allows only 0, 1, backspace and delete in input_binary
        {
            e.SuppressKeyPress = !(e.KeyValue == 48 || e.KeyValue == 49 || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back);
        }
        private void input_binary_TextChanged(object sender, EventArgs e)
        {
            enter_binary.Enabled = input_binary.Text != "";
            int var_pow = 1;
            while (input_binary.Text.Length > Math.Pow(2, var_pow)) var_pow++;
            var_binary_label.Text = var_pow.ToString();
            variables_count_label.Text = var_pow == 1 ? "variable" : "variables";
        }
        private string getFromBinary()
        {
            StringBuilder str = new();
            str.Append(input_binary.Text);
            int length = (int)Math.Pow(2, int.Parse(var_binary_label.Text));
            while (str.Length < length)
                str.Append('0');
            input_binary.Text = str.ToString();
            char[] varArr = new char[int.Parse(var_binary_label.Text)];
            List<List<string>> binTable = binaryTable(varArr.Length);
            for (int i = 0; i < varArr.Length; i++)
                varArr[i] = (char)(i + ASCII_LETTERS_START);
            char[] implicants = str.ToString().ToCharArray();
            List<string> res = new();
            Dictionary<string, int> arguments = new();
            for (int i = 0; i < binTable.ElementAt(0).Count; i++)
            {
                if (implicants[i] == '0') continue;
                arguments.Clear();
                for (int j = 0; j < varArr.Length; j++)
                    arguments.Add(varArr[j].ToString(), int.Parse(binTable.ElementAt(binTable.Count - j - 1).ElementAt(i)));
                res.Add(ImplicantsToString(arguments, 0));
            }
            return String.Join('+', res);
        }
        private void enter_binary_Click(object sender, EventArgs e)
        {
            if (input_binary.Text != "") input.Text = getFromBinary();
        }
    }
}