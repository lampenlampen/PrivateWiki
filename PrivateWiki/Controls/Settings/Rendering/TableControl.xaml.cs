using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls.Settings.Rendering
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
