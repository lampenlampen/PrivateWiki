using NodaTime;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Contracts.Storage;
using Models.Pages;

namespace PrivateWiki.Data
{
	public static class DefaultPages
	{
		public static async void InsertDefaultMarkdownPagesAsync(IMarkdownPageStorage backend, IClock clock)
		{
			var syntaxPage = await LoadSyntaxPage(clock);
			if (!await backend.ContainsMarkdownPageAsync(syntaxPage.Link)) backend.InsertMarkdownPageAsync(syntaxPage);

			var startPage = await LoadStartPage(clock);
			if (!await backend.ContainsMarkdownPageAsync(startPage.Link)) backend.InsertMarkdownPageAsync(startPage);

			var testPage = await LoadExamplePage(clock);
			if (!await backend.ContainsMarkdownPageAsync(testPage.Link)) backend.InsertMarkdownPageAsync(testPage);
		}

		private static async Task<MarkdownPage> LoadSyntaxPage(IClock clock)
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var syntaxFile = await defaultPagesDir.GetFileAsync("Syntax.md");

			var content = await FileIO.ReadTextAsync(syntaxFile);

			return new MarkdownPage(Guid.NewGuid(), "syntax", content, clock.GetCurrentInstant(), clock.GetCurrentInstant(), true);
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
	}
}