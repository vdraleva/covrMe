using CovrMe.ViewModels.Pages.Payment;

namespace CovrMe.Views.Pages.Payment;

public partial class PaymentPage : ContentPage
{
	private PaymentPageViewModel viewModel;
    public PaymentPage(PaymentPageViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}