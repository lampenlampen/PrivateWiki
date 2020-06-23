using System.Threading.Tasks;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public interface IMarkdownRenderingSettingsSerializer
	{
		public Task<bool> SaveMarkdownRenderingsSettings(IMarkdownRenderingSettingsService settings);
	}
}