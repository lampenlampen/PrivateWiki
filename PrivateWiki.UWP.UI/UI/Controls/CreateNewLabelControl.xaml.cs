using System.Drawing;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls.Primitives;
using PrivateWiki.Utilities;
using PrivateWiki.UWP.UI.Utilities.ExtensionFunctions;
using PrivateWiki.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls
{
	public abstract class CreateNewLabelBaseControl : ReactiveUserControl<CreateNewLabelControlViewModel> { }

	public sealed partial class CreateNewLabelControl : CreateNewLabelBaseControl
	{
		public CreateNewLabelControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				this.Bind(ViewModel,
						vm => vm.ScopedLabelValue,
						view => view.LabelTextBox.Text)
					.DisposeWith(disposable);

				this.BindValidation(ViewModel,
						vm => vm.ValidLabelRule,
						view => view.LabelTextBoxError.Text)
					.DisposeWith(disposable);

				this.Bind(ViewModel,
						vm => vm.Description,
						view => view.DescriptionTextBox.Text)
					.DisposeWith(disposable);

				this.Bind(ViewModel, vm => vm.ColorHexString,
						view => view.ColorBox.Text)
					.DisposeWith(disposable);

				ColorPickerFlyout.Events().Closing
					.Select(_ =>
					{
						var color = ColorPicker.Color;
						return Color.FromArgb(255, color.R, color.G, color.B).ToHexColor();
					})
					.BindTo(ViewModel, vm => vm.ColorHexString)
					.DisposeWith(disposable);

				this.OneWayBind(ViewModel, vm => vm.Color,
						view => view.ColorPicker.Color,
						color => color.ToWindowsUiColor())
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.Color)
					.Select(color => color.ToHexColor())
					.BindTo(ColorBox, view => view.Text)
					.DisposeWith(disposable);

				this.OneWayBind(ViewModel,
						vm => vm.Color,
						view => view.LabelPreview.Color)
					.DisposeWith(disposable);

				CreateLabelBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, vm => vm.CreateLabel)
					.DisposeWith(disposable);

				CancelBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, vm => vm.Cancel)
					.DisposeWith(disposable);
			});
		}
	}
}