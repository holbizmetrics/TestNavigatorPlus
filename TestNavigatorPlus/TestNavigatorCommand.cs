using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using System.Diagnostics;
using TestNavigatorPlus.Navigation;
namespace TestNavigatorPlus
{
	public sealed class TestNavigatorCommand : IDisposable
	{
		private readonly Package package;
		private static DTE2 _dte;
		public static TestNavigatorCommand Instance { get; private set; }
		private IServiceProvider ServiceProvider => package;

		private NavigationHelper navigationHelper = null;
		private Navigator navigator = null;

		private EventManager eventsManager = null;
		private TestNavigatorCommand(Package package)
		{
			Debug.WriteLine("Initializing constructor with package...");
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			_dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

			navigator = new Navigator(_dte, ServiceProvider);
			navigator.SubscribeToEvents();
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
				Guid commandSet = new Guid("{12345678-1234-1234-1234-123456789012}");

				// Adding commands
				// Add commands for bugs
				commandService.AddMenuCommand(navigator.NavigateToFirstBug, commandSet, 0x0101);
				commandService.AddMenuCommand(navigator.NavigateToNextBug, commandSet, 0x0102);
				commandService.AddMenuCommand(navigator.NavigateToPreviousBug, commandSet, 0x0103);
				commandService.AddMenuCommand(navigator.NavigateToLastBug, commandSet, 0x0104);

				// Add commands for tests
				commandService.AddMenuCommand(navigator.NavigateToFirstTest, commandSet, 0x0105);
				commandService.AddMenuCommand(navigator.NavigateToNextTest, commandSet, 0x0106);
				commandService.AddMenuCommand(navigator.NavigateToPreviousTest, commandSet, 0x107);
				commandService.AddMenuCommand(navigator.NavigateToLastTest, commandSet, 0x0108);

				// Add commands for errors
				commandService.AddMenuCommand(navigator.NavigateToFirstError, commandSet, 0x0109);
				commandService.AddMenuCommand(navigator.NavigateToNextError, commandSet, 0x0110);
				commandService.AddMenuCommand(navigator.NavigateToPreviousError, commandSet, 0x0111);
				commandService.AddMenuCommand(navigator.NavigateToLastError, commandSet, 0x0112);
				Debug.WriteLine("Menu item added...");

			}
			else
			{
				Debug.WriteLine("Command service not found");
			}

		}
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
