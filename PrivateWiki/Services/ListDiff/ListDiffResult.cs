using System.Collections.Generic;
using PrivateWiki.Core;

namespace PrivateWiki.Services.ListDiff
{
	public class ListDiffResult<T> : IQuery<ListDiffResult<T>>
	{
		public IReadOnlyCollection<T> AddedElements { get; init; }

		public IReadOnlyCollection<T> RemovedElements { get; init; }

		public ListDiffResult(IReadOnlyCollection<T> addedElements, IReadOnlyCollection<T> removedElements)
		{
			AddedElements = addedElements;
			RemovedElements = removedElements;
		}
	}
}