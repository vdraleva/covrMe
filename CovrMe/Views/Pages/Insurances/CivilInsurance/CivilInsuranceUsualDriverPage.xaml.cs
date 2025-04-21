using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class CivilInsuranceUsualDriverPage : ContentPage
{
    CivilInsuranceUsualDriverViewModel viewModel;
    public CivilInsuranceUsualDriverPage(CivilInsuranceUsualDriverViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
        CheckBoxSave.IsChecked = true;
    }

    private async void UserPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (selectedOption != null)
        {
            if (selectedOption.Names == GlobalConstants.New)
            {
                viewModel.UsualFirstName = string.Empty;
                viewModel.UsualLastName = string.Empty;
                viewModel.UsualSurName = string.Empty;
                viewModel.UsualUin = string.Empty;
                viewModel.UsualAddress = string.Empty;
                viewModel.UsualBirthDate = DateTime.Parse("01/01/1990"); ;

                viewModel.SelectedCityModelUsual = null;
                viewModel.SelectedDrivingExperienceUsual = null;
                viewModel.SelectedMunicipalityUsual = null;
                viewModel.SelectedNationalityUsual = null;
                viewModel.SelectedRegionUsual = null;
                viewModel.SelectedGuiltType = null;
            }
            else
            {
                //if (!viewModel.IsUpdatingCollection)
                //{
                    await viewModel.ChangeUser(selectedOption);
                //}
                    
            }
        }
    }

    private async void RegionPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = RegionPicker.SelectedItem as LocationDataModel;
        var userSelectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (!viewModel.IsUpdatingCollection)
        {
            if (selectedOption != null && selectedOption.Name != userSelectedOption.Region)
            {
                await viewModel.GetMunicipality(selectedOption);
            }
            else if (selectedOption != null && userSelectedOption == null)
            {
                await viewModel.GetMunicipality(selectedOption);
            }
            else
            {
                viewModel.SelectedRegionUsual = null;
                viewModel.SelectedMunicipalityUsual = null;
                viewModel.SelectedCityModelUsual = null;
            }
        }
        else
        {
            if (selectedOption != null && selectedOption.Id != userSelectedOption.Region)
            {
                userSelectedOption.Region = selectedOption.Id;
                await viewModel.GetMunicipality(selectedOption);
            }
        }

    }

    private async void MunicipalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = MunicipalityPicker.SelectedItem as LocationDataModel;
        var userSelectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (!viewModel.IsUpdatingCollection)
        {
            if (userSelectedOption != null && selectedOption != null && selectedOption.Name != userSelectedOption.Municipality)
            {
                await viewModel.GetCities(selectedOption);
            }
            else if (selectedOption != null && userSelectedOption == null)
            {
                await viewModel.GetCities(selectedOption);
            }
            else
            {
                viewModel.SelectedMunicipalityUsual = null;
                viewModel.SelectedCityModelUsual = null;
            }
        }
        else
        {
            if (userSelectedOption != null && selectedOption != null && selectedOption.Id != userSelectedOption.Municipality)
            {
                userSelectedOption.Municipality = selectedOption.Id;
                await viewModel.GetCities(selectedOption);
            }
        }

    }

    private void CityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = CityPicker.SelectedItem as CityDataModel;

        if (!viewModel.IsUpdatingCollection)
        {
            if (selectedOption != null)
            {
                viewModel.SelectedCityModelUsual = selectedOption;
            }
            else
            {
                viewModel.SelectedCityModelUsual = null;
            }
        }
    }

    private void ExpPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = ExpPicker.SelectedItem as DrivingExperiencePickerModel;
    }

    private void NationalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = NationalityPicker.SelectedItem as CountryModel;
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

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        viewModel.PopulateBirthDate();
    }
}