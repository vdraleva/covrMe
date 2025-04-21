 using CovrMe.Models;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class CivilInsuranceVehiclePage : ContentPage
{
    private CivilInsuranceVehicleViewModel viewModel;
    private bool _startUpdatePlateNumber;
    private bool _startUpdateCertificateNumber;

    private bool _updatedPlateNumber;
    private bool _updatedCertificateNumber;
    public CivilInsuranceVehiclePage(CivilInsuranceVehicleViewModel vm, Dictionary<string, object> parameters)
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
    private void PlateNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string oldText = e.OldTextValue;
        string newText = e.NewTextValue;

        if (oldText != null && oldText != newText && !this._startUpdateCertificateNumber && !this._updatedCertificateNumber)
        {
            this._startUpdatePlateNumber = true;
            viewModel.RegistrationCertificateNumber = string.Empty;
        }
    }
    private void RegistrationCertificateNumEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string oldText = e.OldTextValue;
        string newText = e.NewTextValue;

        if (oldText != null && oldText != newText && !this._startUpdatePlateNumber && !this._updatedPlateNumber)
        {
            this._startUpdateCertificateNumber = true;
            viewModel.VehiclePlateNumber = string.Empty;
        }
    }
    private void RegistrationCertificateNumEntry_Unfocused(object sender, FocusEventArgs e)
    {
        this._startUpdateCertificateNumber = false;

        if (!string.IsNullOrEmpty(viewModel.VehiclePlateNumber) && !string.IsNullOrEmpty(viewModel.RegistrationCertificateNumber))
        {
            this._updatedPlateNumber = false;
            this._updatedCertificateNumber = false;
        }
        else
        {
            this._updatedCertificateNumber = true;
        }
    }
    private void PlateNumberEntry_Unfocused(object sender, FocusEventArgs e)
    {
        this._startUpdatePlateNumber = false;

        if (!string.IsNullOrEmpty(viewModel.VehiclePlateNumber) && !string.IsNullOrEmpty(viewModel.RegistrationCertificateNumber))
        {
            this._updatedPlateNumber = false;
            this._updatedCertificateNumber = false;
        }
        else
        {
            this._updatedPlateNumber = true;
        }

    }
}