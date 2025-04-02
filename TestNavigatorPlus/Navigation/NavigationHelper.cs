using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Linq;
using TestNavigatorPlus.Tests;
using System.Diagnostics;
using StackFrame = System.Diagnostics.StackFrame;

namespace TestNavigatorPlus.Navigation
{
	public class NavigationHelper
	{
		private DTE2 _dte;
		private IServiceProvider ServiceProvider;
		public NavigationHelper(DTE2 dte2, IServiceProvider serviceProvider)
		{
			_dte = dte2;
			ServiceProvider = serviceProvider;
			dte2.ToolWindows.ErrorList.ShowErrors = true;
			dte2.ToolWindows.ErrorList.ShowMessages = true;
			dte2.ToolWindows.ErrorList.ShowWarnings = true;
		}
		public List<NavigationItem> GetBugsInSolution()
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			var bugs = new List<NavigationItem>();
			var solutionProjects = _dte.Solution.Projects;

			foreach (EnvDTE.Project project in solutionProjects)
			{
				foreach (ProjectItem item in project.ProjectItems)
				{
					// Recursively search through project items
					FindBugsInProjectItem(item, bugs);
				}
			}

			return bugs;
		}

		public void FindBugsInProjectItem(ProjectItem item, List<NavigationItem> bugs)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
			{
				var filePath = item.FileNames[1]; // Get the file path
				var lines = System.IO.File.ReadAllLines(filePath);

				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Contains("TODO") || lines[i].Contains("FIXME"))
					{
						bugs.Add(new NavigationItem
						{
							Name = lines[i],
							FilePath = filePath,
							LineNumber = i + 1 // Line numbers are 1-based
						});
					}
				}
			}
			else if (item.Kind == EnvDTE.Constants.vsProjectItemKindSolutionItems)
			{
				if (item.ProjectItems == null)
				{
					Debug.WriteLine("ProjectItems is null");
					return;
				}
				foreach (ProjectItem subItem in item.ProjectItems)
				{
					FindBugsInProjectItem(subItem, bugs); // Recursively search
				}
			}
		}

		public List<NavigationItem> GetTestsInSolution()
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			var tests = new List<NavigationItem>();
			if (_dte == null)
			{
				return tests;
			}
			var solutionProjects = _dte.Solution.Projects;

			foreach (EnvDTE.Project project in solutionProjects)
			{
				foreach (ProjectItem item in project.ProjectItems)
				{
					// Recursively search through project items
					FindTestsInProjectItem(item, tests);
				}
			}

			return tests;
		}

		public void FindTestsInProjectItem(ProjectItem item, List<NavigationItem> tests)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			if (item == null)
			{
				Debug.WriteLine("ProjectItem is null");
				return;
			}

			if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
			{
				var filePath = item.FileNames[1]; // Get the file path
				if (string.IsNullOrEmpty(filePath))
				{
					Debug.WriteLine("FilePath is null or empty");
					return;
				}

				var code = System.IO.File.ReadAllText(filePath);
				var tree = CSharpSyntaxTree.ParseText(code);
				var root = tree.GetCompilationUnitRoot();

				// Find methods with specific test attributes
				var testMethods = root.DescendantNodes()
					.OfType<MethodDeclarationSyntax>()
					.Where(m => m.AttributeLists
						.SelectMany(a => a.Attributes)
						.Any(attr => attr.Name.ToString() == "TestMethod" ||
									 attr.Name.ToString() == "Fact" ||
									 attr.Name.ToString() == "Test"));

				foreach (var method in testMethods)
				{
					tests.Add(new NavigationItem
					{
						Name = method.Identifier.Text,
						FilePath = filePath,
						LineNumber = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1 // Line numbers are 1-based
					});
				}
			}
			else if (item.Kind == EnvDTE.Constants.vsProjectItemKindSolutionItems)
			{
				if (item.ProjectItems == null)
				{
					Debug.WriteLine("ProjectItems is null");
					return;
				}
				foreach (ProjectItem subItem in item.ProjectItems)
				{
					FindTestsInProjectItem(subItem, tests); // Recursively search
				}
			}
		}

		public List<NavigationItem> GetCompilerErrors()
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			List<NavigationItem> errorList = new List<NavigationItem>();
			ThreadHelper.ThrowIfNotOnUIThread();
			EnvDTE80.DTE2 dte2 = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as DTE2;
			ErrorItems errors = dte2.ToolWindows.ErrorList.ErrorItems;
			for (int i = 0; i < errors.Count; i++)
			{
				ErrorItem item = errors.Item(i);
				NavigationItem navigationItem = new NavigationItem();
				navigationItem.ColumnNumber = item.Column;
				navigationItem.LineNumber = item.Line;
				navigationItem.FilePath = item.FileName;
				//navigationItem.BugSeverity = item.ErrorLevel
				navigationItem.Description = item.Description;
				navigationItem.Project = item.Project;
				errorList.Add(navigationItem);
			}
			return errorList;
		}

		private string GetCorrespondingSourceFilePath(string testFilePath)
		{
			// Implement logic to find the corresponding source file
			// This is a simplified example
			return testFilePath.Replace("Tests", "");
		}

		private string GetCorrespondingTestFilePath(string sourceFilePath)
		{
			// Implement logic to find the corresponding test file
			// This is a simplified example
			return sourceFilePath.Insert(sourceFilePath.LastIndexOf('.'), "Tests");
		}
		private void DisplayResults(IEnumerable<MethodDeclarationSyntax> missingTests, IEnumerable<MethodDeclarationSyntax> missingImplementations)
		{
			// Implement logic to display results to the user
			// This could be a custom tool window, output window, or dialog box
		}

		//private void AnalyzeSourceAndTestFiles()
		//{
		//	ThreadHelper.ThrowIfNotOnUIThread();

		//	var currentDocument = _dte.ActiveDocument;
		//	if (currentDocument == null) return;

		//	var currentFilePath = currentDocument.FullName;
		//	var isTestFile = currentFilePath.Contains("Test") || currentFilePath.Contains("Tests");

		//	var sourceFilePath = isTestFile ? GetCorrespondingSourceFilePath(currentFilePath) : currentFilePath;
		//	var testFilePath = isTestFile ? currentFilePath : GetCorrespondingTestFilePath(currentFilePath);

		//	var sourceTree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(sourceFilePath));
		//	var testTree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(testFilePath));

		//	var sourceRoot = sourceTree.GetCompilationUnitRoot();
		//	var testRoot = testTree.GetCompilationUnitRoot();

		//	var sourceMethods = sourceRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();
		//	var testMethods = testRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();

		//	var missingTests = sourceMethods.Where(sm => !testMethods.Any(tm => tm.Identifier.Text == $"{sm.Identifier.Text}Test"));
		//	var missingImplementations = testMethods.Where(tm => !sourceMethods.Any(sm => $"{sm.Identifier.Text}Test" == tm.Identifier.Text));

		//	// Display results to the user
		//	DisplayResults(missingTests, missingImplementations);
		//}

		public void AnalyzeSourceAndTestFiles()
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			ThreadHelper.ThrowIfNotOnUIThread();

			var currentDocument = _dte.ActiveDocument;
			if (currentDocument == null) return;

			var currentFilePath = currentDocument.FullName;
			var isTestFile = currentFilePath.Contains("Test") || currentFilePath.Contains("Tests");

			var sourceFilePath = isTestFile ? GetCorrespondingSourceFilePath(currentFilePath) : currentFilePath;
			var testFilePath = isTestFile ? currentFilePath : GetCorrespondingTestFilePath(currentFilePath);

			// Read the source and test files
			var sourceCode = System.IO.File.ReadAllText(sourceFilePath);
			var testCode = System.IO.File.ReadAllText(testFilePath);

			// Use the TestCoverageAnalyzer2 to analyze the coverage
			var analyzer = new TestCoverageAnalyzer2();
			var coverageResult = analyzer.Analyze(sourceCode, testCode);

			// Display results to the user
			DisplayResults(coverageResult);
		}

		private void DisplayResults(TestCoverageResult coverageResult)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			// Implement logic to display results to the user
			// This could be a custom tool window, output window, or dialog box
			var message = $"Total Methods: {coverageResult.TotalMethodsCount}\n" +
						  $"Tested Methods: {string.Join(", ", coverageResult.TestedMethods)}\n" +
						  $"Untested Methods: {string.Join(", ", coverageResult.UntestedMethods)}";

			// Show the results in a message box or output window
			VsShellUtilities.ShowMessageBox(
				this.ServiceProvider,
				message,
				"Test Coverage Analysis",
				OLEMSGICON.OLEMSGICON_INFO,
				OLEMSGBUTTON.OLEMSGBUTTON_OK,
				OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
		}


		public void GetAllBookmarks()
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			foreach (EnvDTE.Project project in _dte.Solution.Projects)
			{
				GetBookmarksInProject(project);
			}
		}

		private void GetBookmarksInProject(EnvDTE.Project project)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			foreach (ProjectItem item in project.ProjectItems)
			{
				if (item.SubProject != null)
				{
					GetBookmarksInProject(item.SubProject);
				}
				else if (item.FileCodeModel != null)
				{
					GetBookmarksInFile(item);
				}
			}
		}

		private void GetBookmarksInFile(ProjectItem item)
		{
			Debug.WriteLine(new StackFrame(1).GetMethod());

			TextDocument textDoc = (TextDocument)item.Document.Object("TextDocument");
			EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();

			while (editPoint.FindPattern("{BOOKMARK}", (int)vsFindOptions.vsFindOptionsNone))
			{
				int line = editPoint.Line;
				string fileName = item.FileNames[0];
				Console.WriteLine($"Bookmark found in {fileName} at line {line}");

				editPoint.LineDown();
			}
		}
	}
}

