using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls.Settings.Rendering
{
	public sealed partial class DiagramControl : UserControl
	{
		private DiagramRenderModel Model { get; set; }

		public DiagramControl()
		{
			this.InitializeComponent();
		}

		public void Init(DiagramRenderModel model)
		{
			this.Model = model;
		}
	}
}