using DataAccessLibrary;
using JetBrains.Annotations;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ExternalEditor : Page
	{
		private PageModel Page;
		private readonly string TMP_FILE_FUTURE_ACCESS_LIST = "tmp_file_future_access_list";
		private string VSCODE_PATH = "C:\\Software\\Microsoft VS Code\\bin\\code.cmd";

		private DataAccessImpl dataAccess;

		public ExternalEditor()
		{
			InitializeComponent();
			dataAccess = new DataAccessImpl();
		}

		protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var pageId = (string)e.Parameter;

			Page = dataAccess.GetPageOrNull(pageId);

			LaunchExternalEditor();
		}

		private async void LaunchExternalEditor()
		{
			var file = await PickFile();

			await FileIO.WriteTextAsync(file, Page.Content);

			var success = await Launcher.LaunchFileAsync(file);
		}

		private async Task<StorageFile> PickFile()
		{
			// TODO Refactor Method
			var picker = new FileSavePicker
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				SuggestedFileName = Page.Link.Replace(":", "_")
			};
			picker.FileTypeChoices.Add("Markdown", new[] { ".md" });

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

				GenerateDiff(Page, content);

				Page.Content = content;

				dataAccess.UpdatePage(Page);

				file.DeleteAsync();
			}

			//if (Frame.CanGoBack) Frame.GoBack();
		}

		private void GenerateDiff(PageModel page, string newPage)
		{
			var diffEngine = new diff_match_patch();

			var a = diffEngine.diff_linesToChars(page.Content, newPage);
			var text1 = a[0] as string;
			var text2 = a[1] as string;
			var lineArray = a[2] as List<string>;


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