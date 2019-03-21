using System;
using System.Reflection;
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

			var startPage = LoadStartPage();
			if (!dataAccess.ContainsPage(startPage))
			{
				dataAccess.InsertPage(LoadStartPage());
			}
		}

		public static async Task<PageModel> LoadSyntaxPage()
		{
			var defaultPagesDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\DefaultPages");

			var syntaxFile = await defaultPagesDir.GetFileAsync("Syntax.md");

			var content = await FileIO.ReadTextAsync(syntaxFile);

			return new PageModel(Guid.NewGuid(), "syntax", content, SystemClock.Instance);
		}

		public static PageModel LoadStartPage()
		{
			return new PageModel(Guid.NewGuid(), "start", InitPages.GetStartPageString(), SystemClock.Instance);
		}
	}
}