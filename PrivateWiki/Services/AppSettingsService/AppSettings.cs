using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using SimpleInjector;

namespace PrivateWiki.Services.AppSettingsService
{
	public class AppSettings : IAppSettingsService
	{
		private readonly Container _container;
		
		public IRenderingMarkdownSettingsService RenderingMarkdownSettings => _container.GetInstance<IRenderingMarkdownSettingsService>();

		public IFeatureFlagsService FeatureFlags => _container.GetInstance<IFeatureFlagsService>();


		public AppSettings(Container container)
		{
			_container = container;
		}
	}
}