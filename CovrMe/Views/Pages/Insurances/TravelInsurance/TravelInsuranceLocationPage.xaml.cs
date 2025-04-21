using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;

namespace CovrMe.Views.Pages.Insurances.TravelInsurance;

public partial class TravelInsuranceLocationPage : ContentPage
{
    public TravelInsuranceLocationViewModel viewmodel;
    public TravelInsuranceLocationPage(TravelInsuranceLocationViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        viewmodel = vm;
        BindingContext = vm;

        OnStart();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        TripRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }
    private void BussinessEvent_Tapped(object sender, TappedEventArgs e)
    {
        BussinessEventRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }

    private void StudyPurposes_Tapped(object sender, TappedEventArgs e)
    {
        StudyPurposesRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }

    private void Fair_Tapped(object sender, TappedEventArgs e)
    {
        FairRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }

    private void Work_Tapped(object sender, TappedEventArgs e)
    {
        WorkRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }

    private void StudentBrigade_Tapped(object sender, TappedEventArgs e)
    {
        StudentBrigadeRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }

    private void MilitaryPersonnel_Tapped(object sender, TappedEventArgs e)
    {
        MilitaryPersonnelRadio.IsChecked = true;
        ChangeSenderTextColor(sender);
    }
    private void RadioBtnFirst_Tapped(object sender, TappedEventArgs e)
    {
        RadioBtnFirst.IsChecked = true;
        RadioBtnSecond.IsChecked = false;
        ChangeMainSenderTextColor(sender);
    }
    private void RadioBtnSecond_Tapped(object sender, TappedEventArgs e)
    {
        RadioBtnSecond.IsChecked = true;
        RadioBtnFirst.IsChecked = false;
        ChangeMainSenderTextColor(sender);
    }
    private void ChangeSenderTextColor(object sender)
    {
        CleanTextColor();

        var tappedLabel = (Label)sender;
        tappedLabel.TextColor = Color.FromHex("#823189");
    }
    private void ChangeMainSenderTextColor(object sender)
    {
        CleanMainTextColor();

        var tappedLabel = (Label)sender;
        tappedLabel.TextColor = Color.FromHex("#823189");
    }
    private void OnStart()
    {
        RadioBtnFirst.IsChecked = true;
        RadioBtnFirstText.TextColor = Color.FromHex("#823189");
        viewmodel.Territory = (byte)10;

        RadioBtnSecond.IsChecked = false;

        TripRadio.IsChecked = true;
        TripText.TextColor = Color.FromHex("#823189");
        viewmodel.TripPurpose = (byte)1;

        BussinessEventRadio.IsChecked = false;
        StudyPurposesRadio.IsChecked = false;
        FairRadio.IsChecked = false;
        WorkRadio.IsChecked = false;
        StudentBrigadeRadio.IsChecked = false;
        MilitaryPersonnelRadio.IsChecked = false;
    }
    private void CleanMainTextColor()
    {
        RadioBtnFirstText.TextColor = Color.FromHex("#000000");
        RadioBtnSecondText.TextColor = Color.FromHex("#000000");
    }
    private void CleanTextColor()
    {
        TripText.TextColor = Color.FromHex("#000000");
        BussinessEventText.TextColor = Color.FromHex("#000000");
        StudyPurposesText.TextColor = Color.FromHex("#000000");
        FairText.TextColor = Color.FromHex("#000000");
        WorkText.TextColor = Color.FromHex("#000000");
        StudentBrigadeText.TextColor = Color.FromHex("#000000");
        MilitaryPersonnelText.TextColor = Color.FromHex("#000000");
    }
    private void TripRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        TripText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void BussinessEventRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        BussinessEventText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void StudyPurposesRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        StudyPurposesText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void FairRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        FairText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void WorkRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        WorkText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void StudentBrigadeRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        StudentBrigadeText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void MilitaryPersonnelRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CleanTextColor();
        MilitaryPersonnelText.TextColor = Color.FromHex("#823189");

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.TripPurpose = value;
    }

    private void RadioBtnFirst_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            CleanMainTextColor();
            RadioBtnFirstText.TextColor = Color.FromHex("#823189");
            RadioBtnSecond.IsChecked = false;
        }

        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.Territory = value;
    }

    private void RadioBtnSecond_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            CleanMainTextColor();
            RadioBtnSecondText.TextColor = Color.FromHex("#823189");
            RadioBtnFirst.IsChecked = false;
        }
        var currentButton = sender as RadioButton;
        var value = byte.Parse(currentButton.Value.ToString());
        viewmodel.Territory = value;
    }

}