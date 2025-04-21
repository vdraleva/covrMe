using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage(ForgotPasswordViewModel vm)
	{
		InitializeComponent();
		vm.Navigation = Navigation;
		BindingContext = vm;
	}
}