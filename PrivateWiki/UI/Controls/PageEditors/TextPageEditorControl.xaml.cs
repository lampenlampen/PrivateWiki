using System;
using System.Reactive.Disposables;
using Windows.UI.Xaml.Controls;
using NLog;
using PrivateWiki.Models.ViewModels;
using PrivateWiki.Models.ViewModels.PageEditors;
using PrivateWiki.Renderer;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Controls.PageEditors
{
	public class TextPageEditorControlBase : ReactiveUserControl<TextPageEditorControlViewModel>
	{
	}

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TextPageEditorControl : TextPageEditorControlBase
	{
		private readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public TextPageEditorControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				commandBar.ViewModel = (PageEditorCommandBarViewModel) ViewModel.CommandBarViewModel;

				this.Bind(ViewModel,
						vm => vm.Content,
						view => view.PageEditorTextBox.Text)
					.DisposeWith(disposable);

				Pivot.Events().SelectionChanged
					.Subscribe(_ => ViewModel.CurrentPivotItem = SelectedItemToPivotItemConverter(Pivot.SelectedItem))
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.CurrentPivotItem).Subscribe(OnPivotSelectionChanged)
					.DisposeWith(disposable);
			});
		}

		private TextPageEditorControlPivotItem SelectedItemToPivotItemConverter(object selectedItem)
		{
			var selectedPivotItem = (PivotItem) selectedItem;

			switch (selectedPivotItem.Name)
			{
				case "EditorPivotItem":
					return TextPageEditorControlPivotItem.Editor;
				case "MetadataPivotItem":
					return TextPageEditorControlPivotItem.Metadata;
				default:
					throw new ArgumentOutOfRangeException(nameof(selectedItem), selectedItem, $"PivotItem {selectedPivotItem.Name} not handled!");
			}
		}

		private void OnPivotSelectionChanged(TextPageEditorControlPivotItem item)
		{
			ContentRenderer renderer = new ContentRenderer();
			string content;
			switch (item)
			{
				case TextPageEditorControlPivotItem.Editor:
					break;
				case TextPageEditorControlPivotItem.Metadata:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(item), item, null);
			}
		}
	}
}