using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NLog;
using PrivateWiki.Models.Pages;
using PrivateWiki.StorageBackend;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
{
	public class GenericTextPageEditorViewModel : ReactiveObject
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private IGenericPageStorage _backend;

		private GenericPage _page;

		public GenericPage Page
		{
			get => _page;
			set => this.RaiseAndSetIfChanged(ref _page, value);
		}

		private ISubject<Unit> _goBack;

		public IObservable<Unit> GoBack => _goBack;

		public readonly ReactiveCommand<string, Unit> LoadPage;

		public readonly ReactiveCommand<string, Unit> SavePage;

		public readonly ReactiveCommand<Unit, Unit> Abort;

		public readonly ReactiveCommand<Unit, Unit> DeletePage;

		public readonly ReactiveCommand<Unit, Unit> ExternalEditor;

		private readonly Interaction<Unit, bool> confirmDelete;

		public Interaction<Unit, bool> ConfirmDelete => confirmDelete;

		public GenericTextPageEditorViewModel()
		{
			_goBack = new Subject<Unit>();

			// Init Commands
			LoadPage = ReactiveCommand.CreateFromTask<string>(LoadPageAsync);

			SavePage = ReactiveCommand.Create<string>((content) =>
			{
				// TODO Save Page
				Logger.Info($"Save Page: {Page.Link}");
			});

			Abort = ReactiveCommand.Create(() =>
			{
				// TODO Abort
				Logger.Info("Abort");

				_goBack.OnNext(Unit.Default);
			});

			ExternalEditor = ReactiveCommand.Create(() =>
			{
				// TODO External Editor
				Logger.Info("External Editor");
			});

			DeletePage = ReactiveCommand.CreateFromTask(DeletePageAsync);


			// Init Interactions
			confirmDelete = new Interaction<Unit, bool>();

			_backend = new StorageBackend.StorageBackend(Application.Instance.StorageBackend);
		}

		private async Task DeletePageAsync()
		{
			var confirmation = await confirmDelete.Handle(Unit.Default);

			// Delete Page
			Logger.Info($"Delete page: {Page.Link}");

			if (Page.IsLocked)
			{
				// TODO Page is Locked and cannot be deleted.
			}
			else
			{
				_backend.DeletePageAsync(Page.Id);
				_goBack.OnNext(Unit.Default);
			}
		}

		public async Task LoadPageAsync(string pageLink)
		{
			var page = await _backend.GetPageAsync(pageLink);
			Page = page;
		}
	}
}