using CovrMe.Models.Vehicles.Result;
using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class CivilInsuranceVehicleAdditionalInfoPage : ContentPage
{
    CivilInsuranceVehicleAdditionalInfoViewModel viewModel;
    public CivilInsuranceVehicleAdditionalInfoPage(CivilInsuranceVehicleAdditionalInfoViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;

        vm.ApplyQueryAttributes(parameters);
        this.viewModel = vm;
        BindingContext = vm;

        CheckBoxSave.IsChecked = true;
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
}