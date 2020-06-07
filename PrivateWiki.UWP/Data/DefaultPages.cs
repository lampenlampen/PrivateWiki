using System;
using System.Threading.Tasks;
using Windows.Storage;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.UWP.StorageBackend.SQLite;

namespace PrivateWiki.UWP.Data
{
	public static class DefaultPages
	{
		public static async Task InsertDefaultMarkdownPagesAsync(SqLiteBackend backend, IClock clock)
		{
			var syntaxPage = await LoadSyntaxPage(clock);
			if (!await backend.ContainsPageAsync(syntaxPage.Link)) await backend.InsertPageAsync(syntaxPage);

			var startPage = await LoadStartPage(clock);
			if (!await backend.ContainsPageAsync(startPage.Link)) await backend.InsertPageAsync(startPage);

			var testPage = await LoadExamplePage(clock);
			if (!await backend.ContainsPageAsync(testPage.Link)) await backend.InsertPageAsync(testPage);

			var htmlTestPage = await LoadHtmlTestPage(clock);
			if (!await backend.ContainsPageAsync(htmlTestPage)) await backend.InsertPageAsync(htmlTestPage);
		}

		private static async Task<GenericPage> LoadSyntaxPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("Syntax.md");

			var content = await FileIO.ReadTextAsync(startFile);

			return new GenericPage(Path.ofLink("system:syntax"), content, ContentType.Markdown, clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
		}


		private static async Task<GenericPage> LoadStartPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("Start.md");

			var content = await FileIO.ReadTextAsync(startFile);

			return new GenericPage(Path.ofLink("start"), content, ContentType.Markdown, clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
		}

		private static async Task<GenericPage> LoadExamplePage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("Example.md");

			var content = await FileIO.ReadTextAsync(startFile);

			return new GenericPage(Path.ofLink("example"), content, ContentType.Markdown, clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
		}

		private static async Task<GenericPage> LoadHtmlTestPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("HtmlTest.html");

			var content = await FileIO.ReadTextAsync(startFile);

			return new GenericPage(Path.ofLink("system:htmltest"), content, ContentType.Html, clock.GetCurrentInstant(), clock.GetCurrentInstant(), true);
		}
	}
}