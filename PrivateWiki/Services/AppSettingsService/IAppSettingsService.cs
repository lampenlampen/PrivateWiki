using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;

namespace PrivateWiki.Services.AppSettingsService
{
	public interface IAppSettingsService
	{
		IMarkdownRenderingSettingsService MarkdownRenderingSettings { get; }
	}
}