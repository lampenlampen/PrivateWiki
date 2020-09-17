using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using NLog;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageBackendService.SQLite;
using PrivateWiki.UWP.Utilities;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ExternalEditor : Page
	{
		private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		private MarkdownPage Page;
		private readonly string TMP_FILE_FUTURE_ACCESS_LIST = "tmp_file_future_access_list";

		private IMarkdownPageStorage _storage;

		public ExternalEditor()
		{
			InitializeComponent();
			var storage = new SqLiteStorageOptions("test");
			_storage = new SqLiteBackend(storage, SystemClock.Instance);
		}

		protected override async void OnNavigatedTo([NotNull] NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var path = (string) e.Parameter;

			Page = await _storage.GetMarkdownPageAsync(path);

			LaunchExternalEditor();
		}

		private async void LaunchExternalEditor()
		{
			var file = await PickFile();

			await FileIO.WriteTextAsync(file, Page.Content);

			var success = await Windows.System.Launcher.LaunchFileAsync(file);
		}

		private async Task<StorageFile> PickFile()
		{
			// TODO Refactor Method
			var picker = new FileSavePicker
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				SuggestedFileName = Page.Link.Replace(":", "_")
			};
			picker.FileTypeChoices.Add("Markdown", new[] {".md"});

			var file = await picker.PickSaveFileAsync();

			// TODO Save Location
			StorageApplicationPermissions.FutureAccessList.AddOrReplace(
				TMP_FILE_FUTURE_ACCESS_LIST, file);

			return file;
		}

		private async void Import_Changes_Click(object sender, RoutedEventArgs e)
		{
			// TODO Import Changes
			var dialog = new ContentDialog
			{
				Title = "Save and Override",
				Content = "The Content of the current page will be overriden.",
				PrimaryButtonText = "Save",
				CloseButtonText = "Abort",
				DefaultButton = ContentDialogButton.Close
			};

			var action = await dialog.ShowAsync();

			if (action == ContentDialogResult.Primary)
			{
				var file =
					await StorageApplicationPermissions.FutureAccessList.GetFileAsync(
						TMP_FILE_FUTURE_ACCESS_LIST);

				var content = await FileIO.ReadTextAsync(file);

				//GenerateDiff(Page, content);

				Page.Content = content;

				_storage.UpdateMarkdownPage(Page, PageAction.Edited);

				file.DeleteAsync();
			}

			RemovePageEditorFromBackStack();
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private void RemovePageEditorFromBackStack()
		{
			if (Frame.CanGoBack)
			{
				var backstack = Frame.BackStack;
				var lastEntry = backstack[Frame.BackStackDepth - 1];
				if (lastEntry.SourcePageType == typeof(MarkdownPageEditor))
				{
					Logger.Debug("Remove PageEditor from BackStack");
					backstack.Remove(lastEntry);
				}
			}
		}

		private void GenerateDiff(MarkdownPage page, string newPage)
		{
			var diffEngine = new diff_match_patch();

			var a = diffEngine.diff_linesToChars(page.Content, newPage);
			var text1 = (string) a[0];
			var text2 = (string) a[1];
			var lineArray = (List<string>) a[2];


			var diff = diffEngine.diff_main(text1, text2, false);
			diffEngine.diff_charsToLines(diff, lineArray);

			diffEngine.diff_cleanupSemantic(diff);

			var htmlDiff = diffEngine.diff_prettyHtml(diff);

			Frame.Navigate(typeof(ImportDiffPage), Tuple.Create(diff, Page.Id));
		}

		private void ShowDiffView()
		{
		}
	}
}