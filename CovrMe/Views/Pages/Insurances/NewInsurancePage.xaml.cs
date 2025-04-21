using CovrMe.ViewModels.Pages.Insurances;

namespace CovrMe.Views.Pages.Insurances;

public partial class NewInsurancePage : ContentPage
{
	public NewInsurancePage(NewInsurancePageViewModel vm)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
    }
}