using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PrivateWiki.Models.Pages;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels.PageEditors
{
	public class MarkdownPageEditorControlViewModel : PageEditorControlViewModelBase
	{
		public MarkdownPageEditorControlViewModel()
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
}