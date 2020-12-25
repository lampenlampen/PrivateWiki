using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using PrivateWiki.ViewModels.Controls;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls
{
	public class AddLabelsToPageControlBase : ReactiveUserControl<AddLabelsToPageControlViewModel> { }

	public sealed partial class AddLabelsToPageControl : AddLabelsToPageControlBase
	{
		public AddLabelsToPageControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				this.WhenAnyValue(x => x.ViewModel.AllLabelsSelectable)
					.WhereNotNull()
					.Select(x => new SelectableItemsSource(x))
					.BindTo(AddLabelBox, x => x.ItemsSource)
					.DisposeWith(disposable);

				/*
				this.WhenAnyValue(x => x.ViewModel.SelectedLabels)
					.Do(x =>
					{
						AddLabelBox.DeselectAll();
						AddLabelBox.SelectedItems.Add(x);
					})
					.Subscribe()
					.DisposeWith(disposable);
				*/


				this.Bind(ViewModel,
						vm => vm.AddLabelsQueryText,
						view => view.FilterQueryTextBox.Text)
					.DisposeWith(disposable);

				CreateNewLabelBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, vm => vm.CreateNewLabel)
					.DisposeWith(disposable);

				ManageLabelsBtn.Events().Click
					.Select(_ => Unit.Default)
					.Do(_ =>
					{
						var items = AddLabelBox.ItemsSource;
						var vm = ViewModel;
					})
					.InvokeCommand(ViewModel, vm => vm.ManageLabels)
					.DisposeWith(disposable);
			});
		}
	}

	public class SelectableItemsSource : ReadOnlyObservableCollection<SelectableLabel>, ISelectionInfo
	{
		private readonly IList<SelectableLabel> _listImplementation;

		public SelectableItemsSource(ObservableCollection<SelectableLabel> listImplementation) : base(listImplementation)
		{
			_listImplementation = listImplementation;
		}

		public void SelectRange(ItemIndexRange itemIndexRange)
		{
			for (int i = itemIndexRange.FirstIndex; i <= itemIndexRange.LastIndex; i++)
			{
				_listImplementation[i].IsSelected = true;
			}
		}

		public void DeselectRange(ItemIndexRange itemIndexRange)
		{
			for (int i = itemIndexRange.FirstIndex; i <= itemIndexRange.LastIndex; i++)
			{
				_listImplementation[i].IsSelected = false;
			}
		}

		public bool IsSelected(int index)
		{
			if (index < 0 || index >= _listImplementation.Count) return false;

			return _listImplementation[index].IsSelected;
		}

		public IReadOnlyList<ItemIndexRange> GetSelectedRanges()
		{
			var list = new List<ItemIndexRange>();

			int startIndex = -1;
			int lastIndex = -1;

			for (int i = 0; i < _listImplementation.Count; i++)
			{
				var label = _listImplementation[i];

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