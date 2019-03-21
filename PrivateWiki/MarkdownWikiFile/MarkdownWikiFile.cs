using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using DataAccessLibrary;
using JetBrains.Annotations;
using LanguageExt;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using StorageProvider;

namespace PrivateWiki.MarkdownWikiFile
{
	public class MarkdownWikiFile
	{
		private DataAccessImpl dataAccess;


		private MarkdownWikiFile([NotNull] ZipArchive archive)
		{
			Archive = archive;
			dataAccess = new DataAccessImpl();
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

			var pages = dataAccess.GetPages();

			foreach (var page in pages)
			{
				var name = page.Link;
				var file = await tmpMarkdownWikiDir.CreateFileAsync($"{name.Replace(":", "_")}.md");
				await File.WriteAllTextAsync(file.Path, page.Content);
			}

			var fileDest = await MediaAccess.PickMarkdownWikiFileAsync();

			ZipFile.CreateFromDirectory(tmpMarkdownWikiDir.Path, fileDest.Path);
		}

		public async Task<bool> saveToMarkdownWikiFile(PageModel page)
		{
			var entryName = page.Link.Replace(':', '/') + ".md";
			var entry = Archive.CreateEntry(entryName);
			using (var stream = entry.Open())
			using (var writer = new StreamWriter(stream))
			{
				writer.Write(page.Content);
				return true;
			}
		}

		public async Task<Option<PageModel>> openContentPage(string id)
		{
			var entryName = id.Replace(':', '/') + ".md";
			var entry = Archive.GetEntry(entryName);

			if (entry == null) return Option<PageModel>.None;

			using (var stream = entry.Open())
			using (var reader = new StreamReader(stream))
			{
				var content = await reader.ReadToEndAsync();
				var page = new PageModel(Guid.NewGuid(), id, content, SystemClock.Instance);
				return page;
			}
		}

		public async void saveAllPagesAsync()
		{
			var pages = dataAccess.GetPages();
			foreach (var page in pages)
			{
				var name = page.Link.Replace(':', '/') + ".md";
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