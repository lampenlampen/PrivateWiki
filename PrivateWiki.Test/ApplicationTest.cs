using NodaTime;
using Xunit;

namespace PrivateWiki.Test
{
	public class ApplicationTest
	{
		[Fact]
		public async void Test()
		{
			var application = Application.Instance;
			await application.Initialize();

			var clock = application.Container.GetInstance<IClock>();
		}
	}
}