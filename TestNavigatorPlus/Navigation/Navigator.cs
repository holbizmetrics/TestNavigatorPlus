using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TestNavigatorPlus.Detectors;
using StackFrame = System.Diagnostics.StackFrame;

namespace TestNavigatorPlus.Navigation
{
	public class Navigator
	{
		private IProjectEntityDetector testDetector = new TestDetector();
		private IProjectEntityDetector bugDetector = new BugDetector();
		private IProjectEntityDetector errorDetector = new ErrorDetector();
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
			this.dte = dte ?? throw new ArgumentNullException(nameof(dte));
			navigationHelper = new NavigationHelper(dte, serviceProvider);

			if (navigationHelper == null)
			{
				Debug.WriteLine("NavigationHelper is null");
				return;
			}

			tests = testDetector.Detect(navigationHelper).ToList();
			bugs = bugDetector.Detect(navigationHelper).ToList();
			errors = errorDetector.Detect(navigationHelper).ToList();

			eventsManager = new EventManager(dte, navigationHelper);
		}

		// Navigation methods for bugs
		public void NavigateToFirstBug(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (bugs?.Count > 0)
			{
				currentBugIndex = 0;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToNextBug(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (currentBugIndex < bugs?.Count - 1)
			{
				currentBugIndex++;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToPreviousBug(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (currentBugIndex > 0)
			{
				currentBugIndex--;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToLastBug(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
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
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (tests?.Count > 0)
			{
				currentTestIndex = 0;
				OpenItem(tests[currentTestIndex]);
			}
			else
			{
				tests = testDetector.Detect(navigationHelper).ToList();
			}
		}
		public void NavigateToNextTest(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (currentTestIndex < tests?.Count - 1)
			{
				currentTestIndex++;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToPreviousTest(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (currentTestIndex > 0)
			{
				currentTestIndex--;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToLastTest(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (tests?.Count > 0)
			{
				currentTestIndex = tests.Count - 1;
				OpenItem(tests[currentTestIndex]);
			}
		}

		// Navigation methods for errors
		public void NavigateToFirstError(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (errors?.Count > 0)
			{
				currentErrorIndex = 0;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToNextError(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (currentErrorIndex < errors?.Count - 1)
			{
				currentErrorIndex++;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToPreviousError(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
			if (currentErrorIndex > 0)
			{
				currentErrorIndex--;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToLastError(object sender, EventArgs e)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());
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
			Debug.WriteLine("OpenItem");
			try
			{
				var document = dte.Documents.Open(item.FilePath);
				document.Activate();
				var selection = (TextSelection)dte.ActiveDocument.Selection;
				selection.MoveToLineAndOffset(item.LineNumber, 1, false);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error opening item: {ex.Message}");
			}
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
