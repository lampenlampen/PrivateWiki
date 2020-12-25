using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
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
		/// Cache with all labels
		/// </summary>
		private readonly ISourceCache<SelectableLabel, LabelId> _allLabelsSelectable = new SourceCache<SelectableLabel, LabelId>(label => label.Id);

		private readonly ISourceCache<SelectableLabel, LabelId> _allLabelsSelectableFiltered = new SourceCache<SelectableLabel, LabelId>(label => label.Id);

		private readonly ISourceCache<LabelId, LabelId> _selectedLabelIds = new SourceCache<LabelId, LabelId>(id => id);

		public readonly ObservableCollection<SelectableLabel> AllLabelsSelectable = new ObservableCollectionExtended<SelectableLabel>();

		public readonly ReadOnlyObservableCollection<SelectableLabel> SelectedLabels;

		/// <summary>
		/// List with all labels of the page
		/// </summary>
		private readonly ISourceCache<Label, Guid> _pageLabels = new SourceCache<Label, Guid>(label => label.Id);

		/// <inheritdoc cref="_pageLabels"/>
		public readonly ReadOnlyObservableCollection<Label> PageLabels;

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

		public IObservable<Unit> OnCreateNewLabel => _onCreateNewLabel;
		private readonly ISubject<Unit> _onCreateNewLabel;

		public IObservable<Unit> OnManageLabels => _onManageLabels;
		private readonly ISubject<Unit> _onManageLabels;

		public ReactiveCommand<Unit, Unit> CreateNewLabel { get; }

		public ReactiveCommand<Unit, Unit> ManageLabels { get; }

		public ReactiveCommand<PageId, Unit> PopulateForPage { get; }


		public AddLabelsToPageControlViewModel(IPageLabelsBackend pageLabelsBackend, ILabelBackend labelBackend)
		{
			_pageLabelsBackend = pageLabelsBackend;
			_labelBackend = labelBackend;

			CreateNewLabel = ReactiveCommand.Create<Unit>(x => _onCreateNewLabel.OnNext(x));
			ManageLabels = ReactiveCommand.Create<Unit>(x => _onManageLabels.OnNext(x));
			PopulateForPage = ReactiveCommand.CreateFromTask<PageId>(PopulateForPageAsync);

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


			_pageLabels.Connect()
				.Bind(out PageLabels)
				.Subscribe();

			_allLabelsSelectable.Connect()
				.Filter(
					this.WhenAnyValue(x => x.AddLabelsQueryText)
						.Select<string, Func<SelectableLabel, bool>>(x =>
						{
							const StringComparison comp = StringComparison.OrdinalIgnoreCase;
							return label =>
								label.Key.Contains(x, comp) || label.Description.Contains(x, comp) ||
								label.Value is not null && label.Value.Contains(x, comp);
						}))
				.PopulateInto(_allLabelsSelectableFiltered);

			_allLabelsSelectableFiltered.Connect()
				.Bind((IObservableCollection<SelectableLabel>) AllLabelsSelectable)
				.Subscribe();

			_allLabelsSelectableFiltered.Connect()
				//	.Filter(a)
				.Bind(out SelectedLabels)
				.Subscribe();
		}

		private async Task LoadAllLabelsAsync()
		{
			var labels = (await _labelBackend.GetAllLabelsAsync()).Select(label => new SelectableLabel(label, false));

			_allLabelsSelectable.Edit(updater => updater.AddOrUpdate(labels));
		}

		private async Task LoadSelectedLabelsAsync(PageId id)
		{
			var ids = (await _pageLabelsBackend.GetLabelIdsForPageId(id.Id)).Select(id => new LabelId(id));

			foreach (var labelId in ids)
			{
				var label = _allLabelsSelectable.Lookup(labelId).Value;

				label.IsSelected = true;

				_allLabelsSelectable.AddOrUpdate(label);
			}

			_selectedLabelIds.Edit(updater => updater.AddOrUpdate(ids));
		}

		private async Task PopulateForPageAsync(PageId id)
		{
			await LoadAllLabelsAsync();
			await LoadSelectedLabelsAsync(id);

			var labels = await _labelBackend.GetAllLabelsAsync();

			_allLabels.Edit(updater => updater.AddOrUpdate(labels));

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
	}

	public class SelectableLabel
	{
		private readonly Label _label;

		public LabelId Id => _label.LabelId;

		public Color Color => _label.Color;

		public string Description => _label.Description;

		public string Key => _label.Key;

		public string? Value => _label.Value;

		public bool IsSelected { get; set; }

		public SelectableLabel(Label label, bool selected)
		{
			_label = label;
			IsSelected = selected;
		}
	}
}