using System;
using System.Collections.Generic;
using FluentAssertions;
using PrivateWiki.Services.ListDiff;
using Xunit;

namespace PrivateWiki.Test.Services.ListDiff
{
	public class ListDiffQueryTest
	{
		[Fact]
		public void NullListThrows()
		{
			IEnumerable<Guid>? nullList = null;
			IEnumerable<Guid> list = new List<Guid>();

			Action act = () => new ListDiffQuery<Guid>(nullList!, list);

			act.Should().Throw<ArgumentNullException>();
		}
	}
}