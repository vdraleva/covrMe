using CovrMe.Models;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.Casco;

namespace CovrMe.Views.Pages.Insurances.Casco;

public partial class CascoInsuranceVehiclePage : ContentPage
{
    private CascoInsuranceVehicleViewModel viewModel;
    public CascoInsuranceVehiclePage(CascoInsuranceVehicleViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;

        vm.ApplyQueryAttributes(parameters);
        this.viewModel = vm;
        BindingContext = vm;

        CheckBoxSave.IsChecked = true;
        viewModel.IsUpdatingOnLoading = false;
    }

    private async void UserVehiclesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = UserVehiclesPicker.SelectedItem as UserVehiclesPickerModel;

        if (selectedOption != null)
        {
            if (selectedOption.VehicleId == GlobalConstants.New)
            {
                viewModel.RegistrationCertificateNumber = string.Empty;
                viewModel.VehiclePlateNumber = string.Empty;
                viewModel.FirstRegistrationDate = DateTime.Now;
                viewModel.SelectedVehicleBrand = null;
                viewModel.SelectedVehicleModel = null;
                viewModel.SelectedVehicleType = null;
                viewModel.SelectedVehicleUsage = null;
            }
            else
            {
                if (!viewModel.IsUpdatingCollection && !viewModel.IsUpdatingOnLoading)
                {
                    await viewModel.ChangeUserVehicle(selectedOption);
                }
            }
        }
    }

    private async void VehiclesBrandsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedVehicleBrandOption = VehiclesBrandsPicker.SelectedItem as BaseDataModel;
        var selectedUserVehicleOption = UserVehiclesPicker.SelectedItem as UserVehiclesPickerModel;

        if (selectedUserVehicleOption != null && selectedVehicleBrandOption != null && selectedVehicleBrandOption.Id != selectedUserVehicleOption.VehicleBrandId && viewModel.RegCertificateModel == null)
        {
            await viewModel.GetVehicleModels(selectedVehicleBrandOption);
        }
        else if (selectedVehicleBrandOption != null && selectedUserVehicleOption == null && viewModel.RegCertificateModel == null)
        {
            await viewModel.GetVehicleModels(selectedVehicleBrandOption);
        }
        else
        {
            if (!viewModel.IsUpdatingOnLoading && viewModel.RegCertificateModel == null)
            {
                viewModel.SelectedVehicleModel = null;
            }
        }
    }

    private void CheckBoxSave_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBoxSave.IsChecked)
        {
            viewModel.Save = true;
        }
        else
        {
            viewModel.Save = false;
        }
    }


    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (CheckBoxSave.IsChecked)
        {
            CheckBoxSave.IsChecked = false;
            viewModel.Save = false;
        }
        else
        {
            CheckBoxSave.IsChecked = true;
            viewModel.Save = true;
        }
    }

    private void engineTypesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedEngineType = engineTypesPicker.SelectedItem as BaseDataModel;
        viewModel.ValidateEngineType(selectedEngineType.Id);
    }
}