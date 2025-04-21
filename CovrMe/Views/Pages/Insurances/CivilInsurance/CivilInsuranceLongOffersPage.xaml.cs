using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class CivilInsuranceLongOffersPage : ContentPage
{
    CivilInsuranceLongOffersViewModel viewModel;
    public CivilInsuranceLongOffersPage(CivilInsuranceLongOffersViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }

    private async void oneTimeBtn_Clicked(object sender, EventArgs e)
    {
        await viewModel.BoxToggle("one");
    }

    private async void twoTimeBtn_Clicked(object sender, EventArgs e)
    {
        await viewModel.BoxToggle("two");
    }

    private async void fourTimeBtn_Clicked(object sender, EventArgs e)
    {
        await viewModel.BoxToggle("four");
    }
}