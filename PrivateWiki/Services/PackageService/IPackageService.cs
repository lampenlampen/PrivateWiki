using System.Threading.Tasks;

namespace PrivateWiki.Services.PackageService
{
	public interface IAssetsService
	{
		public Task<string> GetAssetsFileContent(string relativePath);

		public Task<string> GetSyntaxPage();

		public Task<string> GetStartPage();

		public Task<string> GetMarkdownTestPage();

		public Task<string> GetHtmlTestPage();

		public Task<string> GetTextTestPage();
	}
}