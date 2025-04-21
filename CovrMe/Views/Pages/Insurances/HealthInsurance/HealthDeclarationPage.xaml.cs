using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;

namespace CovrMe.Views.Pages.Insurances.HealthInsurance;

public partial class HealthDeclarationPage : ContentPage
{
    public HealthDeclarationPageViewModel viewModel;
    public HealthDeclarationPage(HealthDeclarationPageViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
        vm.ApplyQueryAttributes(parameters);
    }

    private void RadioBtnFirstYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnFirstYes.IsChecked)
        {
            viewModel.FirstQuestionAnswer = true;
            RadioBtnFirstNo.IsChecked = false;
        }
        else
        {
            viewModel.FirstQuestionAnswer = false;
        }
    }

    private void LabelFirstYes_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnFirstYes.IsChecked)
        {
            RadioBtnFirstYes.IsChecked = false;
        }
        else
        {
            RadioBtnFirstYes.IsChecked = true;
        }
    }

    private void RadioBtnFirstNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnFirstNo.IsChecked)
        {
            viewModel.FirstQuestionAnswer = false;
            RadioBtnFirstYes.IsChecked = false;
        }
        else
        {
            viewModel.FirstQuestionAnswer = true;
        }
    }

    private void LabelFirstNo_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnFirstNo.IsChecked)
        {
            RadioBtnFirstNo.IsChecked = false;
        }
        else
        {
            RadioBtnFirstNo.IsChecked = true;
        }
    }

    private void RadioBtnSecondYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnSecondYes.IsChecked)
        {
            viewModel.SecondQuestionAnswer = true;
            RadioBtnSecondNo.IsChecked = false;
        }
        else
        {
            viewModel.SecondQuestionAnswer = false;
        }
    }

    private void LabelSecondYes_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnSecondYes.IsChecked)
        {
            RadioBtnSecondYes.IsChecked = false;
        }
        else
        {
            RadioBtnSecondYes.IsChecked = true;
        }
    }

    private void RadioBtnSecondNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnSecondNo.IsChecked)
        {
            viewModel.SecondQuestionAnswer = false;
            RadioBtnSecondYes.IsChecked = false;
        }
        else
        {
            viewModel.SecondQuestionAnswer = true;
        }
    }

    private void LabelSecondNo_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnSecondNo.IsChecked)
        {
            RadioBtnSecondNo.IsChecked = false;
        }
        else
        {
            RadioBtnSecondYes.IsChecked = true;
        }
    }

    private void RadioBtnThirdYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnThirdYes.IsChecked)
        {
            viewModel.ThirdQuestionAnswer = true;
            RadioBtnThirdNo.IsChecked = false;
        }
        else
        {
            viewModel.ThirdQuestionAnswer = false;
        }
    }

    private void LabelThirdYes_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnThirdYes.IsChecked)
        {
            RadioBtnThirdYes.IsChecked = false;
        }
        else
        {
            RadioBtnThirdYes.IsChecked = true;
        }
    }

    private void RadioBtnThirdNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RadioBtnThirdNo.IsChecked)
        {
            viewModel.ThirdQuestionAnswer = false;
            RadioBtnThirdYes.IsChecked = false;
        }
        else
        {
            viewModel.ThirdQuestionAnswer = true;
        }
    }

    private void LabelThirdNo_Tapped(object sender, TappedEventArgs e)
    {
        if (RadioBtnThirdNo.IsChecked)
        {
            RadioBtnThirdNo.IsChecked = false;
        }
        else
        {
            RadioBtnThirdNo.IsChecked = true;
        }
    }
}