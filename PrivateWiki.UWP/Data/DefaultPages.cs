using System;
using System.Threading.Tasks;
using Windows.Storage;
using NodaTime;
using PrivateWiki.Models.Pages;
using PrivateWiki.UWP.StorageBackend.SQLite;

namespace PrivateWiki.UWP.Data
{
	public static class DefaultPages
	{
		public static async Task InsertDefaultMarkdownPagesAsync(SqLiteBackend backend, IClock clock)
		{
			var syntaxPage = await LoadSyntaxPage(clock);
			if (!await backend.ContainsMarkdownPageAsync(syntaxPage.Link)) backend.InsertMarkdownPageAsync(syntaxPage);

			var startPage = await LoadStartPage(clock);
			if (!await backend.ContainsMarkdownPageAsync(startPage.Link)) await backend.InsertMarkdownPageAsync(startPage);

			var testPage = await LoadExamplePage(clock);
			if (!await backend.ContainsMarkdownPageAsync(testPage.Link)) backend.InsertMarkdownPageAsync(testPage);

			var htmlTestPage = await LoadHtmlTestPage(clock);
			if (!await backend.ContainsPageAsync(htmlTestPage)) backend.InsertPageAsync(htmlTestPage);
		}

		private static async Task<MarkdownPage> LoadSyntaxPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var syntaxFile = await defaultPagesDir.GetFileAsync("Syntax.md");

			var content = await FileIO.ReadTextAsync(syntaxFile);

			return new MarkdownPage(Guid.NewGuid(), "system:syntax", content, clock.GetCurrentInstant(), clock.GetCurrentInstant(), true);
		}

		private static async Task<MarkdownPage> LoadStartPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("Start.md");

			var content = await FileIO.ReadTextAsync(startFile);

			return new MarkdownPage(Guid.NewGuid(), "start", content, clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
		}

		private static async Task<MarkdownPage> LoadExamplePage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var exampleFile = await defaultPagesDir.GetFileAsync("Example.md");

			var content = await FileIO.ReadTextAsync(exampleFile);

			return new MarkdownPage(Guid.NewGuid(), "test", content, clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
		}

		private static async Task<GenericPage> LoadHtmlTestPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("HtmlTest.html");

			var content = await FileIO.ReadTextAsync(startFile);

			return new GenericPage(Path.ofLink("system:htmltest"), content, "html", clock.GetCurrentInstant(), clock.GetCurrentInstant(), true);
		}
	}
}