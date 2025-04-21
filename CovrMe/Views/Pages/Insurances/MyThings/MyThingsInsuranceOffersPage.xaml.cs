using CovrMe.ViewModels.Pages.Insurances.MyThings;

namespace CovrMe.Views.Pages.Insurances.MyThings;

public partial class MyThingsInsuranceOffersPage : ContentPage
{
    public MyThingsInsuranceOffersViewModel viewModel;
    public MyThingsInsuranceOffersPage(MyThingsInsuranceOffersViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}