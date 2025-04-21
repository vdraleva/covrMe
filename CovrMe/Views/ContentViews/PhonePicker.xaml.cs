using CountryData.Standard;
using CovrMe.Models;
using CovrMe.Shared;
using CovrMe.ViewModels.ContentViews;
using System.Collections.ObjectModel;

namespace CovrMe.Views.ContentViews;

public partial class PhonePicker : ContentView
{
    public static readonly BindableProperty PhoneProperty =
    BindableProperty.Create(nameof(Phone), typeof(string), typeof(PhonePicker),
         defaultBindingMode: BindingMode.TwoWay);
    public string Phone
    {
        get => (string)GetValue(PhoneProperty);
        set => SetValue(PhoneProperty, value);
    }

    public ObservableCollection<Country> Countries { get; private set; }
    public PhonePicker()
    {
        InitializeComponent();
        PickerEntry.Text = "+359";
        PickerImage.Text = "🇧🇬";
        this.Countries = new ObservableCollection<Country>(new CountryHelper().GetCountryData());

        SuggestionsCollectionView.ItemsSource = Countries;
    }

    private void OnPickerEntryTapped(object sender, EventArgs e)
    {
        PickerEntry.Focus();
        SuggestionsCollectionView.IsVisible = !SuggestionsCollectionView.IsVisible;
    }

    private void OnSuggestionsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCountry = e.CurrentSelection.FirstOrDefault() as Country;


        var countryTelCodeHelper = new CountryTelCodeHelper();

        // Assuming "selectedCountryShortCode" is the short code of the selected country
        string selectedPhoneCode = countryTelCodeHelper.GetPhoneCodeByCountryShortCode(selectedCountry.CountryShortCode);
        PickerImage.IsVisible = true;
        PickerImage.Text = selectedCountry.CountryFlag;
        PickerEntry.Text = selectedPhoneCode;
        SuggestionsCollectionView.IsVisible = false;
    }
}