using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class SuccessfulRegistrationPage : ContentPage
{
	public SuccessfulRegistrationPage(SuccessfulRegistrationPageViewModel vm)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
    }
}