using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Collections.Generic;
using System.Linq;

namespace TestNavigatorPlus
{
	internal class NavigationHelper
	{
		private static DTE2 _dte;
		private IServiceProvider ServiceProvider;
		public NavigationHelper(DTE2 dte2, IServiceProvider serviceProvider)
		{
			_dte = dte2;
			ServiceProvider = serviceProvider;
		}
		public List<NavigationItem> GetBugsInSolution()
		{
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
				foreach (ProjectItem subItem in item.ProjectItems)
				{
					FindBugsInProjectItem(subItem, bugs); // Recursively search
				}
			}
		}

		private static List<NavigationItem> GetTestsInSolution()
		{
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

		private static void FindTestsInProjectItem(ProjectItem item, List<NavigationItem> tests)
		{
			if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
			{
				var filePath = item.FileNames[1]; // Get the file path
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
				foreach (ProjectItem subItem in item.ProjectItems)
				{
					FindTestsInProjectItem(subItem, tests); // Recursively search
				}
			}
		}
		private List<NavigationItem> GetCompilerErrors(TestNavigatorCommand testNavigatorCommand)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var errorList = new List<NavigationItem>();

			// Get the SVsTaskList service
			var taskList = ServiceProvider.GetService(typeof(SVsTaskList)) as IVsTaskList;
			if (taskList != null)
			{
				// Get the list of error items
				if (taskList.EnumTaskItems(out IVsEnumTaskItems enumTaskItems) == VSConstants.S_OK)
				{
					IVsTaskItem[] taskItems = new IVsTaskItem[1];
					uint fetched = 0;

					while (enumTaskItems.Next(1, taskItems) == VSConstants.S_OK && fetched == 1)
					{
						var taskItem = taskItems[0];

						// Get the error details
						taskItem.Column(out int column);
						taskItem.Line(out int lineNumber);
						taskItem.CanDelete(out int canDelete);
						taskItem.get_Text(out string errorMessage);
						taskItem.get_Checked(out int isChecked);
						taskItem.Document(out string filePath);
						taskItem.HasHelp(out int hasHelp);
						taskItem.ImageListIndex(out int imageIndex);
						taskItem.SubcategoryIndex(out int isReadOnly);

						errorList.Add(new NavigationItem
						{
							Name = errorMessage,
							FilePath = filePath,
							LineNumber = lineNumber + 1, // Line numbers are zero-based
							TaskItem = taskItem
						});
					}
				}
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

	}
}
