using CovrMe.ViewModels.Pages.Insurances.MyThings;

namespace CovrMe.Views.Pages.Insurances.MyThings;

public partial class MyThingsInsuranceCalendarPage : ContentPage
{
    public MyThingsInsuranceCalendarPageViewModel viewModel;
    public MyThingsInsuranceCalendarPage(MyThingsInsuranceCalendarPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}