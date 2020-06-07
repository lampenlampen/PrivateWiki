using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.Settings.Rendering
{
	public sealed partial class TableControl : UserControl
	{
		private TableRenderModel Model { get; set; }

		public TableControl()
		{
			this.InitializeComponent();
		}

		public void Init(TableRenderModel model)
		{
			this.Model = model;
		}
	}
}