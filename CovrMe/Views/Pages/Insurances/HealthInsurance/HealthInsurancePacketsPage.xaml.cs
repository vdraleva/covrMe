using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;

namespace CovrMe.Views.Pages.Insurances.HealthInsurance;

public partial class HealthInsurancePacketsPage : ContentPage
{
    public HealthInsurancePacketsPageViewModel viewModel;
    public HealthInsurancePacketsPage(HealthInsurancePacketsPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }

    private void RadioBtnPrestige_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnPrestige.IsChecked)
        {
            int value = int.Parse((string)RadioBtnPrestige.Value);
            viewModel.PrestigeCardOpened = true;
            viewModel.PrestigePacketSelected = true;
            viewModel.ComfortPacketSelected = false;

            viewModel.PacketId = value;
            RadioBtnComfort.IsChecked = false;
        }
        else
        {
            viewModel.PrestigeCardOpened = false;
        }
    }

    private void RadioBtnPrestige_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnPrestige.IsChecked)
        {
            RadioBtnPrestige.IsChecked = false;
        }
        else
        {
            RadioBtnPrestige.IsChecked = true;
        }
    }

    private void RadioBtnComfort_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnComfort.IsChecked)
        {
            int value = int.Parse((string)RadioBtnComfort.Value);
            viewModel.ComforteCardOpened = true;
            viewModel.PrestigePacketSelected = false;
            viewModel.ComfortPacketSelected = true;

            viewModel.PacketId = value;
            RadioBtnPrestige.IsChecked = false;
        }
        else
        {
            viewModel.ComforteCardOpened = false;
        }
    }



    private void RadioBtnComfort_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnComfort.IsChecked)
        {
            RadioBtnComfort.IsChecked = false;
        }
        else
        {
            RadioBtnComfort.IsChecked = true;
        }
    }
}