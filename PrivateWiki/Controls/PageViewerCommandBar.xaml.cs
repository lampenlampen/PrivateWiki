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
using PrivateWiki.Pages;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls
{
	public sealed partial class PageViewerCommandBar : UserControl
	{
		public PageViewerCommandBar()
		{
			this.InitializeComponent();
		}

		public event RoutedEventHandler TopClick;

		private void Top_Click(object sender, RoutedEventArgs e)
		{
			TopClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler PdfClick;

		private void Pdf_Click(object sender, RoutedEventArgs e)
		{
			PdfClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler EditClick;

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			EditClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler SearchClick;

		private void Search_Click(object sender, RoutedEventArgs e)
		{
			SearchClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler HistoryClick;

		private void History_Click(object sender, RoutedEventArgs e)
		{
			HistoryClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler FullscreenClick;

		private void Fullscreen_Click(object sender, RoutedEventArgs e)
		{
			FullscreenClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler ExportClick;

		private void Export_Click(object sender, RoutedEventArgs e)
		{
			ExportClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler ImportClick;

		private void Import_Click(object sender, RoutedEventArgs e)
		{
			ImportClick?.Invoke(sender, e);
		}

		public event RoutedEventHandler SettingsClick;

		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			SettingsClick?.Invoke(sender, e);
		}
	}
}