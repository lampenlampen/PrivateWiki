using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Windows.Storage;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Models.Pages;
using PrivateWiki.StorageBackend;
using PrivateWiki.StorageBackend.SQLite;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

#nullable enable

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


		public ContentType? ContentType
		{
			get => _contentType;
			set => this.RaiseAndSetIfChanged(ref _contentType, value);
		}

		private ContentType? _contentType;

		public IReadOnlyCollection<ContentType> SupportedContentTypes { get; } = new ReadOnlyCollection<ContentType>(AppConfig.SupportedContentTypes2);

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
				contentType => SupportedContentTypes.Contains(contentType),
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

				var file = await FileSystemAccess.PickFileAsync(ContentType.FileExtension);

				if (file == null) return Unit.Default;

				var content = FileIO.ReadTextAsync(file);

				var now = _clock.GetCurrentInstant();

				var page = new GenericPage(Link, await content, ContentType!.Name, now, now, false);

				await backend.InsertPageAsync(page);

				_onImportNewPage.OnNext(Unit.Default);

				return Unit.Default;
			}

			return await Action();
		}

		private Task CreateNewPageAsync()
		{
			return Task.Run(async () =>
			{
				var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), _clock);
				var a = await backend.ContainsPageAsync(LinkString);

				if (a)
				{
					App.Current.GlobalNotificationManager.ShowPageExistsNotificationOnUIThread();
				}
				else
				{
					var now = _clock.GetCurrentInstant();

					var page = new GenericPage(Link, "", ContentType!.Name, now, now, false);

					await backend.InsertPageAsync(page);

					_onCreateNewPage.OnNext(Unit.Default);
				}
			});
		}

		private Task GoBackAsync()
		{
			_onGoBack.OnNext(Unit.Default);
			return Task.CompletedTask;
		}
	}
}