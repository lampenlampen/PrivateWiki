using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.Settings.Rendering
{
	public sealed partial class MathControl : UserControl
	{
		private MathRenderModel Model { get; set; }

		public MathControl()
		{
			this.InitializeComponent();
		}

		public void Init(MathRenderModel model)
		{
			this.Model = model;
		}
	}
}