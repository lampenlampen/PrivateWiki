using System.Globalization;
using System.Threading;
using FluentAssertions;
using PrivateWiki.Core.Logging;
using PrivateWiki.Services.Logger;
using PrivateWiki.Services.TranslationService;
using Xunit;

namespace PrivateWiki.Test.Services.TranslationService
{
	public class InCodeTranslationResourcesTest
	{
		[Fact]
		public void Test()
		{
			TranslationManager manager = new InCodeTranslationManager(new LogCmdHandler(new NullLogger()));

			var actual = manager.TestTranslationString;

			actual.Should().BeSameAs("German Lorem Ipsum");
		}

		[Fact]
		public void Test2()
		{
			TranslationManager manager = new InCodeTranslationManager(new LogCmdHandler(new NullLogger()));

			var actual = manager.GetStringResource("test");

			actual.Should().BeSameAs("German Lorem Ipsum");
		}

		[Fact]
		public void Test3()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			TranslationManager manager = new InCodeTranslationManager(new LogCmdHandler(new NullLogger()));

			var actual = manager.GetStringResource("test");

			actual.Should().BeSameAs("Lorem Ipsum");
		}
	}
}