using CovrMe.ViewModels.Pages.Insurances;

namespace CovrMe.Views.Pages.Insurances;

public partial class MyInsurancesPage : ContentPage
{
    private MyInsurancesPageViewModel viewModel;
    public MyInsurancesPage(MyInsurancesPageViewModel vm)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        this.BindingContext = vm;
    }
}