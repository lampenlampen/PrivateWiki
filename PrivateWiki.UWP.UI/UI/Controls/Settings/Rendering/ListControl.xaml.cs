﻿using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls.Settings.Rendering
{
	public sealed partial class ListControl : UserControl
	{
		private ListRenderModel Model { get; set; }

		public ListControl()
		{
			this.InitializeComponent();
		}

		public void Init(ListRenderModel model)
		{
			this.Model = model;
		}
	}
}