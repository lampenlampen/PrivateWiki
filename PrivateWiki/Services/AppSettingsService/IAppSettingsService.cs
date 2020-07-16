using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;

namespace PrivateWiki.Services.AppSettingsService
{
	public interface IAppSettingsService
	{
		IMarkdownRenderingSettingsService MarkdownRenderingSettings { get; }

		IFeatureFlagsService FeatureFlags { get; }
	}
}