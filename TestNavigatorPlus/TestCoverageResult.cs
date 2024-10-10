using System.Collections.Generic;
using System.Linq;

namespace TestNavigatorPlus
{
	public class TestCoverageResult
	{
		public int TotalMethodsCount { get; set; } = 0;
		public List<string> UntestedMethods { get; set; } = new List<string>();
		public List<string> TestedMethods { get; set; } = new List<string>();
		public double TestedPercentage => TotalMethodsCount > 0 ? (double)TestedMethods.Count / TotalMethodsCount * 100 : 0;
		public double UntestedPercentage => TotalMethodsCount > 0 ? 100 - TestedPercentage : 0;

		public override string ToString()
		{
			string returnString = Environment.NewLine;
			// Correcting the initial summary to show the count of tested methods, not the list itself
			var summary = $"{TestedMethods.Count} of {TotalMethodsCount} methods are tested. Tested: {TestedPercentage:0.00}%, Untested: {UntestedPercentage:0.00}%{returnString}";

			if (UntestedMethods.Any())
			{
				summary += $"{returnString}Untested methods:";
				foreach (var method in UntestedMethods)
				{
					summary += $"{returnString}- {method}";
				}
			}
			else
			{
				summary += "\nAll methods are tested!";
			}

			// Adding a condition to display tested methods if any exist
			if (TestedMethods.Any())
			{
				summary += $"{returnString}{returnString}Tested methods:";
				foreach (var method in TestedMethods)
				{
					summary += $"{returnString}- {method}";
				}
			}
			return summary;
		}
	}
}
