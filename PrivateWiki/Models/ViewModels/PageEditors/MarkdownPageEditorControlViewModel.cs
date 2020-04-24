using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PrivateWiki.Renderer;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels.PageEditors
{
	public class MarkdownPageEditorControlViewModel : PageEditorControlViewModelBase
	{
		private MarkdownPageEditorControlPivotItem _currentPivotItem = MarkdownPageEditorControlPivotItem.Editor;

		private readonly ContentRenderer _renderer;

		public MarkdownPageEditorControlPivotItem CurrentPivotItem
		{
			get => _currentPivotItem;
			set => this.RaiseAndSetIfChanged(ref _currentPivotItem, value);
		}

		private string _previewContent = string.Empty;

		public string PreviewContent
		{
			get => _previewContent;
			private set => this.RaiseAndSetIfChanged(ref _previewContent, value);
		}

		private string _htmlPreviewContent = string.Empty;

		public string HtmlPreviewContent
		{
			get => _htmlPreviewContent;
			private set => this.RaiseAndSetIfChanged(ref _htmlPreviewContent, value);
		}

		public MarkdownPageEditorControlViewModel()
		{
			_renderer = new ContentRenderer();

			this.WhenAnyValue(x => x.Page)
				.Where(x => x != null)
				.Subscribe(x => Content = x.Content);

			this.WhenAnyValue(x => x.CurrentPivotItem)
				.Subscribe(x => PivotSelectionChanged(x));
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

		private async Task PivotSelectionChanged(MarkdownPageEditorControlPivotItem item)
		{
			string content;
			switch (item)
			{
				case MarkdownPageEditorControlPivotItem.Editor:
					break;
				case MarkdownPageEditorControlPivotItem.Preview:
					content = await _renderer.RenderContentAsync(Content, "markdown");
					PreviewContent = content;
					break;
				case MarkdownPageEditorControlPivotItem.HtmlPreview:
					content = await _renderer.RenderContentAsync(Content, "markdown");
					HtmlPreviewContent = content;
					break;
				case MarkdownPageEditorControlPivotItem.Metadata:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(item), item, null);
			}
		}
	}

	public enum MarkdownPageEditorControlPivotItem
	{
		Editor,
		Preview,
		HtmlPreview,
		Metadata
	}
}