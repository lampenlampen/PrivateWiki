using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using JetBrains.Annotations;
using LanguageExt;
using PrivateWiki.Data;
using StorageProvider;

namespace PrivateWiki.MarkdownWikiFile
{
	public class MarkdownWikiFile
	{
		private MarkdownWikiFile([NotNull] ZipArchive archive)
		{
			Archive = archive;
		}

		[NotNull] public ZipArchive Archive { get; }

		[NotNull]
		public static MarkdownWikiFile openMarkdownWikiFile([NotNull] StorageFile file)
		{
			using (var fs = new FileStream(file.Path, FileMode.Open))
			using (var archive = new ZipArchive(fs, ZipArchiveMode.Update, true))
			{
				return new MarkdownWikiFile(archive);
			}
		}

		[NotNull]
		public static async Task<MarkdownWikiFile> createMarkdownWikiFileAsync()
		{
			var file = await MediaAccess.PickMarkdownWikiFileAsync();
			using (var fs = new FileStream(file.Path, FileMode.Create))
			using (var archive = new ZipArchive(fs, ZipArchiveMode.Update, true))
			{
				return new MarkdownWikiFile(archive);
			}
		}

		public void close()
		{
			Archive.Dispose();
		}

		public async void saveToMarkdownWikiFile()
		{
			var tmpMarkdownWikiDir = await
				ApplicationData.Current.TemporaryFolder.CreateFolderAsync("MarkdownWikiTmp",
					CreationCollisionOption.ReplaceExisting);

			var pages = new ContentPageProvider().GetAllContentPages();

			foreach (var page in pages)
			{
				var name = page.Id;
				var file = await tmpMarkdownWikiDir.CreateFileAsync($"{name.Replace(":", "_")}.md");
				await File.WriteAllTextAsync(file.Path, page.Content);
			}

			var fileDest = await MediaAccess.PickMarkdownWikiFileAsync();

			ZipFile.CreateFromDirectory(tmpMarkdownWikiDir.Path, fileDest.Path);
		}

		public async Task<bool> saveToMarkdownWikiFile(ContentPage page)
		{
			var entryName = page.Id.Replace(':', '/') + ".md";
			var entry = Archive.CreateEntry(entryName);
			using (var stream = entry.Open())
			using (var writer = new StreamWriter(stream))
			{
				writer.Write(page.Content);
				return true;
			}
		}

		public async Task<Option<ContentPage>> openContentPage(string id)
		{
			var entryName = id.Replace(':', '/') + ".md";
			var entry = Archive.GetEntry(entryName);

			if (entry == null) return Option<ContentPage>.None;

			var page = ContentPage.Create(id);

			using (var stream = entry.Open())
			using (var reader = new StreamReader(stream))
			{
				page.Content = await reader.ReadToEndAsync();
				return page;
			}
		}

		public async void saveAllPagesAsync()
		{
			var pages = new ContentPageProvider().GetAllContentPages();
			foreach (var page in pages)
			{
				var name = page.Id.Replace(':', '/') + ".md";
				var entry = Archive.CreateEntry(name);

				using (var stream = entry.Open())
				using (var writer = new StreamWriter(stream))
				{
					await writer.WriteAsync(page.Content);
				}
			}
		}
	}
}