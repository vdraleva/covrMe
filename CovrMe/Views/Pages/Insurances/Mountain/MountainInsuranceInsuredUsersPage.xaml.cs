using CovrMe.ViewModels.Pages.Insurances.Mountain;
using CommunityToolkit.Maui.Views;
using CovrMe.Views.Popups;

namespace CovrMe.Views.Pages.Insurances.Mountain;

public partial class MountainInsuranceInsuredUsersPage : ContentPage
{
    public MountainInsuranceInsuredUsersViewModel viewModel;
    public MountainInsuranceInsuredUsersPage(MountainInsuranceInsuredUsersViewModel vm)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
    }

    private void CheckBoxSave_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBoxSave.IsChecked)
        {
            viewModel.IsExtreme = true;
        }
        else
        {
            viewModel.IsExtreme = false;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (CheckBoxSave.IsChecked)
        {
            CheckBoxSave.IsChecked = false;
            viewModel.IsExtreme = false;
        }
        else
        {
            CheckBoxSave.IsChecked = true;
            viewModel.IsExtreme = true;
        }
    }
}