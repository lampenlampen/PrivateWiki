using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;

namespace PrivateWiki.Services.AppSettingsService
{
	public interface IAppSettingsService
	{
		IRenderingMarkdownSettingsService RenderingMarkdownSettings { get; }

		IFeatureFlagsService FeatureFlags { get; }
	}
}