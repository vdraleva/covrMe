using CovrMe.ViewModels.Pages.Payment;

namespace CovrMe.Views.Pages.Payment;

public partial class PrePaymentPage : ContentPage
{
    PrePaymentPageViewModel viewModel;
    public PrePaymentPage(PrePaymentPageViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}