using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class XamlPageViewer : Page
	{
		public XamlPageViewer()
		{
			this.InitializeComponent();
		}

		private void Code_OnClick(object sender, RoutedEventArgs e)
		{
			

			var textbox = new TextBox();
			textbox.Text = "new Header";
			textbox.FontSize = 18d;
			textbox.FontWeight = FontWeights.Bold;

			StackPanel.Children.Insert(StackPanel.Children.Count -1, textbox);
		}

		private void Text_OnClick(object sender, RoutedEventArgs e)
		{
			var textbox = new TextBox();
			textbox.Text = ExampleText();
			textbox.FontSize = 14d;
			textbox.TextWrapping = TextWrapping.Wrap;
			//textbox.IsReadOnly = true;
			textbox.BorderThickness = new Thickness(0);
			

			StackPanel.Children.Insert(StackPanel.Children.Count - 1, textbox);
		}

		private void Header_OnClick(object sender, RoutedEventArgs e)
		{
			var textbox = new TextBox();
			textbox.Text = "new Code";
			textbox.FontSize = 14d;
			textbox.FontWeight = FontWeights.Light;
			textbox.FontStyle = FontStyle.Italic;

			StackPanel.Children.Insert(StackPanel.Children.Count - 1, textbox);
		}

		private string ExampleText()
		{
			return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In condimentum sit amet nunc et scelerisque. Pellentesque nec metus a mauris bibendum interdum. Aliquam malesuada non ante ac convallis. Nunc vitae ligula dolor. Pellentesque a lacinia felis, et facilisis dui. Quisque vulputate auctor nisl vitae elementum. Aenean nec congue justo. Maecenas in dapibus nisl, sit amet mollis sapien. Duis iaculis odio vitae diam bibendum pellentesque. Suspendisse nec rhoncus orci, blandit molestie nisl.\r\n\r\nSed tincidunt magna nisl, sit amet luctus sapien luctus a. In hac habitasse platea dictumst. Sed lorem leo, interdum sed lobortis in, rhoncus nec mi. Aliquam faucibus, risus nec fermentum blandit, orci ante posuere nisl, vel pharetra dui risus vitae ex. Sed vitae consequat augue. Ut quis tincidunt metus. Vestibulum quam nunc, fermentum at leo et, elementum commodo sapien. Vestibulum metus orci, egestas sit amet efficitur a, venenatis ut erat. Suspendisse ut mollis mauris. Aliquam tempor ex sit amet arcu tincidunt tincidunt. Cras cursus ante eget egestas ornare. Nam sit amet dictum enim, ut auctor erat. Quisque accumsan tortor sit amet orci laoreet feugiat. Vivamus quis congue tortor, non scelerisque enim. Phasellus blandit malesuada orci non interdum. Etiam tincidunt malesuada nisi vel accumsan.";
		}
	}
}
