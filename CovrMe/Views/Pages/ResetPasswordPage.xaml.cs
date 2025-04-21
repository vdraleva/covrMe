using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage(ResetPasswordPageViewModel vm)
	{

        InitializeComponent();
        vm.Navigation = Navigation;
        this.BindingContext = vm;
    }
    private void TogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        ImageButton eyeButton = (ImageButton)sender;
        string imageSource = PasswordEntry.IsPassword ? "hide.png" : "eye.png";
        eyeButton.Source = imageSource;
    }
    private void ToggleConfirmPasswordVisibility(object sender, EventArgs e)
    {
        ConfirmPassword.IsPassword = !ConfirmPassword.IsPassword;

        ImageButton eyeButton = (ImageButton)sender;
        string imageSource = ConfirmPassword.IsPassword ? "hide.png" : "eye.png";
        eyeButton.Source = imageSource;
    }
}