using CommunityToolkit.Maui.Views;
using Controls.UserDialogs.Maui;
using CovrMe.ViewModels.Pages.Insurances;
using CovrMe.Views.ContentViews;

namespace CovrMe.Views.Pages.Insurances;

public partial class ScanOrFillVehicleDataPage : ContentPage
{
    private ScanOrFillVehicleDataPageViewModel viewModel;

    public ScanOrFillVehicleDataPage(ScanOrFillVehicleDataPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;

        vm.ApplyQueryAttributes(parameters);
        this.viewModel = vm;
        this.BindingContext = vm;
    }
}