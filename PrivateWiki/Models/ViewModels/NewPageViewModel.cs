using System;
using System.Globalization;
using System.Reactive;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Models.Pages;
using PrivateWiki.Data;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace PrivateWiki.Models.ViewModels
{
	public class NewPageViewModel : ReactiveObject, IValidatableViewModel
	{
		public ValidationContext ValidationContext { get; } = new ValidationContext();


		public Path Link => Path.ofLink(LinkString);

		public string LinkString
		{
			get => _linkString;
			set => this.RaiseAndSetIfChanged(ref _linkString, value);
		}

		private string _linkString = string.Empty;

		public ValidationHelper LinkValidationRule { get; }

		public string ContentType
		{
			get => _contentType;
			set => this.RaiseAndSetIfChanged(ref _contentType, value);
		}

		private string _contentType = string.Empty;

		private ContentType? ContentType2 = null;

		public string[] ContentTypes { get; } = AppConfig.SupportedContentTypes;

		public ValidationHelper ContentTypeValidationRule { get; }

		public ReactiveCommand<Unit, Unit> ImportPage { get; }

		public ReactiveCommand<Unit, Unit> CreateNewPage { get; }

		public ReactiveCommand<Unit, Unit> GoBack { get; }

		public IObservable<Unit> OnImportNewPage => _onImportNewPage;
		private readonly ISubject<Unit> _onImportNewPage;

		public IObservable<Unit> OnCreateNewPage => _onCreateNewPage;
		private readonly ISubject<Unit> _onCreateNewPage;

		public IObservable<Unit> OnGoBack => _onGoBack;
		private readonly ISubject<Unit> _onGoBack;

		public NewPageViewModel()
		{
			LinkValidationRule = this.ValidationRule(
				vm => vm.LinkString,
				link => !string.IsNullOrEmpty(link),
				link => $"{LinkString} must be non empty!");

			ContentTypeValidationRule = this.ValidationRule(
				vm => vm.ContentType,
				contentType => Enum.TryParse<ContentType>(contentType, true, out _),
				contentType => "ContentType must be \"markdown\" or \"Html\"");

			var isCreateAndImportEnabled = this.IsValid();

			ImportPage = ReactiveCommand.CreateFromTask(ImportPageAsync, isCreateAndImportEnabled);
			CreateNewPage = ReactiveCommand.CreateFromTask(CreateNewPageAsync, isCreateAndImportEnabled);
			GoBack = ReactiveCommand.CreateFromTask(GoBackAsync);

			_onImportNewPage = new Subject<Unit>();
			_onCreateNewPage = new Subject<Unit>();
			_onGoBack = new Subject<Unit>();
		}

		private Task ImportPageAsync()
		{
			// TODO Import Page
			return Task.CompletedTask;
		}

		private Task CreateNewPageAsync()
		{
			// TODO Create new Page
			return Task.CompletedTask;
		}

		private Task GoBackAsync()
		{
			_onGoBack.OnNext(Unit.Default);
			return Task.CompletedTask;
		}
	}
}