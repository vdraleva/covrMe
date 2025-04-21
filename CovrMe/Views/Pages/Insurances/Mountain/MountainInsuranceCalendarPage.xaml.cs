using CovrMe.Models;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.ViewModels.Pages.Insurances.Mountain;

namespace CovrMe.Views.Pages.Insurances.Mountain;

public partial class MountainInsuranceCalendarPage : ContentPage
{
    public MountainInsuranceCalendarViewModel viewModel;
    public MountainInsuranceCalendarPage(MountainInsuranceCalendarViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}