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
		public void Main() { }
	}
}