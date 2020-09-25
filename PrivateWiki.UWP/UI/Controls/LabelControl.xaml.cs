using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using JetBrains.Annotations;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;
using PrivateWiki.ViewModels.Controls;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls
{
	public class LabelControlBase : ReactiveUserControl<LabelControlViewModel>
	{
	}

	public sealed partial class LabelControl : LabelControlBase, INotifyPropertyChanged
	{
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register(
				"Label",
				typeof(string),
				typeof(LabelControl),
				null
			);

		public string Label
		{
			get => (string) GetValue(LabelProperty);
			set => SetValue(LabelProperty, value);
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof(string),
				typeof(LabelControl),
				null);

		public string? Value
		{
			get => (string) GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register(
				"Description",
				typeof(string),
				typeof(LabelControl),
				null);

		public string? Description
		{
			get => (string?) GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register(
				"Color",
				typeof(Color),
				typeof(LabelControl),
				new PropertyMetadata(new Color(255, 255, 255)));

		public Color Color
		{
			get => (Color) GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		public static readonly DependencyProperty ScopedLabelValueProperty =
			DependencyProperty.Register(
				"ScopedLabelValue",
				typeof(string),
				typeof(LabelControl),
				new PropertyMetadata(""));

		public string ScopedLabelValue
		{
			get => (string) GetValue(ScopedLabelValueProperty);
			set => SetValue(ScopedLabelValueProperty, value);
		}

		private bool IsAccentColorDark()
		{
			// TODO Extract to ThemeSettings

			var color = Color.ToWindowsUiColor();

			bool isDark = (5 * color.G + 2 * color.R + color.B) <= 8 * 128;
			return isDark;
		}

		public bool IsDarkAccent
		{
			get => IsAccentColorDark();
			set => OnPropertyChanged();
		}

		private readonly ISubject<Label> _onClick;
		public IObservable<Label> OnClick => _onClick;

		public LabelControl()
		{
			this.InitializeComponent();

			_onClick = new Subject<Label>();

			this.WhenAnyValue(x => x.Description)
				.WhereNotNullAndWhitespace()
				.Subscribe(x =>
				{
					var toolTip = new ToolTip {Content = x};
					ToolTipService.SetToolTip(RootGrid, toolTip);
				});

			this.WhenAnyValue(x => x.Color)
				.Subscribe(x => { OnPropertyChanged(nameof(IsDarkAccent)); });

			this.WhenAnyValue(x => x.ScopedLabelValue)
				.Subscribe(x =>
				{
					var result = x.Split("::", 2, StringSplitOptions.RemoveEmptyEntries);

					if (result.Length == 1)
					{
						Label = result[0];
						Value = "";
					}
					else if (result.Length == 2)
					{
						Label = result[0];
						Value = result[1];
					}
				});

			this.WhenAnyValue(x => x.Label, x => x.Value)
				.Subscribe(x => ScopedLabelValue = $"{x.Item1}::{x.Item2}");
		}

		private void Label_Tapped(object sender, TappedRoutedEventArgs e)
		{
			_onClick.OnNext((Label) Tag);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}