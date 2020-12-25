using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace PrivateWiki.UWP.Utilities
{
	public class SelectableItemsSource<T> : ReadOnlyObservableCollection<T>, ISelectionInfo where T : ISelectable
	{
		public SelectableItemsSource(ObservableCollection<T> listImplementation) : base(listImplementation) { }

		public void SelectRange(ItemIndexRange itemIndexRange)
		{
			for (int i = itemIndexRange.FirstIndex; i <= itemIndexRange.LastIndex; i++)
			{
				Items[i].IsSelected = true;
			}
		}

		public void DeselectRange(ItemIndexRange itemIndexRange)
		{
			for (int i = itemIndexRange.FirstIndex; i <= itemIndexRange.LastIndex; i++)
			{
				Items[i].IsSelected = false;
			}
		}

		public bool IsSelected(int index)
		{
			if (index < 0 || index >= Items.Count) return false;

			return Items[index].IsSelected;
		}

		public IReadOnlyList<ItemIndexRange> GetSelectedRanges()
		{
			var list = new List<ItemIndexRange>();

			int startIndex = -1;
			int lastIndex = -1;

			for (int i = 0; i < Items.Count; i++)
			{
				var label = Items[i];

				if (label.IsSelected)
				{
					if (startIndex == -1)
					{
						startIndex = i;
						lastIndex = -1;
					}
					else
					{
						lastIndex = i;
					}
				}
				else
				{
					if (startIndex == -1) { }
					else
					{
						list.Add(new ItemIndexRange(startIndex, (uint) (lastIndex - startIndex + 1)));
					}
				}
			}

			return list;
		}
	}
}