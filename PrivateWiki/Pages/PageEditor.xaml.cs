using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using PrivateWiki.Data;
using PrivateWiki.Dialogs;
using PrivateWiki.Markdig;
using StorageProvider;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages
{
    /// <summary>
    ///     Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageEditor : Page
	{
		public PageEditor()
		{
			InitializeComponent();
		}

		[NotNull] private ContentPage Page { get; set; }
		private bool NewPage { get; set; }

		private void PreviewWebviewNavigationStartedAsync(WebView sender,
			[NotNull] WebViewNavigationStartingEventArgs args)
		{
			var uri = args.Uri;
			Debug.WriteLine($"Link Clicked: {uri}");

			InAppNotification.Show($"Link Clicked: {uri.AbsoluteUri}", 5000);

			if (uri.AbsoluteUri.StartsWith("ms-local-stream:"))
			{
				Debug.WriteLine($"Local HtmlFile: {uri.AbsoluteUri}");
				return;
			}

			args.Cancel = true;
		}

		protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
		{
			var pageId = (string) e.Parameter;
			Debug.WriteLine($"Id: {pageId}");
			if (pageId == null) throw new ArgumentNullException("Page id must be nonnull!");

			var pageProvider = new ContentPageProvider();

			if (pageProvider.ContainsContentPage(pageId))
			{
				Page = pageProvider.GetContentPage(pageId);
			}
			else
			{
				NewPage = true;
				Page = ContentPage.Create(pageId);
			}

			ShowPageInEditor();
			if (NewPage) RemoveNewPageFromBackStack();
		}

		private void ShowPageInEditor()
		{
			PageEditorTextBox.Text = Page.Content;
		}

		private void RemoveNewPageFromBackStack()
		{
			if (Frame.CanGoBack)
			{
				var backstack = Frame.BackStack;
				var lastEntry = backstack[Frame.BackStackDepth - 1];
				if (lastEntry.SourcePageType == typeof(NewPage))
				{
					Debug.WriteLine("Remove NewPage from BackStack");
					backstack.Remove(lastEntry);
				}
			}
		}

		private void RemoveEditorPageFromBackStack()
		{
			if (Frame.CanGoBack)
			{
				var backstack = Frame.BackStack;
				var lastEntry = backstack[Frame.BackStackDepth - 1];
				if (lastEntry.SourcePageType == typeof(PageEditor))
				{
					Debug.WriteLine("Remove EditorPage from BackStack");
					backstack.Remove(lastEntry);
				}
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			Page.Content = PageEditorTextBox.Text;

			if (NewPage)
			{
				// TODO Error while Inserting Page
				new ContentPageProvider().InsertContentPage(Page);

				Frame.Navigate(typeof(PageViewer), Page.Id);
				RemoveEditorPageFromBackStack();
			}
			else
			{
				// TODO Error while Updating Page
				new ContentPageProvider().UpdateContentPage(Page);

				if (Frame.CanGoBack) Frame.GoBack();
			}
		}

		private void Abort_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private async void Delete_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new ContentDialog
			{
				Title = $"Delete Page {Page.Id}",
				Content =
					"Delete this page permanently. After this action there is no way of restoring the current state.",
				CloseButtonText = "Cancel",
				PrimaryButtonText = "Delete Page",
				DefaultButton = ContentDialogButton.Close
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				// Delete the page.
				Debug.WriteLine("Delete");
				new ContentPageProvider().DeleteContentPage(Page);

				if (Frame.CanGoBack)
				{
					Frame.GoBack();
					Frame.GoBack();
				}
			}
		}

		private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Pivot.SelectedIndex == 1)
			{
				var htmlFileName = "index_preview.html";
				var parser = new MarkdigParser();
				var html = parser.ToHtmlString(PageEditorTextBox.Text);
				var localFolder = ApplicationData.Current.LocalFolder;
				var mediaFolder = await localFolder.GetFolderAsync("media");
				var file = await mediaFolder.CreateFileAsync(htmlFileName, CreationCollisionOption.ReplaceExisting);
				await FileIO.WriteTextAsync(file, html);

				Preview_WebView.Navigate(new Uri($"ms-appdata:///local/media/{htmlFileName}"));
			}

			if (Pivot.SelectedIndex == 2)
				foreach (var tag in Page.Tags)
					ListView.Items.Add(tag.Name);
		}

		private void AddTag_Click(object sender, RoutedEventArgs e)
		{
			var tagName = AddTagBox.Text;
			var tag = new Tag
			{
				Name = tagName
			};

			// TODO Save Tags to DB
			//Page.Tags.Add(tag);
			ListView.Items.Add(tagName);
		}

		private void VS_Code_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(ExternalEditor), Page.Id);
		}

		private async void PageEditor_AddImage(object sender, RoutedEventArgs e)
		{
			var file = await MediaAccess.PickImageFileAsync();

			if (file != null)
			{
				var imageFolder = await MediaAccess.GetImageFolder();

				// Copy image to imageFolder
				// Throws AggregateException if file already exists.
				try
				{
					file.CopyAsync(imageFolder).AsTask().Wait();
				}
				catch (AggregateException)
				{
				}

				var index = PageEditorTextBox.SelectionStart;
				var imageMarkdown = $"![{file.Name}](\\images\\{file.Name}.{file.FileType})";
				var newContent = PageEditorTextBox.Text.Insert(index, imageMarkdown);
				PageEditorTextBox.Text = newContent;
			}
		}

		private void PageEditorToolbar_Bold(object sender, RoutedEventArgs e)
		{
			var index = PageEditorTextBox.SelectionStart;
			var length = PageEditorTextBox.SelectionLength;

			if (length == 0)
			{
				var newContent = PageEditorTextBox.Text.Insert(index, "****");
				PageEditorTextBox.Text = newContent;
				PageEditorTextBox.SelectionStart = index + 2;
			}
			else
			{
				var text = PageEditorTextBox.Text.Insert(index, "**").Insert(index + length + 2, "**");
				PageEditorTextBox.Text = text;
				PageEditorTextBox.SelectionStart = index;
				PageEditorTextBox.SelectionLength = length + 4;
			}
		}

		private void PageEditorToolbar_Underline(object sender, RoutedEventArgs e)
		{
			// TODO Toolbar Underline
		}

		private void PageEditorToolbar_Italic(object sender, RoutedEventArgs e)
		{
			var index = PageEditorTextBox.SelectionStart;
			var length = PageEditorTextBox.SelectionLength;

			if (length == 0)
			{
				var newContent = PageEditorTextBox.Text.Insert(index, "**");
				PageEditorTextBox.Text = newContent;
				PageEditorTextBox.SelectionStart = index + 2;
			}
			else
			{
				var text = PageEditorTextBox.Text.Insert(index, "*").Insert(index + length + 2, "*");
				PageEditorTextBox.Text = text;
				PageEditorTextBox.SelectionStart = index;
				PageEditorTextBox.SelectionLength = length + 4;
			}
		}

		private async void PageEditorToolbar_HyperLink(object sender, RoutedEventArgs e)
		{
			if (PageEditorTextBox.SelectionLength != 0) return;

			var dialog = new HyperlinkDialog();

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				// Check if URI is valid.
				// Only HTTP and HTTPS allowed.

				Uri link;
				Uri.TryCreate(dialog.Hyperlink, UriKind.Absolute, out link);
				var isValid = link.Scheme == Uri.UriSchemeHttp || link.Scheme == Uri.UriSchemeHttps;
				Debug.WriteLine($"Link isValid: {isValid}, {link}");

				if (isValid && PageEditorTextBox.SelectionLength == 0)
				{
					var index = PageEditorTextBox.SelectionStart;
					var markup = $"[{link}]({link})";
					var text = PageEditorTextBox.Text.Insert(index, markup);
					PageEditorTextBox.Text = text;
					PageEditorTextBox.SelectionStart = index + markup.Length;
				}
			}
		}

		private async void PageEditorToolbar_Wikilink(object sender, RoutedEventArgs e)
		{
			if (PageEditorTextBox.SelectionLength != 0) return;

			var dialog = new WikiLinkDialog();

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				var link = dialog.WikiLink;

				var index = PageEditorTextBox.SelectionStart;
				var markup = $"[{link}](:{link})";
				var text = PageEditorTextBox.Text.Insert(index, markup);
				PageEditorTextBox.Text = text;
				PageEditorTextBox.SelectionStart = index + markup.Length;
			}
		}

		private void PageEditorToolbar_NumberedList(object sender, RoutedEventArgs e)
		{
			// TODO Toolbar NumberedList
		}

		private void PageEditorToolbar_List(object sender, RoutedEventArgs e)
		{
			// TODO Toolbar List
		}

		private void PageEditorToolbar_Quote(object sender, RoutedEventArgs e)
		{
			// TODO Toolbar Quote
		}

		private void PageEditorToolbar_HorizontalRule(object sender, RoutedEventArgs e)
		{
			if (PageEditorTextBox.SelectionLength != 0) return;

			var index = PageEditorTextBox.SelectionStart;

			string text;

			if (PageEditorTextBox.Text[index - 1].Equals('\r'))
				text = PageEditorTextBox.Text.Insert(index, "---\r\n");
			else
				text = PageEditorTextBox.Text.Insert(index, "\r\n---\r\n");

			PageEditorTextBox.Text = text;
			PageEditorTextBox.SelectionStart = index + 5;
		}

		private void PageEditorToolbar_Code(object sender, RoutedEventArgs e)
		{
			// TODO Toolbar Code
		}

		private void PageEditorToolbar_Strikethrough(object sender, RoutedEventArgs e)
		{
			var index = PageEditorTextBox.SelectionStart;
			var length = PageEditorTextBox.SelectionLength;

			if (length == 0)
			{
				var newContent = PageEditorTextBox.Text.Insert(index, "~~~~");
				PageEditorTextBox.Text = newContent;
				PageEditorTextBox.SelectionStart = index + 2;
			}
			else
			{
				var text = PageEditorTextBox.Text.Insert(index, "~~").Insert(index + length + 2, "~~");
				PageEditorTextBox.Text = text;
				PageEditorTextBox.SelectionStart = index;
				PageEditorTextBox.SelectionLength = length + 4;
			}
		}

		private async void PageEditorToolbar_Table(object sender, RoutedEventArgs e)
		{
			// TODO Toolbar Table

			var dialog = new GridViewDialog();
			await dialog.ShowAsync();
		}
	}
}