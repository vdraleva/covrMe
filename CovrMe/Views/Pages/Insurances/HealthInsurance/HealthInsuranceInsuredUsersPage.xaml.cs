using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;

namespace CovrMe.Views.Pages.Insurances.HealthInsurance;

public partial class HealthInsuranceInsuredUsersPage : ContentPage
{
    public HealthInsuranceInsuredUsersViewModel viewModel;
    public HealthInsuranceInsuredUsersPage(HealthInsuranceInsuredUsersViewModel vm)
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
            viewModel.IsFamily = true;
        }
        else
        {
            viewModel.IsFamily = false;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (CheckBoxSave.IsChecked)
        {
            CheckBoxSave.IsChecked = false;
            viewModel.IsFamily = false;
        }
        else
        {
            CheckBoxSave.IsChecked = true;
            viewModel.IsFamily = true;
        }
    }

    private void agesList_BindingContextChanged(object sender, EventArgs e)
    {
        var viewModel = BindingContext as HealthInsuranceInsuredUsersViewModel;
        if (viewModel != null)
        {
            CheckBoxSave.IsChecked = viewModel.InsuredUsersAgesList.Count > 1;
        }
    }
}