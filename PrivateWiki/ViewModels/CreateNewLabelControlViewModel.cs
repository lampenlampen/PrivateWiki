using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Utilities;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class CreateNewLabelControlViewModel : ReactiveObject
	{
		private readonly ObservableAsPropertyHelper<Color> _color;
		public Color Color => _color.Value;

		private string _colorHexString = "#428BCA";

		public string ColorHexString
		{
			get => _colorHexString;
			set => this.RaiseAndSetIfChanged(ref _colorHexString, value);
		}

		private string _scopedLabelValue = "";

		public string ScopedLabelValue
		{
			get => _scopedLabelValue;
			set => this.RaiseAndSetIfChanged(ref _scopedLabelValue, value);
		}

		private string _description = "";

		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}

		public ReactiveCommand<Unit, Unit> CreateLabel { get; }

		public ReactiveCommand<Unit, Unit> Cancel { get; }

		public CreateNewLabelControlViewModel()
		{
			CreateLabel = ReactiveCommand.CreateFromTask(CreateLabelAsync);
			Cancel = ReactiveCommand.CreateFromTask(CancelAsync);

			this.WhenAnyValue(x => x.ColorHexString)
				.Select(x => x.HexToColor())
				.ToProperty(this, x => x.Color, out _color);
		}

		private Task CreateLabelAsync()
		{
			var label = new Label(ScopedLabelValue, Description, Color);

			// TODO Create Label

			Application.Instance.GlobalNotificationManager.ShowNotImplementedNotification();
			return Task.CompletedTask;
		}

		private Task CancelAsync()
		{
			// TODO Cancel

			Application.Instance.GlobalNotificationManager.ShowNotImplementedNotification();
			return Task.CompletedTask;
		}
	}
}