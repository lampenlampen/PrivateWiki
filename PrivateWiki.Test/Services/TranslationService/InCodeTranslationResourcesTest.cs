using System.Globalization;
using System.Threading;
using FluentAssertions;
using PrivateWiki.Services.TranslationService;
using Xunit;

namespace PrivateWiki.Test.Services.TranslationService
{
	public class InCodeTranslationResourcesTest
	{
		[Fact]
		public void Test()
		{
			TranslationResources resources = new InCodeTranslationResources();

			var actual = resources.TestTranslationString;

			actual.Should().BeSameAs("German Lorem Ipsum");
		}

		[Fact]
		public void Test2()
		{
			TranslationResources resources = new InCodeTranslationResources();

			var actual = resources.GetStringResource("test");

			actual.Should().BeSameAs("German Lorem Ipsum");
		}

		[Fact]
		public void Test3()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			TranslationResources resources = new InCodeTranslationResources();

			var actual = resources.GetStringResource("test");

			actual.Should().BeSameAs("Lorem Ipsum");
		}
	}
}