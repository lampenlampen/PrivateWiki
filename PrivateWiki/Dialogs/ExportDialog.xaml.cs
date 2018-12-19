using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using JetBrains.Annotations;
using PrivateWiki.Data;
using PrivateWiki.Markdig;
using StorageProvider;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class ExportDialog : DissmissableDialog
	{
		[NotNull] private string _id;

		public ExportDialog(string id)
		{
			this.InitializeComponent();
			_id = id;
		}

		private async void Export_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var folder = await MediaAccess.PickFolderAsync();

			List<ContentPage> pages = new List<ContentPage>();

			if (ExportAllPages.IsChecked == true)
			{
				pages.AddRange(new ContentPageProvider().GetAllContentPages());
			}
			else if (ExportSinglePage.IsChecked == true)
			{
				pages.Add(new ContentPageProvider().GetContentPage(_id));
			}
			else
			{
				// TODO Error
			}

			var exportHtml = ExportHtml.IsChecked == true;
			var exportMarkdown = ExportMarkdown.IsChecked == true;

			foreach (var page in pages)
			{
				if (exportHtml)
				{
					var parser = new MarkdigParser();

					var file = await folder.CreateFileAsync($"{page.Id.Replace(':', '_')}.html",
						CreationCollisionOption.ReplaceExisting);
					await FileIO.WriteTextAsync(file, parser.ToHtmlString(page.Content), UnicodeEncoding.Utf8);
				}

				if (exportMarkdown)
				{
					var file = await folder.CreateFileAsync($"{page.Id.Replace(':', '_')}.md",
						CreationCollisionOption.ReplaceExisting);
					await FileIO.WriteTextAsync(file, page.Content, UnicodeEncoding.Utf8);
				}
			}

			Hide();
		}
	}
}