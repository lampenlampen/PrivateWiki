using System.Threading.Tasks;

namespace PrivateWiki.Services.DefaultPagesService
{
	public interface IDefaultPagesService
	{
		Task<bool> InsertStartPage();

		Task<bool> InsertSyntaxPage();

		Task<bool> InsertMarkdownTestPage();

		Task<bool> InsertHtmlTestPage();

		Task<bool> InsertTextTestPage();

		Task<bool> InsertDefaultPages();
	}
}