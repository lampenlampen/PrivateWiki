using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using PrivateWiki.Core;
using PrivateWiki.Services.ListDiff;
using Xunit;

namespace PrivateWiki.Test.Services.ListDiff
{
	public class ListDiffTestWithGuids
	{
		private readonly IQueryHandler<ListDiffQuery<Guid>, ListDiffResult<Guid>> _listDiff;

		public ListDiffTestWithGuids()
		{
			_listDiff = new ListDiff<Guid>();
		}

		[Fact]
		public void QueryNotNull()
		{
			Action act = () => _listDiff.Handle(null!);

			act.Should().Throw<ArgumentNullException>();
		}

		[Fact]
		public void SameListsProduceNoResults()
		{
			IEnumerable<Guid> list1 = new List<Guid>
			{
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid(),
			};

			var result = _listDiff.Handle(new ListDiffQuery<Guid>(list1, list1));

			using (new AssertionScope())
			{
				result.AddedElements.Should().BeEmpty();
				result.RemovedElements.Should().BeEmpty();
			}
		}

		[Fact]
		public void Test1()
		{
			IList<Guid> guids = new[]
			{
				Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
			};

			IEnumerable<Guid> list1 = new List<Guid>
			{
				guids[0], guids[1], guids[2]
			};

			IEnumerable<Guid> list2 = new List<Guid>
			{
				guids[4], guids[1], guids[3], guids[2]
			};

			var result = _listDiff.Handle(new ListDiffQuery<Guid>(list1, list2));

			using (new AssertionScope())
			{
				result.AddedElements.Should().BeEquivalentTo(new List<Guid> {guids[3], guids[4]});
				result.RemovedElements.Should().BeEquivalentTo(new List<Guid> {guids[0]});
			}
		}
	}
}