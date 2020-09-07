using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using NLog;
using PrivateWiki.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CreateNewLabelPage : Page
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		
		public CreateNewLabelPage()
		{
			this.InitializeComponent();

			var vm = new CreateNewLabelControlViewModel();
			CreateNewLabelControl.ViewModel = vm;

			vm.OnCancel.Subscribe(_ =>
			{
				if (Frame.CanGoBack)
				{
					Frame.GoBack();
				}
				else
				{
					Logger.Error("Can't go back");
				}
			});
		}
	}
}