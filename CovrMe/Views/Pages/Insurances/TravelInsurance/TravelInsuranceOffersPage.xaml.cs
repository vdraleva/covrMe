using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceOffersPage : ContentPage
{
    public TravelInsuranceOffersViewModel viewmodel;
    public TravelInsuranceOffersPage(TravelInsuranceOffersViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);

        if(viewmodel.OffersCollection.Count == 0 || viewmodel.OffersCollection == null)
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

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await viewmodel.OnBorderTapped(e.Parameter.ToString());
    }
}