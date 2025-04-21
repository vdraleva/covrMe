using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceDocumentsPage : ContentPage
{
    public TravelInsuranceDocumentsViewModel viewmodel;

    public TravelInsuranceDocumentsPage(TravelInsuranceDocumentsViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;
       
        continueBtn.IsEnabled = false;
        vm.ApplyQueryAttributes(parameters);
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