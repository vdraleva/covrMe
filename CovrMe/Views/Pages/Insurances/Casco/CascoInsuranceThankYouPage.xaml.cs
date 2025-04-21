using CovrMe.ViewModels.Pages.Insurances.Casco;

namespace CovrMe.Views.Pages.Insurances.Casco;

public partial class CascoInsuranceThankYouPage : ContentPage
{
    private CascoInsuranceThankYouViewModel viewModel;
    public CascoInsuranceThankYouPage(CascoInsuranceThankYouViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;
    }
}