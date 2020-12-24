using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace PrivateWiki.ViewModels.PageEditors
{
	public class HtmlPageEditorControlViewModel : PageEditorControlViewModelBase
	{
		private HtmlPageEditorControlPivotItem _currentPivotItem = HtmlPageEditorControlPivotItem.Editor;

		public HtmlPageEditorControlPivotItem CurrentPivotItem
		{
			get => _currentPivotItem;
			set => this.RaiseAndSetIfChanged(ref _currentPivotItem, value);
		}

		public HtmlPageEditorControlViewModel()
		{
			this.WhenAnyValue(x => x.Page)
				.Where(x => x != null)
				.Subscribe(x => Content = x.Content);
		}

		private protected override async Task SavePageAsync()
		{
			if (Content != Page.Content)
			{
				var newPage = Page with {Content = Content};
				_onSavePage.OnNext(newPage);
			}
			else
			{
				// TODO Save, although nothing changed
			}
		}
	}

	public enum HtmlPageEditorControlPivotItem
	{
		Editor,
		Preview,
		Metadata
	}
}