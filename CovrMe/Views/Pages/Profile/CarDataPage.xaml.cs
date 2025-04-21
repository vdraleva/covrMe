using CovrMe.Models;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Profile;

namespace CovrMe.Views.Pages.Profile;

public partial class CarDataPage : ContentPage
{
    CarDataPageViewModel viewModel;
    private bool _startUpdatePlateNumber;
    private bool _startUpdateCertificateNumber;

    private bool _updatedPlateNumber;
    private bool _updatedCertificateNumber;
	public CarDataPage(CarDataPageViewModel vm)
	{
		InitializeComponent();
        vm.Navigation = Navigation;      
        this.viewModel = vm;
        BindingContext = vm;

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
                viewModel.Vin = null;
                viewModel.VehicleKilowatts = null;
                viewModel.EngineVolume = null;
                viewModel.NetWeight = null;
                viewModel.GrossWeight = null;
                viewModel.SelectedBodyType = null;
                viewModel.SelectedColor = null;
                viewModel.SelectedEngineType = null;
                viewModel.SelectedSteeringWheel = null;
                viewModel.Places = null;
            }
            else
            {
                if (!viewModel.IsUpdatingOnLoading && !viewModel.IsUpdatingCollection)
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

        if (selectedUserVehicleOption != null && selectedVehicleBrandOption != null && selectedVehicleBrandOption.Id != selectedUserVehicleOption.VehicleBrandId)
        {
            await viewModel.GetVehicleModels(selectedVehicleBrandOption);
        }
        else if (selectedVehicleBrandOption != null && selectedUserVehicleOption == null)
        {
            await viewModel.GetVehicleModels(selectedVehicleBrandOption);
        }
        else
        {
            if (!viewModel.IsUpdatingOnLoading)
            {
                viewModel.SelectedVehicleModel = null;
            }              
        }
    }

    private void vehicleTypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedVehicleTypeOption = VehicleTypePicker.SelectedItem as BaseDataModel;

        if(selectedVehicleTypeOption != null)
        {
            if (selectedVehicleTypeOption.Id == 7 || selectedVehicleTypeOption.Id == 4 || selectedVehicleTypeOption.Id == 11 ||
                            selectedVehicleTypeOption.Id == 6 || selectedVehicleTypeOption.Id == 10)
            {
                viewModel.ShowWeightFields = true;
            }
            else
            {
                viewModel.ShowWeightFields = false;
            }
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

        if(!string.IsNullOrEmpty(viewModel.VehiclePlateNumber) && !string.IsNullOrEmpty(viewModel.RegistrationCertificateNumber))
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