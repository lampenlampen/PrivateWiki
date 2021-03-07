using System;
using System.Linq;
using Windows.UI.Core;

namespace PrivateWiki.UWP.UI.Utilities.ExtensionFunctions
{
	public static class LinqExtensions
	{
		public static void RunOnUIThread<TSource>(this ParallelQuery<TSource> source, Action<TSource> action)
		{
			source.ForAll(async x => { await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action.Invoke(x); }); });
		}
	}
}