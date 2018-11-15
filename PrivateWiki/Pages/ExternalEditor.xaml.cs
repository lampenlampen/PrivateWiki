using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using PrivateWiki.Data;
using StorageProvider;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExternalEditor : Page
    {
        private string VSCODE_PATH = "C:\\Software\\Microsoft VS Code\\bin\\code.cmd";
        private string TMP_FILE_FUTURE_ACCESS_LIST = "tmp_file_future_access_list";

        private ContentPage Page;

        public ExternalEditor()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string pageId = (string) e.Parameter;

            Page = new ContentPageProvider().GetContentPage(pageId);

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
            var picker = new Windows.Storage.Pickers.FileSavePicker()
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
                SuggestedFileName = Page.Id.Replace(":", "_")
            };
            picker.FileTypeChoices.Add("Markdown", new[] {".md"});

            var file = await picker.PickSaveFileAsync();

            // TODO Save Location
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(
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
                    await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync(
                        TMP_FILE_FUTURE_ACCESS_LIST);

                var content = await FileIO.ReadTextAsync(file);

                Page.Content = content;

                new ContentPageProvider().UpdateContentPage(Page);

                file.DeleteAsync();
            }

            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}