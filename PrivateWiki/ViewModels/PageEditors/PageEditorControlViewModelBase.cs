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
using PrivateWiki.Utilities;
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

		/// <summary>
		/// Cache with all labels
		/// </summary>
		private readonly ISourceCache<Label, Guid> _allLabels = new SourceCache<Label, Guid>(label => label.Id);

		/// <summary>
		/// List with all labels that can be added to the page.
		/// Consists of all labels except the ones already added to the page.
		/// </summary>
		public readonly ReadOnlyObservableCollection<Label> AddLabelsList;

		/// <summary>
		/// List with all labels of the page
		/// </summary>
		private readonly ISourceCache<Label, Guid> _pageLabels = new SourceCache<Label, Guid>(label => label.Id);

		/// <summary>
		/// List with all labels of the page for binding
		/// </summary>
		public readonly ReadOnlyObservableCollection<Label> PageLabels;

		private string _addLabelsQueryText = "";

		public string AddLabelsQueryText
		{
			get => _addLabelsQueryText;
			set => this.RaiseAndSetIfChanged(ref _addLabelsQueryText, value);
		}

		public IObservable<GenericPage> OnSavePage => _onSavePage;
		private protected readonly ISubject<GenericPage> _onSavePage;

		public IObservable<Unit> OnAbort { get; }
		public IObservable<Unit> OnOpenInExternalEditor { get; }
		public IObservable<Unit> OnDelete { get; }

		protected PageEditorControlViewModelBase()
		{
			CommandBarViewModel = new PageEditorCommandBarViewModel();

			SavePage = ReactiveCommand.CreateFromTask(SavePageAsync);
			RemoveLabel = ReactiveCommand.CreateFromTask<Label>(RemoveLabelAsync);
			AddLabel = ReactiveCommand.CreateFromTask<Label>(AddLabelAsync);
			AddLabels = ReactiveCommand.CreateFromTask<IEnumerable<Label>>(AddLabelsAsync);
			CreateNewLabel = ReactiveCommand.CreateFromTask(CreateNewLabelAsync);

			OnAbort = CommandBarViewModel.OnAbort.AsObservable();
			_onSavePage = new Subject<GenericPage>();

			CommandBarViewModel.OnSave.InvokeCommand(this, x => x.SavePage);
			OnOpenInExternalEditor = CommandBarViewModel.OnOpenInExternalEditor.AsObservable();
			OnDelete = CommandBarViewModel.OnDelete.AsObservable();

			this.WhenAnyValue(x => x.Page)
				.WhereNotNull()
				.Subscribe(x =>
				{
					_pageLabels.Clear();
					_pageLabels.Edit(updater => updater.AddOrUpdate(x.Labels));
				});

			_allLabels.Edit(updater => updater.AddOrUpdate(Label.GetTestData()));

			var a = this.WhenAnyValue(x => x.AddLabelsQueryText);
			var b = a.Select<string, Func<Label, bool>>(x =>
			{
				const StringComparison comp = StringComparison.OrdinalIgnoreCase;
				return label => label.Key.Contains(x, comp) || label.Description.Contains(x, comp) || label.Value is not null && label.Value.Contains(x, comp);
			});

			_allLabels.Connect()
				.Except(_pageLabels.Connect())
				.Filter(b)
				//.Filter(x => x.Key.Contains(AddLabelsQueryText) || x.Value.Contains(AddLabelsQueryText) || x.Description.Contains(AddLabelsQueryText))
				.Bind(out AddLabelsList)
				.Subscribe();

			_pageLabels.Connect()
				.Bind(out PageLabels)
				.Subscribe();
		}

		public ReactiveCommand<Unit, Unit> SavePage { get; }

		public ReactiveCommand<Label, Unit> RemoveLabel { get; }

		public ReactiveCommand<Label, Unit> AddLabel { get; }

		public ReactiveCommand<IEnumerable<Label>, Unit> AddLabels { get; }

		public ReactiveCommand<Unit, Unit> CreateNewLabel { get; }

		private protected abstract Task SavePageAsync();

		private Task RemoveLabelAsync(Label label)
		{
			_pageLabels.Remove(label);

			return Task.CompletedTask;
		}

		private Task AddLabelAsync(Label label)
		{
			_pageLabels.AddOrUpdate(label);

			return Task.CompletedTask;
		}

		private Task AddLabelsAsync(IEnumerable<Label> labels)
		{
			_pageLabels.Edit(updater => updater.AddOrUpdate(labels));

			return Task.CompletedTask;
		}

		private Task CreateNewLabelAsync()
		{
			return Task.CompletedTask;
		}
	}
}