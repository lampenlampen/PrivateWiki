using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Contracts.Storage;
using Models.Pages;
using Models.Storage;
using NLog;
using NodaTime;
using PrivateWiki.Storage;
using ReactiveUI;
using StorageBackend.SQLite;

namespace Models.ViewModels
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

		public ReactiveCommand<string, Unit> LoadPage;

		public ReactiveCommand<string, Unit> SavePage;

		public ReactiveCommand<Unit, Unit> Abort;

		public ReactiveCommand<Unit, Unit> DeletePage;

		public ReactiveCommand<Unit, Unit> ExternalEditor;

		private readonly Interaction<Unit, bool> confirmDelete;

		public Interaction<Unit, bool> ConfirmDelete => this.confirmDelete;

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
			
			_backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
		}

		public async Task DeletePageAsync()
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