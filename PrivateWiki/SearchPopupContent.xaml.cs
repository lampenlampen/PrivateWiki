using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PrivateWiki.Data;
using SQLitePCL;
using StorageProvider;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki
{
    public sealed partial class SearchPopupContent : UserControl
    {
        private List<ContentPage> _pages;
        ObservableCollection<ContentPage> Pages = new ObservableCollection<ContentPage>();



        public SearchPopupContent()
        {
            this.InitializeComponent();

            var provider = new ContentPageProvider();
            _pages = provider.GetAllContentPages();

            
            SearchResultsBox.ItemsSource = Pages;
            //SearchBox.ItemsSource = Pages;

        }

        private void Filter(string text)
        {
            // TODO Filter Search Results
            return;
            var pages = _pages.Filter(p => p.Id.Contains(text));
            

            foreach (var page in Pages)
            {
                if (!pages.Contains(page))
                {
                    
                }
            }
        }

        private void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Filter(SearchBox.Text);
        }

        private void SearchBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Debug.WriteLine($"Search: {SearchBox.Text}");

            var  p = Parent as FrameworkElement;

            while (!(p is Popup))
            {
                p = p.Parent as FrameworkElement;
            }

            (p as Popup).IsOpen = false;
        }
    }
}
