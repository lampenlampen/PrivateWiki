using System.Collections.Generic;
using System.Linq;
using Dawn;
using PrivateWiki.Core;

namespace PrivateWiki.Services.ListDiff
{
	public class ListDiff<T> : IQueryHandler<ListDiffQuery<T>, ListDiffResult<T>>
	{
		public ListDiffResult<T> Handle(ListDiffQuery<T> query)
		{
			Guard.Argument(query, nameof(query)).NotNull();

			return DiffLists(query.List1.ToList(), query.List2.ToList(), query.EqualityComparer);
		}

		private ListDiffResult<T> DiffLists(IList<T> list1, IList<T> list2, IEqualityComparer<T> comparer)
		{
			var addedElements = new List<T>(list2);
			var removedElements = new List<T>(list1);

			foreach (var element in list2)
			{
				if (list1.Contains(element, comparer))
				{
					addedElements.Remove(element);
				}
			}

			foreach (var element in list1)
			{
				if (list2.Contains(element, comparer))
				{
					removedElements.Remove(element);
				}
			}

			return new ListDiffResult<T>(addedElements, removedElements);
		}
	}
}