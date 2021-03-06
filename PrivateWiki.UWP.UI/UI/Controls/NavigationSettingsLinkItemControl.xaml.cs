﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService.SQLite;
using Page = PrivateWiki.DataModels.Pages.Page;

#nullable enable

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls
{
	public sealed partial class NavigationSettingsLinkItemControl : UserControl
	{
		public ObservableCollection<GenericPage> Pages { get; private set; }

		public string Label => Text.Text;

		public NavigationSettingsLinkItemControl()
		{
			this.InitializeComponent();
			var storage = new SqLiteStorageOptions("test");
			var backend = new SqLiteBackend(storage, SystemClock.Instance);
			LoadPages(backend);
		}

		public void InitControl(string? label, Guid? id)
		{
			Text.Text = label;

			if (id != null)
			{
				LinkCombo.SelectedIndex = Pages.IndexOf(Pages.First(p => p.Id.Equals(id)));
			}
		}

		private async void LoadPages(SqLiteBackend storage)
		{
			var pages = await storage.GetAllPagesAsync();

			Pages = new ObservableCollection<GenericPage>(pages);
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
			var link = (Page) e.AddedItems.First();
			LinkSelected?.Invoke(link.Id);
		}
	}

	public delegate void LinkSelectedEventHandler([In] Guid id);
}