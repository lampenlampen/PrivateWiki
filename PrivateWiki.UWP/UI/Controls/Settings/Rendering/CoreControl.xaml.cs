using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.Settings.Rendering
{
	public sealed partial class CoreControl : UserControl
	{
		private CoreRenderModel Model { get; set; }

		public CoreControl()
		{
			this.InitializeComponent();
		}

		public void Init(CoreRenderModel model)
		{
			this.Model = model;
		}
	}
}