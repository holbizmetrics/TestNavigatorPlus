using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestNavigatorPlus.Navigation
{
	public class Navigator
	{
		private List<NavigationItem> bugs = null;
		private List<NavigationItem> tests = null;
		private List<NavigationItem> errors = null;

		// Current indices for navigation
		private int currentBugIndex = -1;
		private int currentTestIndex = -1;
		private int currentErrorIndex = -1;

		DTE2 dte = null;
		EventManager eventsManager = null;
		NavigationHelper navigationHelper = null;
		public Navigator(DTE2 dte, IServiceProvider serviceProvider)
		{ 
			this.dte = dte;
			navigationHelper = new NavigationHelper(dte, serviceProvider);
			
			tests = navigationHelper.GetTestsInSolution();
			errors = navigationHelper.GetCompilerErrors();
			bugs = navigationHelper.GetBugsInSolution();

			eventsManager = new EventManager(dte, navigationHelper);
		}

		// Navigation methods for bugs
		public void NavigateToFirstBug(object sender, EventArgs e)
		{
			Debug.WriteLine("NavigateToFirstBug");
			if (bugs?.Count > 0)
			{
				currentBugIndex = 0;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToNextBug(object sender, EventArgs e)
		{
			Debug.WriteLine("NavigateToNextBug");
			if (currentBugIndex < bugs?.Count - 1)
			{
				currentBugIndex++;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToPreviousBug(object sender, EventArgs e)
		{
			Debug.WriteLine("NavigateToPreviousBug");
			if (currentBugIndex > 0)
			{
				currentBugIndex--;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToLastBug(object sender, EventArgs e)
		{
			Debug.WriteLine("NavigateToLastBug");
			if (bugs?.Count > 0)
			{
				currentBugIndex = bugs.Count - 1;
				OpenItem(bugs[currentBugIndex]);
			}
		}

		// Similar methods can be created for tests and errors
		// Navigation methods for tests
		public void NavigateToFirstTest(object sender, EventArgs e)
		{
			if (tests?.Count > 0)
			{
				currentTestIndex = 0;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToNextTest(object sender, EventArgs e)
		{
			if (currentTestIndex < tests?.Count - 1)
			{
				currentTestIndex++;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToPreviousTest(object sender, EventArgs e)
		{
			if (currentTestIndex > 0)
			{
				currentTestIndex--;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToLastTest(object sender, EventArgs e)
		{
			if (tests?.Count > 0)
			{
				currentTestIndex = tests.Count - 1;
				OpenItem(tests[currentTestIndex]);
			}
		}

		// Navigation methods for errors
		public void NavigateToFirstError(object sender, EventArgs e)
		{
			if (errors?.Count > 0)
			{
				currentErrorIndex = 0;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToNextError(object sender, EventArgs e)
		{
			if (currentErrorIndex < errors?.Count - 1)
			{
				currentErrorIndex++;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToPreviousError(object sender, EventArgs e)
		{
			if (currentErrorIndex > 0)
			{
				currentErrorIndex--;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToLastError(object sender, EventArgs e)
		{
			if (errors?.Count > 0)
			{
				currentErrorIndex = errors.Count - 1;
				OpenItem(errors[currentErrorIndex]);
			}
		}

		/// <summary>
		/// Method to open the item in the editor
		/// </summary>
		/// <param name="item"></param>
		private void OpenItem(NavigationItem item)
		{
			// Open the document
			var document = dte.Documents.Open(item.FilePath);

			// Ensure the document is active
			document.Activate();

			// Get the active document's selection
			var selection = (TextSelection)dte.ActiveDocument.Selection;

			// Move to the specified line number and column (1-based index)
			selection.MoveToLineAndOffset(item.LineNumber, 1);
		}

		public void RefreshCompilerErrors()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			errors = navigationHelper.GetCompilerErrors(); // This is your existing method to fetch errors
			currentErrorIndex = -1; // Reset the index
		}

		public void SubscribeToEvents()
		{
			eventsManager.SubscribeToBuildEvents();
		}

		public void UnsubscribeFromEvents()
		{
			eventsManager.UnsubscribeFromEvents();
		}
	}
}
