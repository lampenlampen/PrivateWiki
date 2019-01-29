using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
    public sealed partial class PageEditorImagePickerDialog : DissmissableDialog
    {
        public ObservableCollection<ImagePickerDialogModel> Images { get; private set; } = new ObservableCollection<ImagePickerDialogModel>();

        public string PickedImage = "";

        public PageEditorImagePickerDialog()
        {
            this.InitializeComponent();
            
            ImageView.ItemsSource = Images;

            GetImagesAsync();
        }
        
        private async void GetImagesAsync()
        {
            var mediaFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("media");
            if (mediaFolder == null) return;
            

            var imageFolder = await mediaFolder.GetFolderAsync("images");
            if (imageFolder == null) return;

            var imageFiles = await imageFolder.GetFilesAsync();
            var images = imageFiles.Map(file => new ImagePickerDialogModel(file.Path, file.DisplayName));

            Images = new ObservableCollection<ImagePickerDialogModel>(images);

            ImageView.ItemsSource = Images;

            Debug.WriteLine($"Images: {Images.Count}");
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            PickedImage = "Hallo Test";
        }
    }
}
