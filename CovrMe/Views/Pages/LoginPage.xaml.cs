using CovrMe.ViewModels.Pages;

namespace CovrMe.Views.Pages;

public partial class LoginPage : ContentPage
{
    private LoginPageViewModel viewModel;
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.BindingContext = vm;
        PasswordEntry.IsPassword = true;
        this.viewModel = vm;

        Task.Run(async () => { await AutoLogin(); }).Wait();
    }
    private void TogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        ImageButton eyeButton = (ImageButton)sender;
        string imageSource = PasswordEntry.IsPassword ? "hide.png" : "eye.png";
        eyeButton.Source = imageSource;
    }

    private async Task AutoLogin()
    {
        var isLogged = this.viewModel.CheckIfUserIsLogged();

        if (isLogged)
        {
            await this.viewModel.AutoLogin();
        }
    }
}