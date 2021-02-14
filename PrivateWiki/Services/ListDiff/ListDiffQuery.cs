using System.Collections.Generic;
using Dawn;
using PrivateWiki.Core;

namespace PrivateWiki.Services.ListDiff
{
	public class ListDiffQuery<T> : IQuery<ListDiffResult<T>>
	{
		public IEnumerable<T> List1 { get; }

		public IEnumerable<T> List2 { get; }

		public IEqualityComparer<T> EqualityComparer { get; } = EqualityComparer<T>.Default;

		public ListDiffQuery(IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T>? comparer = null)
		{
			Guard.Argument(list1, nameof(list1)).NotNull();
			Guard.Argument(list2, nameof(list2)).NotNull();

			List1 = list1;
			List2 = list2;

			if (comparer is not null) EqualityComparer = comparer;
		}
	}
}