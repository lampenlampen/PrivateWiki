using System;
using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using SimpleInjector;

namespace PrivateWiki.Services.AppSettingsService
{
	[Obsolete]
	public class AppSettings : IAppSettingsService
	{
		public Container Container { get; set; }

		public IRenderingMarkdownSettingsService RenderingMarkdownSettings => Container.GetInstance<IRenderingMarkdownSettingsService>();

		public IFeatureFlagsService FeatureFlags => Container.GetInstance<IFeatureFlagsService>();


		public AppSettings(Container container)
		{
			Container = container;
		}
	}
}