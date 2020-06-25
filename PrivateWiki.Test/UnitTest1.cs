using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using PrivateWiki.Services.KeyValueCaches;
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

		[Fact]
		public void Main()
		{
			var application = new Application();

			application.Initialize();

			var a = new MarkdownRenderingSettings(new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions {Path = "C:\\Users\\felix"})));

			a.IsHtmlEnabled = true;

			var b = a.IsHtmlEnabled;

			Assert.True(b);
		}
	}
}