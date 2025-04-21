using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.Casco;

namespace CovrMe.Views.Pages.Insurances.Casco;

public partial class CascoInsuranceOwnerPage : ContentPage
{
    private CascoInsuranceOwnerViewModel viewModel;
    public CascoInsuranceOwnerPage(CascoInsuranceOwnerViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;

        vm.ApplyQueryAttributes(parameters);
        this.viewModel = vm;
        BindingContext = vm;

        CheckBoxSave.IsChecked = true;
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
                viewModel.Email = string.Empty;
                viewModel.Phone = string.Empty;
                viewModel.Uin = string.Empty;
                viewModel.BirthDate = DateTime.Parse("01/01/1990");
                viewModel.SelectedDrivingExperience = null;
            }
            else
            {
                await viewModel.ChangeUser(selectedOption);
            }
        }
    }
    private void ExpPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = ExpPicker.SelectedItem as DrivingExperiencePickerModel;
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
    private void NationalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = NationalityPicker.SelectedItem as CountryModel;

        viewModel.PopulateBirthDate();
    }
}