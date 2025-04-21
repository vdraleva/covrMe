using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class ExistingCivilInsurancePage : ContentPage
{
    ExistingCivilInsurancePageViewModel viewModel;
    public ExistingCivilInsurancePage(ExistingCivilInsurancePageViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}