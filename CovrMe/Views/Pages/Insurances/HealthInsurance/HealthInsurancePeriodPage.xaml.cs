using CovrMe.Models;
using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;

namespace CovrMe.Views.Pages.Insurances.HealthInsurance;

public partial class HealthInsurancePeriodPage : ContentPage
{
    public HealthInsurancePeriodPageViewModel viewModel;
    public HealthInsurancePeriodPage(HealthInsurancePeriodPageViewModel vm, Dictionary<string, object> parameters)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }

    //private void YearPicker_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    var selectedOption = YearPicker.SelectedItem as YearsPickerModel;

    //    if(selectedOption != null)
    //    {
    //        viewModel.SelectedYear = selectedOption;
    //        viewModel.PopulateMonthsCollection(selectedOption.Year);
    //    }
    //}

    //private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    var selectedOption = MonthPicker.SelectedItem as MonthsPickerModel;

    //    if (selectedOption != null)
    //    {
    //        viewModel.SelectedMonth = selectedOption;
    //        viewModel.SetStartEndDate();
    //    }
    //}
}