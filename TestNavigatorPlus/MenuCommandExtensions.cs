using System.ComponentModel.Design;

namespace TestNavigatorPlus
{
	public static class MenuCommandExtensions
	{
		/// <summary>
		/// Adds a MenuCommand 
		/// </summary>
		/// <param name="commandService"></param>
		/// <param name="handler"></param>
		/// <param name="commandSet"></param>
		/// <param name="commandId"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void AddMenuCommand(this OleMenuCommandService commandService, EventHandler handler, Guid commandSet, int commandId)
		{
			if (commandService == null)
				throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(commandSet, commandId);
			var menuItem = new MenuCommand(handler, menuCommandID);
			commandService.AddCommand(menuItem);
		}
	

	    /// <summary>
        /// Adds an OleMenuCommand to the OleMenuCommandService and optionally attaches a BeforeQueryStatus event handler.
        /// </summary>
        /// <param name="commandService">The command service to add the command to.</param>
        /// <param name="invokeHandler">The handler for the command's execution.</param>
        /// <param name="beforeQueryStatusHandler">The handler for the BeforeQueryStatus event (can be null).</param>
        /// <param name="commandSet">The GUID of the command set.</param>
        /// <param name="commandId">The ID of the command.</param>
        public static void AddOleMenuCommand(this OleMenuCommandService commandService, EventHandler invokeHandler, EventHandler beforeQueryStatusHandler, Guid commandSet, int commandId)
		{
			if (commandService == null)
				throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(commandSet, commandId);
			var menuItem = new OleMenuCommand(invokeHandler, menuCommandID);

			if (beforeQueryStatusHandler != null)
			{
				menuItem.BeforeQueryStatus += beforeQueryStatusHandler;
			}

			commandService.AddCommand(menuItem);
		}
	}
}
