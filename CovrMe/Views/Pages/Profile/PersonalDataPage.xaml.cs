using CovrMe.Models.Locations.Result;

namespace CovrMe.ViewModels.Pages.Profile;

public partial class PersonalDataPage : ContentPage
{
    PersonalDataViewModel viewModel;
	public PersonalDataPage(PersonalDataViewModel vm)
	{
		InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;       
    }

    #region TogglePasswords
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
    private void ToggleOldPasswordVisibility(object sender, EventArgs e)
    {
        OldPasswordEntry.IsPassword = !OldPasswordEntry.IsPassword;

        ImageButton eyeButton = (ImageButton)sender;
        string imageSource = OldPasswordEntry.IsPassword ? "hide.png" : "eye.png";
        eyeButton.Source = imageSource;
    }
    #endregion

    #region Collections
    private async void RegionPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = RegionPicker.SelectedItem as LocationDataModel;

        if (selectedOption != null)
        {
            viewModel.SelectedRegion = selectedOption;

            if (!viewModel.UpdatedRegionOnLoading)
            {
                viewModel.SelectedMunicipality = null;
                viewModel.SelectedCityModel = null;
            }
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

        if (selectedOption != null)
        {
            viewModel.SelectedCityModel = selectedOption;
        }
        else
        {
            viewModel.SelectedCityModel = null;
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
        }

    }
    #endregion

}