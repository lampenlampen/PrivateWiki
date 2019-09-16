﻿using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Contracts.Storage;
using JetBrains.Annotations;
using Models.Pages;
using Models.Storage;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using PrivateWiki.Markdig;
using StorageBackend;
using StorageBackend.SQLite;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class ExportDialog : DissmissableDialog
	{
		[NotNull] private readonly string _id;

		public ExportDialog(string id)
		{
			InitializeComponent();
			_id = id;
		}

		private async void Export_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var backend = new SqLiteBackend(new SqLiteStorage("test"), SystemClock.Instance);
			var folder = await MediaAccess.PickFolderAsync();

			var pages = new List<MarkdownPage>();

			if (ExportAllPages.IsChecked == true)
				pages.AddRange((IEnumerable<MarkdownPage>) await backend.GetAllMarkdownPagesAsync());
			else if (ExportSinglePage.IsChecked == true) pages.Add((MarkdownPage) await backend.GetMarkdownPageAsync(_id));

			var exportHtml = ExportHtml.IsChecked == true;
			var exportMarkdown = ExportMarkdown.IsChecked == true;

			foreach (var page in pages)
			{
				if (exportHtml)
				{
					var parser = new Markdig.Markdig();

					var file = await folder.CreateFileAsync($"{page.Link.Replace(':', '_')}.html",
						CreationCollisionOption.ReplaceExisting);
					await FileIO.WriteTextAsync(file, await parser.ToHtmlString(page.Content), UnicodeEncoding.Utf8);
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