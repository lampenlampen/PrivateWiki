using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

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
            var images = imageFiles.Select(file => new ImagePickerDialogModel(file.DisplayName, new Uri(file.Path)));

            Images = new ObservableCollection<ImagePickerDialogModel>(images);

            ImageView.ItemsSource = Images;

            Debug.WriteLine($"Images: {Images.Count}");
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
			var selectedImage = ImageView.SelectedItem as ImagePickerDialogModel;
            PickedImage = selectedImage.Title;
        }
    }
}
