﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using DataAccessLibrary;
using JetBrains.Annotations;
using PrivateWiki.Data.DataAccess;

#nullable enable

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls
{
	public sealed partial class NavigationSettingsLinkItemControl : UserControl
	{
		public ObservableCollection<PageModel> Pages { get; private set; } = new ObservableCollection<PageModel>();

		public string Label => Text.Text;

		public NavigationSettingsLinkItemControl()
		{
			this.InitializeComponent();
			LoadPages();
		}

		public void InitControl(string? label, Guid? id)
		{
			Text.Text = label;

			if (id != null)
			{
				LinkCombo.SelectedIndex = Pages.IndexOf(Pages.First(p => p.Id.Equals(id)));
			}
		}

		private void LoadPages()
		{
			var dataAccess = new DataAccessImpl();
			var pages = dataAccess.GetPages();
			Pages = new ObservableCollection<PageModel>(pages);
		}

		public event TextChangedEventHandler LabelChanged;

		private void Text_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			LabelChanged?.Invoke(this, e);
		}

		public event RoutedEventHandler DeleteLink;

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			DeleteLink?.Invoke(this, e);
		}

		public event LinkSelectedEventHandler LinkSelected;

		private void LinkCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var link = (PageModel) e.AddedItems.First();
			LinkSelected?.Invoke(link.Id);
		}
	}

	public delegate void LinkSelectedEventHandler([In] Guid id);

}
