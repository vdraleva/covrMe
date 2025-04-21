using CovrMe.ViewModels.Pages.Insurances.Mountain;

namespace CovrMe.Views.Pages.Insurances.Mountain;

public partial class MountainInsuranceOffersPage : ContentPage
{
    public MountainInsuranceOffersViewModel viewmodel;
    public MountainInsuranceOffersPage(MountainInsuranceOffersViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await viewmodel.OnBorderTapped(e.Parameter.ToString());
    }
}