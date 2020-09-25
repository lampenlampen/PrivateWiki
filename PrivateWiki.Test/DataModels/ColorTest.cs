using FluentAssertions;
using PrivateWiki.DataModels.Pages;
using Xunit;

namespace PrivateWiki.Test.DataModels
{
	public class ColorTest
	{
		[Fact]
		public void ColorValueEqualityTest()
		{
			var color1 = new Color(2, 2, 2);
			var color2 = new Color(2, 2, 2);

			color2.Should().Be(color1);
			color2.Should().BeEquivalentTo(color1);
		}

		[Fact]
		public void ColorValueInequalityTest()
		{
			var color1 = new Color(2, 2, 2);
			var color2 = new Color(2, 2, 3);

			color2.Should().NotBe(color1);
			color2.Should().NotBeEquivalentTo(color1);
		}

		[Fact]
		public void Test()
		{
			var color1 = new Color(2, 2, 2);
			var color2 = new Color(2, 2, 3);

			color2.Should().NotBe(color1);
			color2.Should().NotBeEquivalentTo(color1);
		}
	}
}