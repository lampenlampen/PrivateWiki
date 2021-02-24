using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
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
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.Settings
{
	public class PersonalizationCtrlBase : ReactiveUserControl<PersonalizationCtrlVM> { }

	public sealed partial class PersonalizationCtrl : PersonalizationCtrlBase
	{
		public PersonalizationCtrl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				this.OneWayBind(ViewModel,
						vm => vm.Languages,
						view => view.LanguageComboBox.ItemsSource)
					.DisposeWith(disposable);
			});
		}
	}
}
