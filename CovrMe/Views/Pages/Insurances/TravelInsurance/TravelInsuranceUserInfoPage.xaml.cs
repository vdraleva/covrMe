using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceUserInfoPage : ContentPage
{
    public TravelInsuranceUserInfoViewModel viewModel;
    public TravelInsuranceUserInfoPage(TravelInsuranceUserInfoViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
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