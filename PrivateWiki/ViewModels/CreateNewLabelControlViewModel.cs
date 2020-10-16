using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.Backends;
using PrivateWiki.Utilities;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace PrivateWiki.ViewModels
{
	public class CreateNewLabelControlViewModel : ReactiveObject, IValidatableViewModel
	{
		private readonly ILabelBackend _labelBackend;

		public ValidationContext ValidationContext { get; } = new ValidationContext();

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

		private readonly ISubject<Unit> _onCancel;

		public IObservable<Unit> OnCancel => _onCancel;

		public ValidationHelper ValidLabelRule { get; }

		public CreateNewLabelControlViewModel()
		{
			_labelBackend = Application.Instance.Container.GetInstance<ILabelBackend>();

			CreateLabel = ReactiveCommand.CreateFromTask(x => CreateLabelAsync(), this.IsValid());
			Cancel = ReactiveCommand.CreateFromTask(CancelAsync);

			ValidLabelRule = this.ValidationRule(
				vm => vm.ScopedLabelValue,
				label => !string.IsNullOrWhiteSpace(label),
				"Label must not be null or whitespace");

			_onCancel = new Subject<Unit>();

			this.WhenAnyValue(x => x.ColorHexString)
				.Select(x => x.HexToColor())
				.ToProperty(this, x => x.Color, out _color);
		}

		private async Task CreateLabelAsync()
		{
			var label = new Label(ScopedLabelValue, Description, Color);

			await _labelBackend.InsertLabelAsync(label);

			_onCancel.OnNext(Unit.Default);
		}

		private Task CancelAsync()
		{
			// TODO Cancel
			_onCancel.OnNext(Unit.Default);

			return Task.CompletedTask;
		}
	}
}