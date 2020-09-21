using FluentAssertions;
using PrivateWiki.DataModels.Pages;
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
			var color1 = new Color(15, 15, 15);
			var color2 = new Color(15, 15, 15);
			
			color2.Should().BeEquivalentTo(color1);

		}
	}
}