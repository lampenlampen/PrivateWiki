using System.Threading.Tasks;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public class MarkdownRenderingSettingsSerializer : IMarkdownRenderingSettingsSerializer
	{
		
		
		public Task<bool> SaveMarkdownRenderingsSettings(IMarkdownRenderingSettingsService settings)
		{
			return Task.FromResult(false);
		}
	}
}