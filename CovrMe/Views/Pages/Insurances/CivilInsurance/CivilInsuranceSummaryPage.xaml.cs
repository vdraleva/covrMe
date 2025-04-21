using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;

namespace CovrMe.Views.Pages.Insurances.CivilInsurance;

public partial class CivilInsuranceSummaryPage : ContentPage
{
    public CivilInsuranceSummaryViewModel viewmodel;
    public CivilInsuranceSummaryPage(CivilInsuranceSummaryViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;
        
        vm.ApplyQueryAttributes(parameters);

        continueBtn.IsEnabled = false;
    }

    private void CheckBoxTermsEntry_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBoxTermsEntry.IsChecked)
        {
            continueBtn.IsEnabled = true;
        }
        else
        {
            continueBtn.IsEnabled = false;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (CheckBoxTermsEntry.IsChecked)
        {
            CheckBoxTermsEntry.IsChecked = false;
            continueBtn.IsEnabled = false;
        }
        else
        {
            CheckBoxTermsEntry.IsChecked = true;
            continueBtn.IsEnabled = true;
        }
    }
}