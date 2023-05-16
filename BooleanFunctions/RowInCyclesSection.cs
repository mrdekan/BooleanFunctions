namespace BooleanFunctions
{
	internal class RowInCyclesSection
	{
		public string impl { get; set; }
		public char type { get; set; }
		public RowInCyclesSection(string impl)
		{
			this.impl = impl;
			type = 'X';
		}
	}
}
