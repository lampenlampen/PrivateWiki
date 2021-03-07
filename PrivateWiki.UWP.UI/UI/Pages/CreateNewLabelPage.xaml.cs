using System;
using Windows.UI.Xaml.Controls;
using NLog;
using PrivateWiki.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages
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