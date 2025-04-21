using CovrMe.ViewModels.Pages.Insurances.MyThings;

namespace CovrMe.Views.Pages.Insurances.MyThings;

public partial class MyThingsCharacteristicsPage : ContentPage
{
    public MyThingsCharacteristicsPageViewModel viewModel;

    public MyThingsCharacteristicsPage(MyThingsCharacteristicsPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;

        vm.ApplyQueryAttributes(parameters);
    }

    private void RadioElectricYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioElectricYes.IsChecked)
        {
            viewModel.IsElectric = true;
            RadioBtnElectricNo.IsChecked = false;
        }
        else
        {
            viewModel.IsElectric = false;
        }
    }

    private void LabelElectricYes_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioElectricYes.IsChecked)
        {
            RadioElectricYes.IsChecked = false;
        }
        else
        {
            RadioElectricYes.IsChecked = true;
        }
    }

    private void RadioBtnElectricNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnElectricNo.IsChecked)
        {
            viewModel.IsElectric = false;
            RadioElectricYes.IsChecked = false;
        }
        else
        {
            viewModel.IsElectric = false;
        }
    }

    private void LabelElectricNo_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnElectricNo.IsChecked)
        {
            RadioBtnElectricNo.IsChecked = false;
        }
        else
        {
            RadioBtnElectricNo.IsChecked = true;
        }
    }

    private void RadioHasDamageYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioHasDamageYes.IsChecked)
        {
            viewModel.HasDamamge = true;
            RadioBtnHasDamageNo.IsChecked = false;
        }
        else
        {
            viewModel.IsElectric = false;
        }
    }

    private void LabelHasDamageYes_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioHasDamageYes.IsChecked)
        {
            RadioHasDamageYes.IsChecked = false;
        }
        else
        {
            RadioHasDamageYes.IsChecked = true;
        }
    }

    private void RadioBtnHasDamageNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnHasDamageNo.IsChecked)
        {
            viewModel.HasDamamge = false;
            RadioHasDamageYes.IsChecked = false;
        }
        else
        {
            viewModel.IsElectric = false;
        }
    }

    private void LabelHasDamageNo_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnHasDamageNo.IsChecked)
        {
            RadioBtnHasDamageNo.IsChecked = false;
        }
        else
        {
            RadioBtnHasDamageNo.IsChecked = true;
        }
    }

    private void RadioUnrepairedYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioUnrepairedYes.IsChecked)
        {
            viewModel.HasUnrepairedDamage = true;
            RadioBtnUnrepairedNo.IsChecked = false;
        }
        else
        {
            viewModel.IsElectric = false;
        }
    }

    private void LabelUnrepairedYes_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioUnrepairedYes.IsChecked)
        {
            RadioUnrepairedYes.IsChecked = false;
        }
        else
        {
            RadioUnrepairedYes.IsChecked = true;
        }
    }

    private void RadioBtnUnrepairedNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnUnrepairedNo.IsChecked)
        {
            viewModel.HasUnrepairedDamage = false;
            RadioUnrepairedYes.IsChecked = false;
            viewModel.UnrepairedInfo = string.Empty;
        }
        else
        {
            viewModel.IsElectric = false;
        }
    }

    private void LabelUnrepairedNo_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnUnrepairedNo.IsChecked)
        {
            RadioBtnUnrepairedNo.IsChecked = false;
        }
        else
        {
            RadioBtnUnrepairedNo.IsChecked = true;
        }
    }
}