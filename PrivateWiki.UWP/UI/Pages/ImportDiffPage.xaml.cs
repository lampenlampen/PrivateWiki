using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NLog;
using PrivateWiki.UWP.Utilities;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UWP.UI.Pages
{
	/// <summary>
	///     Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class ImportDiffPage : Page
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public static List<Diff> Diff = null;

		public ImportDiffPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			(List<Diff> diffs, string id) = (Tuple<List<Diff>, string>) e.Parameter;

			GenerateDiff(diffs);
		}

		private void GenerateDiff(List<Diff> diffs)
		{
			var paragraph = new Paragraph();
			DiffBlock.Blocks.Add(paragraph);

			foreach (var diff in diffs)
				switch (diff.operation)
				{
					case Operation.DELETE:

						var run = new Run();
						run.Text = diff.text;
						var span = new Span();
						span.Inlines.Add(run);
						span.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 51, 51));
						span.TextDecorations = TextDecorations.Strikethrough;

						paragraph.Inlines.Add(span);

						break;
					case Operation.INSERT:

						var run2 = new Run();
						run2.Text = diff.text;
						var span2 = new Span();
						span2.Inlines.Add(run2);
						span2.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 204, 0));
						span2.TextDecorations = TextDecorations.Underline;

						paragraph.Inlines.Add(span2);
						break;
					case Operation.EQUAL:

						var run3 = new Run();
						run3.Text = diff.text;
						var span3 = new Span();
						span3.Inlines.Add(run3);
						span3.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

						paragraph.Inlines.Add(span3);
						break;
				}
		}

		private void DiffBtn_Click(object sender, RoutedEventArgs e)
		{
			RemoveExternalEditorPageFromBackStack();
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private void RemoveExternalEditorPageFromBackStack()
		{
			if (Frame.CanGoBack)
			{
				var backstack = Frame.BackStack;
				var lastEntry = backstack[Frame.BackStackDepth - 1];
				if (lastEntry.SourcePageType == typeof(ExternalEditor))
				{
					Logger.Debug("Remove ExternalEditor from BackStack");
					backstack.Remove(lastEntry);
				}
			}
		}
	}
}