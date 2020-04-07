using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using ColorCode.Compilation.Languages;
using Models.Pages;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Storage;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using StorageBackend.SQLite;

namespace PrivateWiki.Models.ViewModels
{
	public class NewPageViewModel : ReactiveObject, IValidatableViewModel
	{
		public ValidationContext ValidationContext { get; } = new ValidationContext();

		private readonly IClock _clock;

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

		public ContentType2? ContentType22 = null;

		public IReadOnlyCollection<string> ContentTypes { get; } = new ReadOnlyCollection<string>(AppConfig.SupportedContentTypes);
		public IReadOnlyCollection<ContentType2> ContentTypes2 { get; } = new ReadOnlyCollection<ContentType2>(AppConfig.SupportedContentTypes2);

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
			_clock = SystemClock.Instance;
			
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

		private async Task<Unit> ImportPageAsync()
		{
			async Task<Unit> Action()
			{
				var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), _clock);

				var file = await FileSystemAccess.PickFileAsync();

				if (file == null) return Unit.Default;

				var content = FileIO.ReadTextAsync(file);

				var now = _clock.GetCurrentInstant();

				var page = new GenericPage(Link, await content, ContentType, now, now, false);

				await backend.InsertPageAsync(page);
				
				_onImportNewPage.OnNext(Unit.Default);

				return Unit.Default;
			}
			
			// TODO Import Page

			return await Action();
		}

		private Task CreateNewPageAsync()
		{
			return Task.Run(async () =>
			{
				var now = _clock.GetCurrentInstant();
			
				var page = new GenericPage(Link, "", ContentType, now, now, false);
			
				var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), _clock);
				await backend.InsertPageAsync(page);
			
				_onCreateNewPage.OnNext(Unit.Default);
			});
		}

		private Task GoBackAsync()
		{
			_onGoBack.OnNext(Unit.Default);
			return Task.CompletedTask;
		}
	}
}