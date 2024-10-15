using System.ComponentModel.Design;

namespace TestNavigatorPlus
{
	public static class MenuCommandExtensions
	{
		public static void AddMenuCommand(this OleMenuCommandService commandService, EventHandler handler, Guid commandSet, int commandId)
		{
			if (commandService == null)
				throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(commandSet, commandId);
			var menuItem = new MenuCommand(handler, menuCommandID);
			commandService.AddCommand(menuItem);
		}
	}
}
