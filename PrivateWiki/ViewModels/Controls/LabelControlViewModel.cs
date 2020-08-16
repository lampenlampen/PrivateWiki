using System.Reactive.Linq;
using PrivateWiki.DataModels.Pages;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Controls
{
	public class LabelControlViewModel : ReactiveObject
	{
		private Label _label;

		public Label Label
		{
			get => _label;
			set => this.RaiseAndSetIfChanged(ref _label, value);
		}

		public string? LabelText = null;

		public LabelControlViewModel()
		{
			this.WhenAnyValue(x => x.Label)
				.WhereNotNull()
				.Select(label => label.Value != null ? $"{label.Key} | {label.Value}" : $"{label.Key}")
				.BindTo(this, x => x.LabelText);
		}
	}
}