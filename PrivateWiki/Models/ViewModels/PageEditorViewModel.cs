using System.Reactive;
using System.Threading.Tasks;
using NodaTime;
using PrivateWiki.Models.Pages;
using PrivateWiki.StorageBackend;
using PrivateWiki.StorageBackend.SQLite;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
{
	public class PageEditorViewModel : ReactiveObject
	{
		private GenericPage Page;

		public readonly ReactiveCommand<Path, Unit> ShowPage;

		public readonly ReactiveCommand<Unit, Unit> SavePage;

		public readonly ReactiveCommand<Unit, Unit> Abort;

		public readonly ReactiveCommand<Unit, Unit> OpenInExternalEditor;

		public PageEditorViewModel()
		{
			ShowPage = ReactiveCommand.CreateFromTask<Path>(ShowPageAsync);
			SavePage = ReactiveCommand.CreateFromTask(SavePageAsync);
			Abort = ReactiveCommand.CreateFromTask(AbortAsync);
			OpenInExternalEditor = ReactiveCommand.CreateFromTask(OpenInExternalEditorAsync);
		}

		private async Task ShowPageAsync(Path path)
		{
			var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			Page = await backend.GetPageAsync(path.FullPath);
		}

		private Task SavePageAsync()
		{
			return Task.CompletedTask;
		}

		private Task AbortAsync()
		{
			return Task.CompletedTask;
		}

		private Task OpenInExternalEditorAsync()
		{
			return Task.CompletedTask;
		}
	}
}