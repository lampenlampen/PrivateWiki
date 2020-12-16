using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.Backends;
using PrivateWiki.Utilities;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Controls
{
	public class AddLabelsToPageControlViewModel : ReactiveObject
	{
		private readonly IPageLabelsBackend _pageLabelsBackend;
		private readonly ILabelBackend _labelBackend;

		/// <summary>
		/// Cache with all labels
		/// </summary>
		private readonly ISourceCache<Label, Guid> _allLabels = new SourceCache<Label, Guid>(label => label.Id);

		public readonly ReadOnlyObservableCollection<Label> AllLabels;

		/// <summary>
		/// List with all labels that can be added to the page.
		/// Consists of all labels except the ones already added to the page.
		/// </summary>
		public readonly ReadOnlyObservableCollection<Label> AddLabelsList;

		/// <summary>
		/// List with all labels of the page
		/// </summary>
		private readonly ISourceCache<Label, Guid> _pageLabels = new SourceCache<Label, Guid>(label => label.Id);

		/// <inheritdoc cref="_pageLabels"/>
		public readonly ReadOnlyObservableCollection<Label> PageLabels;

		private ISourceCache<LabelId, Guid> _pageLabelIds = new SourceCache<LabelId, Guid>(x => x.Id);

		public readonly ReadOnlyObservableCollection<LabelId> PageLabelIds;

		public ReadOnlyObservableCollection<Label> SelectedLabels;

		/// <summary>
		/// List of all selected ids.
		/// </summary>
		private readonly IList<Guid> _selectedLabelIds2;

		/// <summary>
		/// Query to filter the labels.
		/// </summary>
		private string _addLabelsQueryText = "";

		/// <inheritdoc cref="_addLabelsQueryText"/>
		public string AddLabelsQueryText
		{
			get => _addLabelsQueryText;
			set => this.RaiseAndSetIfChanged(ref _addLabelsQueryText, value);
		}

		private ISourceCache<Guid, Guid> _selectedLabelIds = new SourceCache<Guid, Guid>(x => x);

		public IObservable<Unit> OnCreateNewLabel => _onCreateNewLabel;
		private readonly ISubject<Unit> _onCreateNewLabel;

		public IObservable<Unit> OnManageLabels => _onManageLabels;
		private readonly ISubject<Unit> _onManageLabels;

		public ReactiveCommand<IEnumerable<Label>, Unit> AddLabels { get; }

		public ReactiveCommand<Unit, Unit> CreateNewLabel { get; }

		public ReactiveCommand<Unit, Unit> ManageLabels { get; }

		public ReactiveCommand<Unit, Unit> PopulateListWithAllLabels { get; }

		public ReactiveCommand<PageId, Unit> SelectLabelsForPage { get; }

		public ReactiveCommand<PageId, Unit> PopulateForPage { get; }


		public AddLabelsToPageControlViewModel(IPageLabelsBackend pageLabelsBackend, ILabelBackend labelBackend)
		{
			_pageLabelsBackend = pageLabelsBackend;
			_labelBackend = labelBackend;

			AddLabels = ReactiveCommand.CreateFromTask<IEnumerable<Label>>(AddLabelsAsync);
			CreateNewLabel = ReactiveCommand.Create<Unit>(x => _onCreateNewLabel.OnNext(x));
			ManageLabels = ReactiveCommand.Create<Unit>(x => _onManageLabels.OnNext(x));
			PopulateListWithAllLabels = ReactiveCommand.CreateFromTask(PopulateAllLabelsCache);
			PopulateForPage = ReactiveCommand.CreateFromTask<PageId>(PopulateForPageAsync);
			SelectLabelsForPage = ReactiveCommand.CreateFromTask<PageId>(SelectLabelsForPageAsync);

			_onCreateNewLabel = new Subject<Unit>();
			_onManageLabels = new Subject<Unit>();

			_allLabels.Connect()
				.Filter(
					this.WhenAnyValue(x => x.AddLabelsQueryText)
						.Select<string, Func<Label, bool>>(x =>
						{
							const StringComparison comp = StringComparison.OrdinalIgnoreCase;
							return label =>
								label.Key.Contains(x, comp) || label.Description.Contains(x, comp) ||
								label.Value is not null && label.Value.Contains(x, comp);
						}))
				.Bind(out AllLabels)
				.Subscribe();

			_allLabels.Connect()
				.Except(_pageLabels.Connect())
				.Filter(
					this.WhenAnyValue(x => x.AddLabelsQueryText)
						.Select<string, Func<Label, bool>>(x =>
						{
							const StringComparison comp = StringComparison.OrdinalIgnoreCase;
							return label =>
								label.Key.Contains(x, comp) || label.Description.Contains(x, comp) ||
								label.Value is not null && label.Value.Contains(x, comp);
						}))
				.Bind(out AddLabelsList)
				.Subscribe();

			_pageLabels.Connect()
				.Bind(out PageLabels)
				.Subscribe();

			_pageLabelIds.Connect()
				.Bind(out PageLabelIds)
				.Subscribe();

			/*
			
			_allLabels.Connect()
				.Filter()
				.Bind(out SelectedLabels)
				.Subscribe();

			var a = _selectedLabelIds.Connect();

			*/

			var b = _allLabels.Connect().Take(1).Transform(label => label.Id).PopulateInto(_selectedLabelIds);
		}

		private async Task AddLabelsAsync(IEnumerable<Label> labels)
		{
			foreach (var label in labels)
			{
				//await _pageLabelsBackend.AddLabelToPage(_pageId.Id, label.Id);
			}

			_pageLabels.Edit(updater => updater.AddOrUpdate(labels));
		}

		private async Task PopulateAllLabelsCache()
		{
			var labels = await _labelBackend.GetAllLabelsAsync();

			_allLabels.Edit(updater => updater.AddOrUpdate(labels));
		}

		private async Task PopulateForPageAsync(PageId id)
		{
			var labels = await _labelBackend.GetAllLabelsAsync();

			_allLabels.Edit(updater => updater.AddOrUpdate(labels));

			await LoadLabelsForPage(id);
		}

		private async Task PopulatePageLabelIds(PageId id)
		{
			var labels = (await _labelBackend.GetAllLabelsAsync()).AsParallel().Select(label => new LabelId(label.Id));

			_pageLabelIds.Edit(updater => updater.AddOrUpdate(labels));

			await LoadLabelsForPage(id);
		}

		private async Task LoadLabelsForPage(PageId pageId)
		{
			var labelIds = await _pageLabelsBackend.GetLabelIdsForPageId(pageId.Id);
			var labels = new List<Label>();

			foreach (var id in labelIds)
			{
				var label = await _labelBackend.GetLabelAsync(id);

				if (label.IsSuccess)
				{
					labels.Add(label.Value);
				}

				// TODO Failed Case, log
			}

			_pageLabels.Edit(updater =>
			{
				updater.Clear();
				updater.AddOrUpdate(labels);
			});
		}

		private async Task SelectLabelsForPageAsync(PageId pageId)
		{
			var labelIds = await _pageLabelsBackend.GetLabelIdsForPageId(pageId.Id);

			_selectedLabelIds.Edit(updater =>
			{
				updater.Clear();
				updater.AddOrUpdate(labelIds);
			});
		}
	}
}