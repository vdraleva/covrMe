using CovrMe.ViewModels.Pages.Insurances.Mountain;

namespace CovrMe.Views.Pages.Insurances.Mountain;

public partial class MountainInsuranceSummaryPage : ContentPage
{
    public MountainInsuranceSummaryViewModel viewModel;
    public MountainInsuranceSummaryPage(MountainInsuranceSummaryViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;

        vm.ApplyQueryAttributes(parameters);
    }
}