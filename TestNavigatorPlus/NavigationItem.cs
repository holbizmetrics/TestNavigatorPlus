using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Shell.Interop;

namespace TestNavigatorPlus
{
	// Define a simple class to hold information about bugs, tests, and errors
	public class NavigationItem
	{
		public string Name { get; set; }
		public string FilePath
		{
			get; set; // Path to the file where the item is located
		}

		public int LineNumber
		{
			get; set; // Line number in the file
		}

		public IVsTaskItem TaskItem
		{
			get; set; // TaskItem associated with the item
		}
	}
}
