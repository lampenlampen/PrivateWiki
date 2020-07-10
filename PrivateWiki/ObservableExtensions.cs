using System;
using System.Reactive.Linq;

namespace PrivateWiki
{
	public static class ObservableExtensions
	{
		public static IObservable<T> WhereNotNull<T>(this IObservable<T?> source) where T : class
		{
			return source.Where(x => x != null)!;
		}
	}
}