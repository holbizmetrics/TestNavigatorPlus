using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestNavigatorPlus.Tests
{
	public class TestCoverageAnalyzer2
	{
		public ImmutableArray<SyntaxTree> SyntaxTrees { get; set; }
		public TestCoverageResult Analyze(string classCode, string testCode)
		{
			var results = new TestCoverageResult();

			var classTree = CSharpSyntaxTree.ParseText(classCode);
			var testTree = CSharpSyntaxTree.ParseText(testCode);
			var compilation = CSharpCompilation.Create("Analysis")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(classTree, testTree);

			SyntaxTrees = compilation.SyntaxTrees;

			// Get semantic model for classTree
			var classModel = compilation.GetSemanticModel(classTree);
			var testModel = compilation.GetSemanticModel(testTree);

			var publicMethodSymbols = classTree.GetRoot().DescendantNodes()
				.OfType<MethodDeclarationSyntax>()
				.Select(method => classModel.GetDeclaredSymbol(method))
				.Where(symbol => symbol != null && symbol.DeclaredAccessibility == Accessibility.Public)
				.ToList();

			var testMethodInvocations = testTree.GetRoot().DescendantNodes()
				.OfType<InvocationExpressionSyntax>();

			results.TotalMethodsCount = publicMethodSymbols.Count;

			foreach (var methodSymbol in publicMethodSymbols)
			{
				var isTested = testMethodInvocations.Any(invocation =>
				{
					var invokedSymbol = testModel.GetSymbolInfo(invocation).Symbol;
					return invokedSymbol?.Equals(methodSymbol) ?? false;
				});

				if (isTested)
				{
					results.TestedMethods.Add(methodSymbol.ToDisplayString());
				}
				else
				{
					results.UntestedMethods.Add(methodSymbol.Name);
				}
			}

			return results;
		}

		// Overloaded Analyze method to take a Type and testCode as string
		public TestCoverageResult Analyze(Type type, string testCode)
		{
			var result = new TestCoverageResult();
			var testTree = CSharpSyntaxTree.ParseText(testCode);
			var testRoot = testTree.GetRoot();
			var testMethodInvocations = testRoot.DescendantNodes().OfType<InvocationExpressionSyntax>();

			// Getting methods, properties, events
			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			var events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

			// Combining member names for comparison
			var memberNames = methods.Select(m => m.Name)
			.Concat(properties.Select(p => p.Name))
			.Concat(events.Select(e => e.Name)).ToList();

			result.TotalMethodsCount = memberNames.Count;

			// Check each member to see if it appears in any test method invocation
			foreach (var memberName in memberNames)
			{
				if (testMethodInvocations.Any(invocation => invocation.ToString().Contains(memberName)))
				{
					result.TestedMethods.Add(memberName);
				}
				else
				{
					result.UntestedMethods.Add(memberName);
				}
			}

			return result;
		}

		public Type LoadTypeFromAssembly(string AssemblyPath, Type type)
		{
			return LoadTypeFromAssembly(AssemblyPath, type.FullName);
		}

		// New method to load a type from an assembly
		public Type LoadTypeFromAssembly(string assemblyPath, string typeName)
		{
			try
			{
				// Load the assembly from the specified path
				var assembly = Assembly.LoadFrom(assemblyPath);
				// Try to get the type by its fully qualified name
				var type = assembly.GetType(typeName, throwOnError: true);
				return type;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading type from assembly: {ex.Message}");
				return null;
			}
		}

		public object CreateInstanceOfType(string assemblyPath, string typeName)
		{
			var assembly = Assembly.LoadFrom(assemblyPath);
			var type = assembly.GetType(typeName);
			if (type != null)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		public void ReportTestCoverageProgress(TestCoverageResult result)
		{
			if (result.TotalMethodsCount == 0)
			{
				Console.WriteLine("No methods found for analysis.");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine($"{result.TestedMethods} of {result.TotalMethodsCount} methods are tested. Tested: {result.TestedPercentage:0.00}%, Untested: {result.UntestedPercentage:0.00}%");
				Console.WriteLine();
				if (result.UntestedMethods.Any())
				{
					Console.WriteLine("Untested methods:");
					foreach (var method in result.UntestedMethods)
					{
						Console.WriteLine($"- {method}");
					}
					Console.WriteLine();
				}
			}
		}

		public void ShowDiagnostics()
		{
			// Assume testCoverageAnalyzer2.SyntaxTrees represents a collection of syntax trees analyzed
			foreach (var tree in this.SyntaxTrees)
			{
				foreach (var diagnostic in tree.GetDiagnostics())
				{
					var filePath = diagnostic.Location.SourceTree.ToString(); // Assuming each tree correctly stores its file path
																			  // Display the diagnostic message with file path for context
					Console.WriteLine($"Diagnostic in file {filePath}:");
					Console.WriteLine(diagnostic.ToString());

					if (!string.IsNullOrEmpty(filePath) && File.Exists(diagnostic.Location.SourceTree.ToString()))
					{
						// Use FileLinePositionSpan to get start and end lines for the diagnostic
						var lineSpan = diagnostic.Location.GetMappedLineSpan();
						int startLine = lineSpan.StartLinePosition.Line + 1; // Adjusting for zero-based indexing
						int endLine = lineSpan.EndLinePosition.Line + 1;

						// Reading and displaying the relevant lines from the file
						try
						{
							var lines = File.ReadLines(filePath).Skip(startLine - 1).Take(endLine - startLine + 1);
							Console.WriteLine($"Related code in {filePath} (Lines {startLine} to {endLine}):");
							foreach (var line in lines)
							{
								Console.WriteLine(line);
							}
						}
						catch (IOException ex)
						{
							Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
						}
					}
					else
					{
						Console.WriteLine("File path is empty or the file does not exist.");
					}

					Console.WriteLine(); // Add an extra line for better readability between diagnostics
				}
			}
		}
	}
}
