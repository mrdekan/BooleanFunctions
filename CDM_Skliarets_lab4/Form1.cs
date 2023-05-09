using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BooleanFunctions
{
	public partial class Form1 : Form
	{
		private const string ACTIONS = "∨∧*+|&";
		private const int MAX_RAND_ACTIONS = 10;
		private const int MAX_RAND_ARGS = 6;
		private const int MIN_RAND_ARGS = 3;
		private BooleanFunction booleanFunction = new BooleanFunction();
		public Form1()
		{
			InitializeComponent();
			table.RowCount = 0;
			info.UseMnemonic = false;
		}
		//heavy test b∧(!d∧a∨d)∧a∧b∨f∨d∧t∧(g∨r)∨!e∨!o∧(f∨s∧!d)∨g∧c∨!c∧d∨z∨x∨y∧(!z∨!x)∨u
		private void submit_Click(object sender, EventArgs e)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<string> variables = Variables();
			List<List<string>> binTable = binaryTable(variables.Count - 1);
			Dictionary<string, int> arguments = new Dictionary<string, int>();
			Dictionary<int, string> implicants = new Dictionary<int, string>();
			string strInput = ToCorrectInput();
			input.Text = strInput.Replace('+', '∨').Replace('*', '∧');
			table.Rows.Clear();
			table.Columns.Clear();
			table.Refresh();
			PrintToTable(variables.ToArray());
			string[] arr = new string[variables.Count()];
			for (int i = 0; i < binTable.ElementAt(0).Count; i++)
			{
				arguments.Clear();
				for (int j = 0; j < variables.Count - 1; j++)
				{
					arr[j] = binTable.ElementAt(binTable.Count - j - 1).ElementAt(i);
					arguments.Add(variables.ElementAt(j), int.Parse(arr[j]));
				}
				string res = booleanFunction.Calculate(strInput, arguments);
				if (res == "1")
				{
					string impl = "";
					for(int j = 0; j < arr.Length - 1; j++) impl += arr[j];
					implicants.Add(i, impl);
				}
				arr[arr.Length - 1] = res;
				PrintToTable(arr);
			}
			string core = booleanFunction.GetMDNF(implicants);
			//debug_label.Text += "\n" + cycla.GetInString();
			stopwatch.Stop();
			debug_label.Text += "\n" + core + "\n" + stopwatch.Elapsed;
		}
		Func<char, bool> Letter = c => ((int)c >= 97 && (int)c <= 122);
		private List<string> Variables()
		{
			List<string> letters = new List<string>();
			foreach (char c in ParseInputString()) if (Letter(c) && !letters.Contains(c.ToString())) letters.Add(c.ToString());
			letters.Sort();
			letters.Add("f()");
			return letters;
		}
		public void PrintToTable(string[] arr)
		{
			int columns = 0;
			while (table.Columns.Count < arr.Length)
			{
				table.Columns.Add("", "");
				table.Columns[columns].Width = 24;
				columns++;
			}
			table.Rows.Add(arr);
		}
		#region Working with input
		private string ToCorrectInput()
		{
			string strInput = ParseInputString()
				.Replace('∧', '*')
				.Replace('∨', '+')
				.Replace('&', '*')
				.Replace('|', '+');
			int[] addHooks = CheckHooks(strInput).Split('_').Select(num => Convert.ToInt32(num)).ToArray();
			while (addHooks[0] > 0)
			{
				strInput = "(" + strInput;
				addHooks[0]--;
			}
			while (addHooks[1] > 0)
			{
				strInput += ")";
				addHooks[1]--;
			}
			while (strInput.Contains("()")) strInput = strInput.Replace("()", "");
			for (int i = 0; i < strInput.Length - 1; i++)
			{
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
		private string CheckHooks(string s)
		{
			int before = 0, hooks = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '(') hooks++;
				else if (s[i] == ')' && hooks <= 0) before++;
				else if (s[i] == ')') hooks--;
			}
			return before.ToString() + "_" + hooks.ToString();
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
			List<List<string>> res = new List<List<string>>();
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
			StringBuilder str = new StringBuilder();
			Random random = new Random();
			char[] arr = new char[random.Next(MIN_RAND_ARGS, MAX_RAND_ARGS)];
			char letter = 'a';
			string actions = "∨∧";
			int hooks = 0;
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = letter;
				letter++;
			}
			if (random.Next(4) == 0) str.Append('!');
			str.Append(arr[random.Next(arr.Length)]);
			int actionsNumber = random.Next(2, MAX_RAND_ACTIONS);
			for (int i = 0; i < actionsNumber; i++)
			{
				bool hookIsAvailable = true;
				str.Append(actions[random.Next(2)]);
				if (random.Next(3) == 0 && i != actionsNumber - 1)
				{
					str.Append('(');
					hooks++;
					hookIsAvailable = false;
				}
				if (random.Next(4) == 0) str.Append('!');
				str.Append(arr[random.Next(arr.Length)]);
				if (hooks > 0 && random.Next(2) == 0 && hookIsAvailable)
				{
					str.Append(')');
					hooks--;
				}
			}
			while (hooks > 0)
			{
				str.Append(')');
				hooks--;
			}
			input.Text = str.ToString();
		}
	}
}