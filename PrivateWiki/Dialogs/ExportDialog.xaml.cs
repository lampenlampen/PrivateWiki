using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using DataAccessLibrary;
using JetBrains.Annotations;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using PrivateWiki.Markdig;
using StorageProvider;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class ExportDialog : DissmissableDialog
	{
		[NotNull] private readonly string _id;

		private DataAccessImpl dataAccess;

		public ExportDialog(string id)
		{
			InitializeComponent();
			_id = id;
			dataAccess = new DataAccessImpl();
		}

		private async void Export_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var folder = await MediaAccess.PickFolderAsync();

			var pages = new List<PageModel>();

			if (ExportAllPages.IsChecked == true)
			{
				pages.AddRange(dataAccess.GetPages());
			}
			else if (ExportSinglePage.IsChecked == true)
			{
				pages.Add(dataAccess.GetPageOrNull(_id));
			}

			var exportHtml = ExportHtml.IsChecked == true;
			var exportMarkdown = ExportMarkdown.IsChecked == true;

			foreach (var page in pages)
			{
				if (exportHtml)
				{
					var parser = new MarkdigParser();

					var file = await folder.CreateFileAsync($"{page.Link.Replace(':', '_')}.html",
						CreationCollisionOption.ReplaceExisting);
					await FileIO.WriteTextAsync(file, parser.ToHtmlString(page.Content), UnicodeEncoding.Utf8);
				}

				if (exportMarkdown)
				{
					var file = await folder.CreateFileAsync($"{page.Link.Replace(':', '_')}.md",
						CreationCollisionOption.ReplaceExisting);
					await FileIO.WriteTextAsync(file, page.Content, UnicodeEncoding.Utf8);
				}
			}

			Hide();
		}
	}
}