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

		public IObservable<TreeViewExpandingEventArgs> Expanding => Observable.FromEvent<TypedEventHandler<TreeView, TreeViewExpandingEventArgs>, TreeViewExpandingEventArgs>(eventHandler =>
		{
			void Handler(object sender, TreeViewExpandingEventArgs e) => eventHandler(e);
			return Handler;
		}, x => _data.Expanding += x, x => _data.Expanding -= x);
		
		public IObservable<TreeViewCollapsedEventArgs> Collapsed => Observable.FromEvent<TypedEventHandler<TreeView, TreeViewCollapsedEventArgs>, TreeViewCollapsedEventArgs>(eventHandler =>
		{
			void Handler(object sender, TreeViewCollapsedEventArgs e) => eventHandler(e);
			return Handler;
		}, x => _data.Collapsed += x, x => _data.Collapsed -= x);
	}

	public static class TreeViewEventsExtension
	{
		public static RxTreeViewEvents Events(this TreeView item) => new RxTreeViewEvents(item);
	}
}