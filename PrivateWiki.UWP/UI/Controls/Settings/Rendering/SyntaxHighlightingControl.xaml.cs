using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.Settings.Rendering
{
	public sealed partial class SyntaxHighlightingControl : UserControl
	{
		private SyntaxHighlightingRenderModel Model { get; set; }

		public SyntaxHighlightingControl()
		{
			this.InitializeComponent();
		}

		public void Init(SyntaxHighlightingRenderModel model)
		{
			this.Model = model;
		}
	}
}