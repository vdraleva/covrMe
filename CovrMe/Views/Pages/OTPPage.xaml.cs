using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class OTPPage : ContentPage
{
    private OTPPageViewModel viewModel;
    public OTPPage(OTPPageViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.BindingContext = vm;
        this.viewModel = vm;
        InitializeEntryFocusBehavior();
    }
    private void InitializeEntryFocusBehavior()
    {
        Digit1.TextChanged += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length == 1)
                Digit2.Focus();
        };

        Digit2.TextChanged += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length == 1)
                Digit3.Focus();
        };

        Digit3.TextChanged += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length == 1)
                Digit4.Focus();
        };

        Digit4.TextChanged += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length == 1)
            {
                Digit4.Unfocus();
            }
        };
    }
}