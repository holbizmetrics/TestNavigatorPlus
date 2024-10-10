using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;
using Debugger = System.Diagnostics.Debugger;
namespace TestNavigatorPlus
{
	internal sealed class TestNavigatorCommand
	{
		private readonly Package package;
		private static DTE2 _dte;
		public static TestNavigatorCommand Instance { get; private set; }
		private IServiceProvider ServiceProvider => package;

		private List<NavigationItem> bugs = null;
		private List<NavigationItem> tests = null;
		private List<NavigationItem> errors = null;

		private NavigationHelper navigationHelper = null;
		private TestNavigatorCommand(Package package)
		{
			Debugger.Break();
			Debug.WriteLine("");
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			_dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

			navigationHelper = new NavigationHelper(_dte, ServiceProvider);
		}
		public static async Task InitializeAsync(AsyncPackage package)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			Debug.WriteLine("Initializing the extension...");

			Instance = new TestNavigatorCommand(package);
			Instance.Initialize();
		}
		private void Initialize()
		{
			Debug.WriteLine("Initializing command...");
			var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				var menuCommandID = new CommandID(new Guid("{12345678-1234-1234-1234-123456789012}"), 0x0100);
				var menuItem = new MenuCommand(this.ExecuteCommand, menuCommandID);
				commandService.AddCommand(menuItem);
				Debug.WriteLine("Menu item added...");
			}
			else
			{
				Debug.WriteLine("Command service not found");
			}
		}

		private void ExecuteCommand(object sender, EventArgs e)
		{
			System.Diagnostics.Debugger.Break();
			ThreadHelper.ThrowIfNotOnUIThread();

			// Implement command logic here
			navigationHelper.AnalyzeSourceAndTestFiles();
		}

		// Current indices for navigation
		private int currentBugIndex = -1;
		private int currentTestIndex = -1;
		private int currentErrorIndex = -1;


		// Navigation methods for bugs
		public void NavigateToFirstBug()
		{
			Debug.WriteLine("NavigateToFirstBug");
			if (bugs.Count > 0)
			{
				currentBugIndex = 0;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToNextBug()
		{
			Debug.WriteLine("NavigateToNextBug");
			if (currentBugIndex < bugs.Count - 1)
			{
				currentBugIndex++;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToPreviousBug()
		{
			Debug.WriteLine("NavigateToPreviousBug");
			if (currentBugIndex > 0)
			{
				currentBugIndex--;
				OpenItem(bugs[currentBugIndex]);
			}
		}
		public void NavigateToLastBug()
		{
			Debug.WriteLine("NavigateToLastBug");
			if (bugs.Count > 0)
			{
				currentBugIndex = bugs.Count - 1;
				OpenItem(bugs[currentBugIndex]);
			}
		}

		// Similar methods can be created for tests and errors
		// Navigation methods for tests
		public void NavigateToFirstTest()
		{
			if (tests.Count > 0)
			{
				currentTestIndex = 0;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToNextTest()
		{
			if (currentTestIndex < tests.Count - 1)
			{
				currentTestIndex++;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToPreviousTest()
		{
			if (currentTestIndex > 0)
			{
				currentTestIndex--;
				OpenItem(tests[currentTestIndex]);
			}
		}
		public void NavigateToLastTest()
		{
			if (tests.Count > 0)
			{
				currentTestIndex = tests.Count - 1;
				OpenItem(tests[currentTestIndex]);
			}
		}

		// Navigation methods for errors
		public void NavigateToFirstError()
		{
			if (errors.Count > 0)
			{
				currentErrorIndex = 0;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToNextError()
		{
			if (currentErrorIndex < errors.Count - 1)
			{
				currentErrorIndex++;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToPreviousError()
		{
			if (currentErrorIndex > 0)
			{
				currentErrorIndex--;
				OpenItem(errors[currentErrorIndex]);
			}
		}
		public void NavigateToLastError()
		{
			if (errors.Count > 0)
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
			System.Diagnostics.Debugger.Break();
			// Open the document
			var document = _dte.Documents.Open(item.FilePath);

			// Ensure the document is active
			document.Activate();

			// Get the active document's selection
			var selection = (TextSelection)_dte.ActiveDocument.Selection;

			// Move to the specified line number and column (1-based index)
			selection.MoveToLineAndOffset(item.LineNumber, 1);
		}
	}
}
