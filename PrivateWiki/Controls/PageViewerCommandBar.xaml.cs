﻿using System;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JetBrains.Annotations;
using NLog;
using Page = Models.Pages.Page;
using Path = Models.Pages.Path;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls
{
	public sealed partial class PageViewerCommandBar : UserControl
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public new object Content
		{
			get => commandBar.Content;
			set => commandBar.Content = value;
		}

		public PageViewerCommandBar()
		{
			this.InitializeComponent();
			ShowLastVisitedPages();
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

		private void ShowLastVisitedPages()
		{
			var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
			var stack = NavigationHandler.Pages;

			if (stack.Count <= 0) return;

			for (var index = 0; index < stack.Count - 1; index++)
			{
				var page = stack[index];
				var textBlock = GetTextBlock(page);
				stackPanel.Children.Add(textBlock);

				var delimiterBox = GetDelimiterBlock();
				stackPanel.Children.Add(delimiterBox);
			}

			var lastTextBox = GetTextBlock(stack.Last());
			stackPanel.Children.Add(lastTextBox);

			commandBar.Content = stackPanel;

			Button GetTextBlock(Path path)
			{
				var button = new Button
				{
					Content = path.Title,
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Background = new RevealBackgroundBrush(),
					BorderBrush = new RevealBorderBrush(),
					FontStyle = FontStyle.Italic
				};

				var tooltip = new ToolTip();
				tooltip.Content = path.FullPath;
				ToolTipService.SetToolTip(button, tooltip);

				button.Click += Btn_Click;

				return button;
			}

			TextBlock GetDelimiterBlock()
			{
				return new TextBlock
				{
					Text = ">",
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Margin = new Thickness(2, 0, 2, 0)
				};
			}
		}

		public delegate void NavigateToPageHandler(object sender, string id);

		public event NavigateToPageHandler NavigateToPage;

		private void Btn_Click(object sender, RoutedEventArgs e)
		{
			var id = (string)((Button)sender).Content;
			Logger.Debug($"Page Clicked: {id}");

			NavigateToPage?.Invoke(sender, id);
		}
	}
}