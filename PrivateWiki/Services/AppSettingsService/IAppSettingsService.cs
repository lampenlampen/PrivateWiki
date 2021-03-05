using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using SimpleInjector;

namespace PrivateWiki.Services.AppSettingsService
{
	public interface IAppSettingsService
	{
		Container Container { get; set; }

		IRenderingMarkdownSettingsService RenderingMarkdownSettings { get; }

		IFeatureFlagsService FeatureFlags { get; }
	}
}