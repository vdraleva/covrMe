using CovrMe.ViewModels.Pages.Speedy;
using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Speedy;

public partial class SpeedyDeliveryUserAddressPage : ContentPage
{
    SpeedyDeliveryUserAddressViewModel viewModel;
    public SpeedyDeliveryUserAddressPage(SpeedyDeliveryUserAddressViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        viewModel = vm;
        vm.ApplyQueryAttributes(parameters);
    }
    private async void RegionPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = RegionPicker.SelectedItem as LocationDataModel;

        if (selectedOption != null)
        {
            viewModel.SelectedRegion = selectedOption;
            await viewModel.GetMunicipality(selectedOption);
        }
        else
        {
            viewModel.SelectedRegion = null;
            viewModel.SelectedMunicipality = null;
            viewModel.SelectedCityModel = null;
        }
         
    }
    private void CityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = CityPicker.SelectedItem as CityDataModel;

        if(selectedOption != null)
        {
            viewModel.SelectedCityModel = selectedOption;
            viewModel.PostCode = selectedOption.PostCode;
        }
        else
        {
            viewModel.SelectedCityModel = null;
            viewModel.PostCode = string.Empty;
        }
        
    }
    private async void MunicipalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = MunicipalityPicker.SelectedItem as LocationDataModel;

        if (selectedOption != null)
        {
            viewModel.SelectedMunicipality = selectedOption;
            await viewModel.GetCities(selectedOption);
        }
        else
        {
            viewModel.SelectedMunicipality = null;
            viewModel.SelectedCityModel = null;
            viewModel.PostCode = string.Empty;
        }
            
    }
}