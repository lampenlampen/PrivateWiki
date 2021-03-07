using System;
using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Controls;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.UWP.UI.UI.Events;
using PrivateWiki.UWP.UI.Utilities.ExtensionFunctions;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PagesSettingsPage : Page, IViewFor<PagesSettingsPageViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(PagesSettingsPageViewModel), typeof(PagesSettingsPage),
				new PropertyMetadata(null));

		public PagesSettingsPageViewModel ViewModel
		{
			get => (PagesSettingsPageViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (PagesSettingsPageViewModel) value;
		}

		#endregion

		private GenericPage _selectedPage;

		public PagesSettingsPage()
		{
			this.InitializeComponent();

			ViewModel = new PagesSettingsPageViewModel();

			this.WhenActivated(disposable =>
			{
				ViewModel.LoadPages.Execute().Subscribe();

				PagesTable.ItemsSource = ViewModel.Pages;

				PagesTable.Events().SelectionChanged
					.Subscribe(_ =>
					{
						var selectedPage = (GenericPage) PagesTable.SelectedItem;
						GenericPage selectedPage1 = selectedPage;
					})
					.DisposeWith(disposable);

				PagesTable.Events().RightTapped
					.Subscribe(x =>
					{
						var a = x;
						var b = a.OriginalSource;

						var c = ((DependencyObject) b).FindParent<DataGridRow>();

						if (c != null)
						{
							var index = c.GetIndex();
							PagesTable.SelectedIndex = index;

							x.Handled = true;
						}
						else
						{
							PagesTable.SelectedIndex = -1;
							x.Handled = false;
						}
					})
					.DisposeWith(disposable);

				DeletePage.Events().Click
					.Subscribe(x => { App.Current.GlobalNotificationManager.ShowNotImplementedNotification(); })
					.DisposeWith(disposable);
			});
		}

		private static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			T parent = null;
			DependencyObject CurrentParent = VisualTreeHelper.GetParent(child);
			while (CurrentParent != null)
			{
				if (CurrentParent is T)
				{
					parent = (T) CurrentParent;
					break;
				}

				CurrentParent = VisualTreeHelper.GetParent(CurrentParent);
			}

			return parent;
		}
	}
}