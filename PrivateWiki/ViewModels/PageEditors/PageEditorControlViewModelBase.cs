using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using NLog;
using PrivateWiki.DataModels.Pages;
using ReactiveUI;

namespace PrivateWiki.ViewModels.PageEditors
{
	public abstract class PageEditorControlViewModelBase : ReactiveObject, IPageEditorControlViewModel
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private IPageEditorCommandBarViewModel _commandBarViewModel = null!;

		public IPageEditorCommandBarViewModel CommandBarViewModel
		{
			get => _commandBarViewModel;
			set => this.RaiseAndSetIfChanged(ref _commandBarViewModel, value);
		}

		private GenericPage _page;

		public GenericPage Page
		{
			get => _page;
			set => this.RaiseAndSetIfChanged(ref _page, value);
		}

		private string _content = string.Empty;

		public string Content
		{
			get => _content;
			set => this.RaiseAndSetIfChanged(ref _content, value);
		}

		private readonly IList<Label> _labels2 = new List<Label>();

		public readonly ObservableCollection<Label> Labels2 = new ObservableCollection<Label>();

		private readonly ISourceCache<Label, Guid> _allLabels2 = new SourceCache<Label, Guid>(label => label.Id);

		public readonly ReadOnlyObservableCollection<Label> AddLabelsList;

		private ISourceCache<Label, Guid> _pageLabels = new SourceCache<Label, Guid>(label => label.Id);

		public IObservable<GenericPage> OnSavePage => _onSavePage;
		private protected readonly ISubject<GenericPage> _onSavePage;

		public IObservable<Unit> OnAbort { get; }
		public IObservable<Unit> OnOpenInExternalEditor { get; }
		public IObservable<Unit> OnDelete { get; }

		protected PageEditorControlViewModelBase()
		{
			CommandBarViewModel = new PageEditorCommandBarViewModel();

			SavePage = ReactiveCommand.CreateFromTask(SavePageAsync);
			DeleteLabel = ReactiveCommand.CreateFromTask<Label>(DeleteLabelAsync);
			AddLabel = ReactiveCommand.CreateFromTask<Label>(AddLabelAsync);
			NewLabel = ReactiveCommand.CreateFromTask(NewLabelAsync);

			OnAbort = CommandBarViewModel.OnAbort.AsObservable();
			_onSavePage = new Subject<GenericPage>();

			CommandBarViewModel.OnSave.InvokeCommand(this, x => x.SavePage);
			OnOpenInExternalEditor = CommandBarViewModel.OnOpenInExternalEditor.AsObservable();
			OnDelete = CommandBarViewModel.OnDelete.AsObservable();

			this.WhenAnyValue(x => x.Page)
				.WhereNotNull()
				.Subscribe(x =>
				{
					_labels2.AddRange(x.Labels);
					_labels2.RemoveAt(0);

					_pageLabels.Clear();
					_pageLabels.Edit(updater => updater.AddOrUpdate(x.Labels));

					foreach (var label in x.Labels) Labels2.Add(label);
				});

			_allLabels2.Edit(updater => updater.AddOrUpdate(Label.GetTestData()));

			_allLabels2.Connect()
				.Except(_pageLabels.Connect())
				.Bind(out AddLabelsList)
				.Subscribe();
		}

		public ReactiveCommand<Unit, Unit> SavePage { get; }

		public ReactiveCommand<Label, Unit> DeleteLabel { get; }

		public ReactiveCommand<Label, Unit> AddLabel { get; }

		public ReactiveCommand<Unit, Unit> NewLabel { get; }

		private protected abstract Task SavePageAsync();

		private Task DeleteLabelAsync(Label label)
		{
			// TODO Delete Label
			Labels2.Remove(label);
			_pageLabels.Remove(label);

			return Task.CompletedTask;
		}

		private Task AddLabelAsync(Label label)
		{
			return Task.CompletedTask;
		}

		private Task NewLabelAsync()
		{
			return Task.CompletedTask;
		}
	}
}