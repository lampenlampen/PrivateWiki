using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#nullable enable

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UI.Controls
{
	public sealed partial class SettingsHeader : UserControl
	{
		public SettingsHeader()
		{
			this.InitializeComponent();
		}

		public IconElement Icon
		{
			get => (IconElement) GetValue(IconProperty);
			set
			{
				SetValue(IconProperty, value);
				SettingsHeaderIcon.Children.Add(value);
			}
		}

		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("IconProperty", typeof(IconElement), typeof(SettingsHeader), null);

		public string Title
		{
			get => (string) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		// Using a DependencyProperty as the backing store for TitleProperty.
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("TitleProperty",
			typeof(string), typeof(SettingsHeader), null);

		public string Subtitle
		{
			get => (string) GetValue(SubtitleProperty);
			set => SetValue(SubtitleProperty, value);
		}

		// Using a DependencyProperty as the backing store for TitleProperty.
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register("SubtitleProperty",
			typeof(string), typeof(SettingsHeader), null);

		public event RoutedEventHandler? ApplyClick;

		protected void Apply_Click(object sender, RoutedEventArgs e)
		{
			//bubble the event up to the parent
			ApplyClick?.Invoke(this, e);
		}

		public event RoutedEventHandler? ResetClick;

		protected void Reset_Click(object sender, RoutedEventArgs e)
		{
			//bubble the event up to the parent
			ResetClick?.Invoke(this, e);
		}
	}
}