
using CovrMe.ViewModels.Pages;
using Maui.Plugins.PageResolver;

namespace CovrMe.Views.Pages;

public partial class HomePage : ContentPage
{
    private HomePageViewModel viewModel;
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        this.BindingContext = vm;

        Task.Run(async () => { await GetUserInsurances(); }).Wait();

    }

    private async Task GetUserInsurances()
    {
        if (!string.IsNullOrEmpty(App.JwtToken))
        {
            await viewModel.UserInsurances();
        }
        
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}