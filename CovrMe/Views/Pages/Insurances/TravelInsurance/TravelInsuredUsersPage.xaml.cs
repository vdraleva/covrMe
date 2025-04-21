using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuredUsersPage : ContentPage
{
    private TravelInsuredUsersPageViewModel viewModel;
    public TravelInsuredUsersPage(TravelInsuredUsersPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        this.viewModel = vm;
        this.BindingContext = vm;

        vm.ApplyQueryAttributes(parameters);
    }

    private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedOption = picker.SelectedItem as UserFamilyAndFriendsPicker;
        var index = int.Parse(picker.ClassId);

        if (selectedOption != null)
        {
            if (selectedOption.Names == GlobalConstants.New)
            {
                await viewModel.EmptySelectedUserFields(index);
            }
            else
            {
                await viewModel.ChangeUser(selectedOption, index);
            }
        }

        picker.Unfocus();
    }

    private void CheckBoxSave_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var index = int.Parse(checkbox.ClassId);
        if (checkbox.IsChecked)
        {
            var currentUser = viewModel.InsuredUsers.FirstOrDefault(x => x.Index == index);
            currentUser.Save = true;
        }
        else
        {
            var currentUser = viewModel.InsuredUsers.FirstOrDefault(x => x.Index == index);
            currentUser.Save = false;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var grid = (Grid)sender;
        var index = int.Parse(grid.ClassId);

        var currentUser = viewModel.InsuredUsers.FirstOrDefault(x => x.Index == index);

        if (currentUser.Save)
        {
            currentUser.Save = false;
        }
        else
        {
            currentUser.Save = true;
        }
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        var entry = (Entry)sender;
        var index = int.Parse(entry.ClassId);
        viewModel.PopulateBirthDate(index);
    }

    private void NationalityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var index = int.Parse(picker.ClassId);
        viewModel.PopulateBirthDate(index);
    }
}