﻿using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls.Settings.Rendering
{
	public sealed partial class EmphasisExtraControl : UserControl
	{
		private EmphasisExtraRenderModel Model { get; set; }

		public EmphasisExtraControl()
		{
			this.InitializeComponent();
		}

		public void Init(EmphasisExtraRenderModel model)
		{
			this.Model = model;
		}
	}
}