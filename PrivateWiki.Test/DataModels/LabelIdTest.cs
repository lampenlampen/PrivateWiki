using System;
using FluentAssertions;
using PrivateWiki.DataModels.Pages;
using Xunit;

namespace PrivateWiki.Test.DataModels
{
	public class LabelIdTest
	{
		[Fact]
		public void LabelIdValueEqualityTest()
		{
			var id1 = Guid.NewGuid();

			var label1 = new LabelId(id1);
			var label2 = new LabelId(id1);

			label2.Should().Be(label1);
			label2.Should().BeEquivalentTo(label1);
		}

		[Fact]
		public void LabelIdValueInequalityTest1()
		{
			var id1 = Guid.NewGuid();

			var label1 = new LabelId(id1);
			var label2 = new LabelId(Guid.NewGuid());

			label2.Should().NotBe(label1);
			label2.Should().NotBeEquivalentTo(label1);
		}
	}
}