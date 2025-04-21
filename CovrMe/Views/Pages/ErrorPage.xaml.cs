using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class ErrorPage : ContentPage
{
    ErrorPageViewModel viewModel;
    public ErrorPage(ErrorPageViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        viewModel = vm;

        vm.ApplyQueryAttributes(parameters);
    }
}