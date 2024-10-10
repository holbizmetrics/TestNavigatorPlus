//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using Microsoft.VisualStudio.Imaging;
//namespace TestNavigatorPlus
//{
//	public class MyToolWindow : BaseToolWindow<MyToolWindow>
//	{
//		public override string GetTitle(int toolWindowId) => "My Tool Window";

//		public override Type PaneType => typeof(Pane);

//		public override async Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
//		{
//			await Task.Delay(2000); // Long running async task
//			return new MyUserControl();
//		}

//		// Give this a new unique guid
//		[Guid("d3b3ebd9-87d1-41cd-bf84-268d88953417")]
//		internal class Pane : ToolWindowPane
//		{
//			public Pane()
//			{
//				// Set an image icon for the tool window
//				BitmapImageMoniker = KnownMonikers.StatusInformation;
//			}
//		}
//	}
//}
