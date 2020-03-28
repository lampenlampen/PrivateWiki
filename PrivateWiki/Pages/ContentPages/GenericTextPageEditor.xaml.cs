using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Models.Pages;
using Models.ViewModels;
using NLog;
using NodaTime;
using PrivateWiki.Models;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages.ContentPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class GenericTextPageEditor : Page, IViewFor<GenericTextPageEditorViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(GenericTextPageEditorViewModel), typeof(GenericTextPageEditor), new PropertyMetadata(null));

		public GenericTextPageEditorViewModel ViewModel
		{
			get => (GenericTextPageEditorViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (GenericTextPageEditorViewModel) value;
		}

		#endregion

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public GenericTextPageEditor()
		{
			this.InitializeComponent();
			
			ViewModel = new GenericTextPageEditorViewModel();

			this.WhenActivated(disposables =>
			{
				SaveButton.Events().Click
					.Select(args => PageEditorTextBox.Text)
					.InvokeCommand(this, x => x.ViewModel.SavePage)
					.DisposeWith(disposables);

				AbortButton.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(this, x => x.ViewModel.Abort)
					.DisposeWith(disposables);

				DeleteButton.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(this, x => x.ViewModel.DeletePage)
					.DisposeWith(disposables);

				ExternalEditorButton.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(this, x => x.ViewModel.ExternalEditor)
					.DisposeWith(disposables);

				ViewModel.ConfirmDelete
					.RegisterHandler(async interaction =>
					{
						// Ask user for confirmation to delete page.

						var dialog = new ContentDialog
						{
							Title = $"Delete Page",
							Content =
								"Delete this page permanently. After this action there is no way of restoring the current state.",
							CloseButtonText = "Cancel",
							PrimaryButtonText = "Delete Page",
							DefaultButton = ContentDialogButton.Close
						};

						var result = await dialog.ShowAsync();

						if (result == ContentDialogResult.Primary)
						{
							interaction.SetOutput(true);
						}
						else
						{
							interaction.SetOutput(false);
						}
					})
					.DisposeWith(disposables);

				this.Bind(ViewModel, x => x.Page.Content, v => v.PageEditorTextBox.Text).DisposeWith(disposables);

				ViewModel.GoBack.Subscribe( _ => GoBack()).DisposeWith(disposables);
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var pageLink = (string) e.Parameter;
			ViewModel.LoadPage.Execute(pageLink);
		}

		private void GoBack()
		{
			if (Frame.CanGoBack)
			{
				Frame.GoBack();
			}
		}
	}
}