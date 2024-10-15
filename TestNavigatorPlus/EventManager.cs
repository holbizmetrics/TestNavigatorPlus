using EnvDTE;
using EnvDTE80;
using System.Diagnostics;
using System.Timers;
using TestNavigatorPlus.Navigation;

namespace TestNavigatorPlus
{
	public class EventManager
	{
		public event EventHandler<EventArgs> Event;

		DTE2 dte;
		NavigationHelper navigationHelper;
		public EventManager(DTE2 dte, NavigationHelper navigationHelper)
		{ 
			this.dte = dte;
			this.navigationHelper = navigationHelper;
		}
		public void SubscribeToBuildEvents()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			var buildEvents = dte.Events.BuildEvents;
			buildEvents.OnBuildDone += OnBuildDone;
		}

		public void SubscribeToSolutionEvents()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			var solutionEvents = dte.Events.SolutionEvents;
			solutionEvents.ProjectAdded += OnProjectChanged;
			solutionEvents.ProjectRemoved += OnProjectChanged;
			solutionEvents.ProjectRenamed += OnProjectRenamed; ;
		}

		private void ExecuteCommand(object sender, EventArgs e)
		{
			System.Diagnostics.Debugger.Break();
			ThreadHelper.ThrowIfNotOnUIThread();

			// Implement command logic here
			navigationHelper.AnalyzeSourceAndTestFiles();
		}


		private void OnProjectRenamed(EnvDTE.Project Project, string OldName)
		{
			throw new NotImplementedException();
		}	
		private void OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			int buildInfo = dte.Solution.SolutionBuild.LastBuildInfo;
			switch (buildInfo)
			{
				case 0:
					Debug.WriteLine("Build success.");
					break;
				case 1:
					Debug.WriteLine("Build errors occurred.");

					break;
			}

			RefreshEntities();
		}
		private void OnProjectChanged(EnvDTE.Project Project)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			RefreshEntities();
		}

		private void RefreshEntities()
		{
			//RefreshCompilerErrors();
			// RefreshTests();
		}


		private ErrorListProvider errorListProvider;
		public void UnsubscribeFromEvents()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			if (dte != null)
			{
				// Unsubscribe from build events
				var buildEvents = dte.Events.BuildEvents;
				if (buildEvents != null)
				{
					buildEvents.OnBuildDone -= OnBuildDone;
				}

				// Unsubscribe from solution events
				var solutionEvents = dte.Events.SolutionEvents;
				if (solutionEvents != null)
				{
					solutionEvents.ProjectAdded -= OnProjectChanged;
					solutionEvents.ProjectRemoved -= OnProjectChanged;
					solutionEvents.ProjectRenamed -= OnProjectRenamed;
				}

				// Unsubscribe from any other events you've subscribed to
			}

			// Dispose of any other resources
			if (errorListProvider != null)
			{
				errorListProvider.Dispose();
				errorListProvider = null;
			}

			if (refreshTimer != null)
			{
				refreshTimer.Stop();
				refreshTimer.Dispose();
				refreshTimer = null;
			}
		}

		private Timer refreshTimer;

		private void InitializeRefreshTimer()
		{
			refreshTimer = new Timer();
			refreshTimer.Interval = 5000; // 5 seconds
			//refreshTimer.Elapsed += (sender, e) => RefreshCompilerErrors();
			refreshTimer.Start();
		}
	}
}
