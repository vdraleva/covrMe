using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceCalendarPage : ContentPage
{
    public TravelInsuranceCalendarViewModel viewmodel;
	public TravelInsuranceCalendarPage(TravelInsuranceCalendarViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}