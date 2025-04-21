using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class ThankYouPage : ContentPage
{
    ThankYouPageViewModel viewModel;
    public ThankYouPage(ThankYouPageViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        viewModel = vm;
    }
}