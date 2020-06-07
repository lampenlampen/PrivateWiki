using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PrivateWiki.Services.PackageService
{
	public class AssetsService : IAssetsService
	{
		public async Task<string> GetAssetsFileContent(string relativePath)
		{
			var assembly = typeof(AssetsService).GetTypeInfo().Assembly;

			var stream = assembly.GetManifestResourceStream($"PrivateWiki.Assets.{relativePath}");

			using var sr = new StreamReader(stream);

			var content = await sr.ReadToEndAsync();

			return content;
		}

		public Task<string> GetSyntaxPage() => GetAssetsFileContent("DefaultPages.Syntax.md");

		public Task<string> GetStartPage() => GetAssetsFileContent("DefaultPages.Start.md");

		public Task<string> GetMarkdownTestPage() => GetAssetsFileContent("DefaultPages.MarkdownTest.md");

		public Task<string> GetHtmlTestPage() => GetAssetsFileContent("DefaultPages.HtmlTest.html");

		public Task<string> GetTextTestPage() => GetAssetsFileContent("DefaultPages.TextTest.txt");
	}
}