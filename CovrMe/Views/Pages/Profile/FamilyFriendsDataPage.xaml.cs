using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Profile;

namespace CovrMe.Views.Pages.Profile;

public partial class FamilyFriendsDataPage : ContentPage
{
    FamilyFriendsDataPageViewModel viewModel;
    public FamilyFriendsDataPage(FamilyFriendsDataPageViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        BindingContext = vm;

        viewModel.IsUpdatingOnLoading = false;
    }

    #region Collections

    private async void UserPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (selectedOption != null)
        {
            if (selectedOption.Names == GlobalConstants.New)
            {
                viewModel.FullName = string.Empty;
                viewModel.FullNameEng = string.Empty;
                viewModel.Uin = string.Empty;
                viewModel.Address = string.Empty;
                viewModel.LatinAddress = string.Empty;
                viewModel.BirthDate = DateTime.Parse("01/01/1990");

                viewModel.SelectedCityModel = null;
                viewModel.SelectedMunicipality = null;
                viewModel.SelectedRegion = null;
            }
            else
            {
                if (!viewModel.IsUpdatingCollection && !viewModel.IsUpdatingOnLoading)
                {
                    await viewModel.ChangeUser(selectedOption);
                }
            }
        }
    }
    private async void RegionPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = RegionPicker.SelectedItem as LocationDataModel;
        var userSelectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (userSelectedOption != null)
        {
            if (!viewModel.IsUpdatingCollection && !viewModel.IsUpdatingOnLoading)
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
        }
        else
        {
            await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.EnterUserInfo, App.MESSAGE_OK);
        }
    }
    private void CityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = CityPicker.SelectedItem as CityDataModel;

        if (!viewModel.IsUpdatingCollection && !viewModel.IsUpdatingOnLoading)
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
    private async void MunicipalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = MunicipalityPicker.SelectedItem as LocationDataModel;
        var userSelectedOption = UserPicker.SelectedItem as UserFamilyAndFriendsPicker;

        if (!viewModel.IsUpdatingCollection && !viewModel.IsUpdatingOnLoading)
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
    #endregion

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        viewModel.PopulateBirthDate();
    }

    private void NationalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = NationalityPicker.SelectedItem as CountryModel;

        viewModel.PopulateBirthDate();
    }
}