using System;
using System.Reactive.Linq;

namespace PrivateWiki
{
	public static class ObservableExtensions
	{
		public static IObservable<T> WhereNotNull<T>(this IObservable<T> source) => source.Where(x => x != null);
	}
}