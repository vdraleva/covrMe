using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceOwnerPage : ContentPage
{
    public TravelInsuranceOwnerViewModel viewModel;
    public TravelInsuranceOwnerPage(TravelInsuranceOwnerViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);

        CheckBoxSave.IsChecked = true;
        CheckBoxInsurer.IsChecked = true;
    }

    private async void UserPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (selectedOption != null)
        {
            if (selectedOption.Names == GlobalConstants.New)
            {
                viewModel.FirstName = string.Empty;
                viewModel.LastName = string.Empty;
                viewModel.SurName = string.Empty;
                viewModel.Uin = string.Empty;
                viewModel.VatNumber = string.Empty;
                viewModel.Address = string.Empty;
                viewModel.BirthDate = DateTime.Parse("01/01/1990");
                viewModel.CompanyName = string.Empty;
                viewModel.Email = string.Empty;
                viewModel.Phone = string.Empty;

                viewModel.SelectedCityModel = null;
                viewModel.SelectedMunicipality = null;
                viewModel.SelectedNationality = null;
                viewModel.SelectedRegion = null;
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
                viewModel.SelectedRegion = null;
                viewModel.SelectedMunicipality = null;
                viewModel.SelectedCityModel = null;
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
                viewModel.SelectedMunicipality = null;
                viewModel.SelectedCityModel = null;
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
                viewModel.SelectedCityModel = selectedOption;
            }
            else
            {
                viewModel.SelectedCityModel = null;
            }
        }
    }

    private void NationalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = NationalityPicker.SelectedItem as CountryModel;
        viewModel.PopulateBirthDate();
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

    private void SaveTapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
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

    private void CheckBoxInsurer_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBoxInsurer.IsChecked)
        {
            viewModel.IsInsured = true;
        }
        else
        {
            viewModel.IsInsured = false;
        }
    }

    private void InsurerTapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (CheckBoxInsurer.IsChecked)
        {
            CheckBoxInsurer.IsChecked = false;
            viewModel.IsInsured = false;
        }
        else
        {
            CheckBoxInsurer.IsChecked = true;
            viewModel.IsInsured = true;
        }
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        viewModel.PopulateBirthDate();
    }
}