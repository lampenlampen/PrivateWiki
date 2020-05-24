using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels.PageEditors
{
	public class TextPageEditorControlViewModel : PageEditorControlViewModelBase
	{
		private TextPageEditorControlPivotItem _currentPivotItem = TextPageEditorControlPivotItem.Editor;

		public TextPageEditorControlPivotItem CurrentPivotItem
		{
			get => _currentPivotItem;
			set => this.RaiseAndSetIfChanged(ref _currentPivotItem, value);
		}

		public TextPageEditorControlViewModel()
		{
			this.WhenAnyValue(x => x.Page)
				.Where(x => x != null)
				.Subscribe(x => Content = x.Content);
		}

		private protected override async Task SavePageAsync()
		{
			if (Content != Page.Content)
			{
				var newPage = Page.Clone(content: Content);
				_onSavePage.OnNext(newPage);
			}
			else
			{
				// TODO Save, although nothing changed
			}
		}
	}

	public enum TextPageEditorControlPivotItem
	{
		Editor,
		Metadata
	}
}