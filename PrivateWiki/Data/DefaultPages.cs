using System;
using System.Threading.Tasks;
using Windows.Storage;
using DataAccessLibrary;
using NodaTime;
using PrivateWiki.Data.DataAccess;

namespace PrivateWiki.Data
{
	public static class DefaultPages
	{
		public static async void InsertDefaultPages()
		{
			var dataAccess = new DataAccessImpl();

			var syntaxPage = await LoadSyntaxPage();
			if (!dataAccess.ContainsPage(syntaxPage))
			{
				dataAccess.InsertPage(await LoadSyntaxPage());
			}

			var startPage = await LoadStartPage();
			if (!dataAccess.ContainsPage(startPage))
			{
				dataAccess.InsertPage(startPage);
			}

			var testPage = await LoadExamplePage();
			if (!dataAccess.ContainsPage(testPage))
			{
				dataAccess.InsertPage(testPage);
			}
		}

		private static async Task<PageModel> LoadSyntaxPage()
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var syntaxFile = await defaultPagesDir.GetFileAsync("Syntax.md");

			var content = await FileIO.ReadTextAsync(syntaxFile);

			return new PageModel(Guid.NewGuid(), "syntax", content, SystemClock.Instance);
		}

		private static async Task<PageModel> LoadStartPage()
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var startFile = await defaultPagesDir.GetFileAsync("Start.md");

			var content = await FileIO.ReadTextAsync(startFile);

			return new PageModel(Guid.NewGuid(), "start", content, SystemClock.Instance);
		}

		private static async Task<PageModel> LoadExamplePage()
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var exampleFile = await defaultPagesDir.GetFileAsync("Example.md");

			var content = await FileIO.ReadTextAsync(exampleFile);

			return new PageModel(Guid.NewGuid(), "test", content, SystemClock.Instance);
		}
	}
}