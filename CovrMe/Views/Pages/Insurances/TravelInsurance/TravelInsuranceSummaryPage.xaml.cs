using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceSummaryPage : ContentPage
{
    public TravelInsuranceSummaryViewModel viewModel;
    public TravelInsuranceSummaryPage(TravelInsuranceSummaryViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;

        vm.ApplyQueryAttributes(parameters);
    }
}