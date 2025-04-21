using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;
using CovrMe.Views.ContentViews;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceInsuredUsersPage : ContentPage
{
    public TravelInsuranceInsuredUsersViewModel viewmodel;
    public TravelInsuranceInsuredUsersPage(TravelInsuranceInsuredUsersViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}