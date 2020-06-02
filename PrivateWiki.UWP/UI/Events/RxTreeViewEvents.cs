using System;
using System.Reactive.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;

namespace PrivateWiki.UWP.UI.Events
{
	public class RxTreeViewEvents
	{
		private readonly TreeView _data;

		public RxTreeViewEvents(TreeView data)
		{
			_data = data;
		}

		public IObservable<TreeViewItemInvokedEventArgs> ItemInvoked => Observable.FromEvent<TypedEventHandler<TreeView, TreeViewItemInvokedEventArgs>, TreeViewItemInvokedEventArgs>(eventHandler =>
		{
			void Handler(object sender, TreeViewItemInvokedEventArgs e) => eventHandler(e);
			return Handler;
		}, x => _data.ItemInvoked += x, x => _data.ItemInvoked -= x);
	}

	public static class TreeViewEventsExtension
	{
		public static RxTreeViewEvents Events(this TreeView item) => new RxTreeViewEvents(item);
	}
}