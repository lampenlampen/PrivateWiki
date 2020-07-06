using System;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace PrivateWiki.UWP.UI.Events
{
	public class RxDataGridEvents : RxControlEvents
	{
		private readonly DataGrid _data;

		public RxDataGridEvents(DataGrid data) : base(data)
		{
			_data = data;
		}

		public IObservable<SelectionChangedEventArgs> SelectionChanged => Observable.FromEvent<SelectionChangedEventHandler, SelectionChangedEventArgs>(eventHandler =>
			{
				void Handler(object sender, SelectionChangedEventArgs e) => eventHandler(e);
				return Handler;
			}
			, x => _data.SelectionChanged += x, x => _data.SelectionChanged -= x);
	}

	public static class DataGridEventsExtension
	{
		public static RxDataGridEvents Events(this DataGrid item) => new RxDataGridEvents(item);
	}
}