using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;

namespace CovrMe.Views.Pages.Insurances.HealthInsurance;

public partial class HealthInsuranceSummaryPage : ContentPage
{
    public HealthInsuranceSummaryPageViewModel viewModel;
    public HealthInsuranceSummaryPage(HealthInsuranceSummaryPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;

        vm.ApplyQueryAttributes(parameters);
    }
}