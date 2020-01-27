using System;
using System.Drawing;
using System.Linq;
using Windows.System;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Contracts;
using Contracts.Storage;
using JetBrains.Annotations;
using Models.Pages;
using NLog;
using PrivateWiki.Data;
using PrivateWiki.Dialogs;
using PrivateWiki.Utilities;
using PrivateWiki.Utilities.ExtensionFunctions;
using RavinduL.LocalNotifications.Notifications;
using Page = Models.Pages.Page;

namespace PrivateWiki.Pages.ContentPages
{
	public abstract class ContentPage : Windows.UI.Xaml.Controls.Page, IPageViewerCommandBar
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		protected IMarkdownPageStorage _storage;

		protected Page Page { get; set; }


		public async void NavigateToPage(Page page, string link)
		{
			if (page.Link.Equals(link)) return;

			if (await _storage.ContainsMarkdownPageAsync(link))
				Frame.Navigate(typeof(MarkdownPageViewer), link);
			else
				Frame.Navigate(typeof(NewPage), link);
		}

		public void Edit_Click(object sender, RoutedEventArgs e)
		{
			Logger.Debug("Edit Page");

			if (Page.IsLocked)
			{
				App.Current.manager.Show(new SimpleNotification
				{
					TimeSpan = TimeSpan.FromSeconds(3),
					Text = "Page cannot be edited.",
					Glyph = "\uE1F6",
					VerticalAlignment = VerticalAlignment.Bottom,
					Background = new SolidColorBrush(Color.Red.ToWindowsUiColor())
				});
			}
			else
			{
				Frame.Navigate(typeof(PageEditor), Page.Link);
			}
		}

		public void Revision_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(HistoryPage), Page.Link);
		}

		public abstract void Top_Click(object sender, RoutedEventArgs e);

		/// <summary>
		/// This method is called, when the user clicks the "Print"-Button.
		///
		/// Prints the current page to a pdf file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void Pdf_Click(object sender, RoutedEventArgs e)
		{
			// TODO Print PDF
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

			var dialog = new ContentDialog
			{
				Title = resourceLoader.GetString("PrintPDF/Dialog/Title"),
				Content = resourceLoader.GetString("PrintPDF/Dialog/Content"),
				PrimaryButtonText = resourceLoader.GetString("PrintPDF/Dialog/OpenInBrowser"),
				CloseButtonText = resourceLoader.GetString("Close"),
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				var pageExporter = new PageExporter();
				var file = await pageExporter.ExportPage((MarkdownPage)Page);

				_ = Launcher.LaunchFileAsync(file);
			}
		}

		public void Fullscreen_Click(object sender, RoutedEventArgs e)
		{
			var view = ApplicationView.GetForCurrentView();
			if (view.IsFullScreenMode) view.ExitFullScreenMode();
			else view.TryEnterFullScreenMode();
		}


		public void Setting_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SettingsPage));
		}

		public abstract void Search_Click(object sender, RoutedEventArgs e);

		public void Export_Click(object sender, RoutedEventArgs e)
		{
			_ = new ExportDialog(Page.Link).ShowAsync();
		}

		public void Import_Click(object sender, RoutedEventArgs e)
		{
			// TODO Import Dialog

			App.Current.manager.ShowNotImplementedNotification();
		}

		public void CommandBar_OnNavigateToPage(object sender, string id)
		{
			NavigateToPage(Page, id);
		}
	}
}