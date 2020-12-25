using System;
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
		private readonly ISourceCache<SelectableLabel, LabelId> _allLabelsCache = new SourceCache<SelectableLabel, LabelId>(label => label.Id);

		/// <summary>
		/// Cache with all labels filtered by the predicate from the user.
		/// </summary>
		private readonly ISourceCache<SelectableLabel, LabelId> _allLabelsFilteredCache = new SourceCache<SelectableLabel, LabelId>(label => label.Id);

		/// <summary>
		/// Cache with all labelIds, that should be pre-selected.
		/// </summary>
		private readonly ISourceCache<LabelId, LabelId> _selectedLabelIds = new SourceCache<LabelId, LabelId>(id => id);

		/// <summary>
		/// Collection with all labels for binding.
		/// </summary>
		public readonly ObservableCollection<SelectableLabel> AllLabelsCollection = new ObservableCollectionExtended<SelectableLabel>();

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


			_allLabelsCache.Connect()
				.Filter(
					this.WhenAnyValue(x => x.AddLabelsQueryText)
						.Select<string, Func<SelectableLabel, bool>>(x =>
						{
							const StringComparison comp = StringComparison.OrdinalIgnoreCase;
							return label =>
								label.Key.Contains(x, comp) || label.Description.Contains(x, comp) ||
								label.Value is not null && label.Value.Contains(x, comp);
						}))
				.PopulateInto(_allLabelsFilteredCache);

			_allLabelsFilteredCache.Connect()
				.Bind((IObservableCollection<SelectableLabel>) AllLabelsCollection)
				.Subscribe();
		}

		private async Task LoadAllLabelsAsync()
		{
			var labels = (await _labelBackend.GetAllLabelsAsync()).Select(label => new SelectableLabel(label, false));

			_allLabelsCache.Edit(updater => updater.AddOrUpdate(labels));
		}

		private async Task LoadSelectedLabelIdsAsync(PageId id)
		{
			var ids = (await _pageLabelsBackend.GetLabelIdsForPageId(id.Id)).Select(id => new LabelId(id));

			foreach (var labelId in ids)
			{
				var label = _allLabelsCache.Lookup(labelId).Value;

				label.IsSelected = true;

				_allLabelsCache.AddOrUpdate(label);
			}

			_selectedLabelIds.Edit(updater => updater.AddOrUpdate(ids));
		}

		private async Task PopulateForPageAsync(PageId id)
		{
			await LoadAllLabelsAsync();
			await LoadSelectedLabelIdsAsync(id);
		}
	}

	public class SelectableLabel : ISelectable
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