using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewCollapsedEventArgs = Microsoft.UI.Xaml.Controls.TreeViewCollapsedEventArgs;
using TreeViewExpandingEventArgs = Microsoft.UI.Xaml.Controls.TreeViewExpandingEventArgs;
using TreeViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs;
using muxc = Microsoft.UI.Xaml.Controls;

#nullable enable

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MediaManager : Page
	{
		public MediaManager()
		{
			InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			// Init TreeView
			var localFolder = ApplicationData.Current.LocalFolder;
			var mediaFolder = await localFolder.CreateFolderAsync("media", CreationCollisionOption.OpenIfExists);

			//var folder = await PickFolder();
			var folder = mediaFolder;

			var mediaNode = new muxc.TreeViewNode
			{
				Content = folder,
				IsExpanded = true,
				HasUnrealizedChildren = true
			};

			TreeViewMedia.RootNodes.Add(mediaNode);
			FillTreeNode(mediaNode);
		}

		private async void FillTreeNode(muxc.TreeViewNode node)
		{
			// Only process the node if it's a folder and has unrealized children.
			StorageFolder folder;
			if (node.Content is StorageFolder content && node.HasUnrealizedChildren) folder = content;
			else return;

			var itemsList = await folder.GetItemsAsync();

			if (itemsList.Count == 0) return;

			foreach (var item in itemsList)
			{
				var newNode = new muxc.TreeViewNode { Content = item };

				if (item is StorageFolder) newNode.HasUnrealizedChildren = true;

				node.Children.Add(newNode);
			}

			// Children were just added to this node, so set HasUnrealizedChildren to false.
			node.HasUnrealizedChildren = false;
		}

		private void TreeViewMedia_OnExpanding(muxc.TreeView treeView, TreeViewExpandingEventArgs args)
		{
			if (args.Node.HasUnrealizedChildren) FillTreeNode(args.Node);
		}

		private void TreeViewMedia_OnCollapsed(muxc.TreeView treeView, TreeViewCollapsedEventArgs args)
		{
			args.Node.Children.Clear();
			args.Node.HasUnrealizedChildren = true;
		}

		private void TreeViewMedia_OnItemInvoked(TreeView treeView, TreeViewItemInvokedEventArgs args)
		{
			var node = (TreeViewNode) args.InvokedItem;

			if (node.Content is IStorageItem item)
			{
				Debug.WriteLine($"Media: {item.Path}/{item.Name}");
				ClickedItem.Text = $"Media: {item.Path}/{item.Name}";

				if (node.Content is StorageFolder) node.IsExpanded = !node.IsExpanded;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private void TreeViewMedia_OnDropCompleted(UIElement sender, DropCompletedEventArgs args)
		{
			Debug.WriteLine($"Drop Completed DropResult: {args.DropResult}");
		}

		private async void MediaTreeView_CreateFolder(object sender, RoutedEventArgs e)
		{
			var node = (muxc.TreeViewNode) ((MenuFlyoutItem) sender).DataContext;
			var folder = (StorageFolder) node.Content;

			var panel = new StackPanel { Orientation = Orientation.Vertical };
			var textBlock = new TextBlock
			{
				Text = "Please enter a name for the new folder."
			};
			panel.Children.Add(textBlock);
			var textBox = new TextBox();
			panel.Children.Add(textBox);

			var dialog = new ContentDialog
			{
				Title = "Create Folder",
				Content = panel,
				PrimaryButtonText = "Create",
				CloseButtonText = "Abort",
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				// Create Folder
				var name = textBox.Text;
				await folder.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);

				var parent = node.Parent;
				parent.IsExpanded = false;
				parent.IsExpanded = true;

				Debug.WriteLine($"Create folder: {name}");
			}
		}

		private async void MediaTreeView_DeleteFolder(object sender, RoutedEventArgs e)
		{
			var node = (muxc.TreeViewNode) ((MenuFlyoutItem) sender).DataContext;
			var folder = (StorageFolder) node.Content;

			var dialog = new ContentDialog
			{
				Title = $"Delete Folder: {folder.Name}",
				Content = "Do you really want to delete this folder with it's children?",
				PrimaryButtonText = "Delete",
				CloseButtonText = "Abort",
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				await folder.DeleteAsync();

				var parent = node.Parent;
				parent.IsExpanded = false;
				parent.IsExpanded = true;
				Debug.WriteLine("Delete folder");
			}
		}

		private async void MediaTreeView_RenameFolder(object sender, RoutedEventArgs e)
		{
			var node = (muxc.TreeViewNode) ((MenuFlyoutItem) sender).DataContext;
			var folder = (StorageFolder) node.Content;

			var panel = new StackPanel { Orientation = Orientation.Vertical };
			var textBlock = new TextBlock
			{
				Text = "Please enter a new name."
			};
			panel.Children.Add(textBlock);
			var textBox = new TextBox
			{
				Text = folder.Name
			};
			panel.Children.Add(textBox);

			var dialog = new ContentDialog
			{
				Title = $"Rename Folder: {folder.Name}",
				Content = panel,
				PrimaryButtonText = "Rename",
				CloseButtonText = "Abort",
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				// Rename Folder
				var newName = textBox.Text;
				await folder.RenameAsync(newName, NameCollisionOption.GenerateUniqueName);

				var parent = node.Parent;
				parent.IsExpanded = false;
				parent.IsExpanded = true;

				Debug.WriteLine($"Rename folder to {newName}");
			}
		}

		private async void MediaTreeView_DeleteFile(object sender, RoutedEventArgs e)
		{
			var node = (muxc.TreeViewNode) ((MenuFlyoutItem) sender).DataContext;
			var file = (StorageFile) node.Content;

			var dialog = new ContentDialog
			{
				Title = $"Delete File: {file.Name}",
				Content = "Do you really want to delete this file?",
				PrimaryButtonText = "Delete",
				CloseButtonText = "Abort",
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				await file.DeleteAsync();

				var parent = node.Parent;
				parent.IsExpanded = false;
				parent.IsExpanded = true;
				Debug.WriteLine($"Delete file: {file.Name}");
			}
		}

		private async void MediaTreeView_RenameFile(object sender, RoutedEventArgs e)
		{
			var node = (muxc.TreeViewNode) ((MenuFlyoutItem) sender).DataContext;
			var file = (StorageFile) node.Content;

			var panel = new StackPanel { Orientation = Orientation.Vertical };
			var textBlock = new TextBlock
			{
				Text = "Please enter a new name."
			};
			panel.Children.Add(textBlock);
			var textBox = new TextBox
			{
				Text = file.Name
			};
			panel.Children.Add(textBox);

			var dialog = new ContentDialog
			{
				Title = $"Rename Folder: {file.Name}",
				Content = panel,
				PrimaryButtonText = "Rename",
				CloseButtonText = "Abort",
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				// Rename Folder
				var newName = textBox.Text;
				await file.RenameAsync(newName, NameCollisionOption.GenerateUniqueName);

				var parent = node.Parent;
				parent.IsExpanded = false;
				parent.IsExpanded = true;

				Debug.WriteLine($"Rename file to {newName}");
			}
		}

		private async void MediaTreeView_AddFiles(object sender, RoutedEventArgs e)
		{
			var node = (muxc.TreeViewNode) ((MenuFlyoutItem) sender).DataContext;
			var folder = (StorageFolder) node.Content;

			var picker = new FileOpenPicker
			{
				ViewMode = PickerViewMode.Thumbnail,
				SuggestedStartLocation = PickerLocationId.PicturesLibrary
			};
			picker.FileTypeFilter.Add(".jpg");
			picker.FileTypeFilter.Add(".png");

			var files = await picker.PickMultipleFilesAsync();

			if (files.Count > 0)
			{
				var copiedFiles = files.Select(it => it.CopyAsync(folder).AsTask());

				var panel = new StackPanel();
				var progressBar = new ProgressBar { IsIndeterminate = true };
				panel.Children.Add(progressBar);

				var dialog2 = new ContentDialog
				{
					Title = "Copy Files...",
					Content = panel,
					PrimaryButtonText = "Cancel"
				};

				dialog2.ShowAsync();
				await Task.WhenAll(copiedFiles.ToArray());
				dialog2.Hide();

				var dialog = new ContentDialog
				{
					Title = "Copy successful",
					PrimaryButtonText = "Okay",
					DefaultButton = ContentDialogButton.Primary
				};

				await dialog.ShowAsync();

				var parent = node.Parent;
				parent.IsExpanded = false;
				parent.IsExpanded = true;
			}
		}
	}
}