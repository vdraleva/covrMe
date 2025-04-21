using CovrMe.ViewModels.Pages.Speedy;

namespace CovrMe.Views.Pages.Speedy;

public partial class SpeedyDeliveryOfficePage : ContentPage
{
    SpeedyDeliveryOfficeViewModel viewmodel;
	public SpeedyDeliveryOfficePage(SpeedyDeliveryOfficeViewModel  vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        viewmodel = vm;
        vm.ApplyQueryAttributes(parameters);
    }
}