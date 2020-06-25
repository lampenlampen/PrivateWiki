using PrivateWiki.Services.AppSettingsService.KeyValueCaches;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using PrivateWiki.Services.SqliteStorage;
using Xunit;
using Xunit.Abstractions;

namespace PrivateWiki.Test
{
	public class UnitTest1
	{
		private readonly ITestOutputHelper output;

		public UnitTest1(ITestOutputHelper output)
		{
			this.output = output;
		}
		
	}
}