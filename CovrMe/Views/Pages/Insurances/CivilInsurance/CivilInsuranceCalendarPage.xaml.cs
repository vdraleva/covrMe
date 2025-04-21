using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;
using CovrMe.Views.ContentViews;
using System.ComponentModel;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class CivilInsuranceCalendarPage : ContentPage
{
    public CivilInsuranceCalendarPageViewModel viewmodel;

    public CivilInsuranceCalendarPage(CivilInsuranceCalendarPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        viewmodel = vm;
        vm.ApplyQueryAttributes(parameters);
    }

}