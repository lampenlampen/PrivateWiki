using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Toolkit.Uwp.UI.Extensions;
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
					.BindTo(AddLabelBox, x => x.ItemsSource)
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.SelectedLabels)
					.Do(x =>
					{
						AddLabelBox.DeselectAll();
						AddLabelBox.SelectedItems.Add(x);
					})
					.Subscribe()
					.DisposeWith(disposable);

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
						var vm = ViewModel;

						AddLabelBox.SelectedItems.Add(vm.SelectedLabels.First());
					})
					.InvokeCommand(ViewModel, vm => vm.ManageLabels)
					.DisposeWith(disposable);
			});
		}
	}
}