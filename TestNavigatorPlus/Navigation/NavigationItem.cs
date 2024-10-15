using Microsoft.VisualStudio.Shell.Interop;

namespace TestNavigatorPlus.Navigation
{
	// Define a simple class to hold information about bugs, tests, and errors
	public enum NavigationItemType { Bug, Test, Error }

	public class NavigationItem : IEquatable<NavigationItem>
	{
		public string Name { get; set; }
		public string FilePath { get; set; }
		public int LineNumber { get; set; }
		public int ColumnNumber { get; set; }
		public string Description { get; set; }
		public string Project { get; set; }
		public NavigationItemType Type { get; set; }
		public IVsTaskItem TaskItem { get; set; }

		// Additional properties based on type
		public string TestMethodName { get; set; }
		public string BugSeverity { get; set; }

		public bool Equals(NavigationItem other)
		{
			if (other == null) return false;
			return this.FilePath == other.FilePath &&
				   this.LineNumber == other.LineNumber &&
				   this.ColumnNumber == other.ColumnNumber &&
				   this.Type == other.Type;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				hash = hash * 23 + (FilePath?.GetHashCode() ?? 0);
				hash = hash * 23 + LineNumber.GetHashCode();
				hash = hash * 23 + ColumnNumber.GetHashCode();
				hash = hash * 23 + Type.GetHashCode();
				return hash;
			}
		}
	}
}

