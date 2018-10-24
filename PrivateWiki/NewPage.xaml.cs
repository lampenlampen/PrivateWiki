using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using JetBrains.Annotations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace PrivateWiki
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPage : Page
    {
        [CanBeNull]
        private string _pageId;

        public NewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _pageId = (string) e.Parameter;
        }

        private void CreatePage_Click([NotNull] object sender, [NotNull] RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageEditor), _pageId);
        }
    }
}
