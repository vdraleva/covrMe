using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel vm)
	{
		InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        RegBtn.IsEnabled = false;

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

    private void CheckBoxTermsEntry_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(CheckBoxTermsEntry.IsChecked && CheckBoxPersonalEntry.IsChecked)
        {
            RegBtn.IsEnabled = true;
        }
        else
        {
            RegBtn.IsEnabled = false;
        }
    }

    private void CheckBoxPersonalEntry_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBoxTermsEntry.IsChecked && CheckBoxPersonalEntry.IsChecked)
        {
            RegBtn.IsEnabled = true;
        }
        else
        {
            RegBtn.IsEnabled = false;
        }
    }
}