using System;
using FluentAssertions;
using PrivateWiki.DataModels.Pages;
using Xunit;

namespace PrivateWiki.Test.DataModels
{
	public class LabelTest
	{
		[Fact]
		public void LabelValueEqualityTest()
		{
			var id1 = Guid.NewGuid();

			var label1 = new Label(id1, "key1", "value1", "description1", new Color(2, 2, 2));
			var label2 = new Label(id1, "key1", "value1", "description1", new Color(2, 2, 2));

			label2.Should().Be(label1);
			label2.Should().BeEquivalentTo(label1);
		}

		[Fact]
		public void LabelValueInequalityTest1()
		{
			var id1 = Guid.NewGuid();

			var label1 = new Label(id1, "key1", "value1", "description1", new Color(2, 2, 2));
			var label2 = new Label(Guid.NewGuid(), "key1", "value1", "description1", new Color(2, 2, 2));

			label2.Should().NotBe(label1);
			label2.Should().NotBeEquivalentTo(label1);
		}

		[Fact]
		public void LabelValueInequalityTest2()
		{
			var id1 = Guid.NewGuid();

			var label1 = new Label(id1, "key1", "value1", "description1", new Color(2, 2, 2));
			var label2 = new Label(id1, "key2", "value1", "description1", new Color(2, 2, 2));

			label2.Should().NotBe(label1);
			label2.Should().NotBeEquivalentTo(label1);
		}

		[Fact]
		public void ConstructorsShouldProduceSameLabelsTest()
		{
			var id = Guid.NewGuid();
			const string key = "key";
			const string value = "value";
			const string description = "description";
			var color = new Color(2, 2, 2);

			var label1 = new Label(id, key, value, description, color);
			var label2 = new Label(id, $"{key}::{value}", description, color);

			label2.Should().Be(label1);
			label2.Should().BeEquivalentTo(label1);
		}
	}
}