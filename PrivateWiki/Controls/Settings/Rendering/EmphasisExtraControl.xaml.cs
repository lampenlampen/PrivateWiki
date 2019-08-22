﻿using System;
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