using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;

namespace CovrMe.Views.Pages.Insurances.HealthInsurance;

public partial class HealthInsuranceOffersPage : ContentPage
{
    public HealthInsuranceOffersPageViewModel viewModel;
    public HealthInsuranceOffersPage(HealthInsuranceOffersPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);

        if (vm.OffersCollection.Count == 0)
        {
            noOffersLabel.IsVisible = true;
            continueBtn.IsEnabled = false;
        }
        else
        {
            noOffersLabel.IsVisible = false;
            continueBtn.IsEnabled = true;
        }
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

    private async void twelveTimeBtn_Clicked(object sender, EventArgs e)
    {
        await viewModel.BoxToggle("twelve");
    }
}