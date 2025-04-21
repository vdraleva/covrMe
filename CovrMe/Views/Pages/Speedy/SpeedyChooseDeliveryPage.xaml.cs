using CovrMe.ViewModels.Pages.Speedy;

namespace CovrMe.Views.Pages.Speedy;

public partial class SpeedyChooseDeliveryPage : ContentPage
{
    SpeedyChooseDeliveryViewModel viewmodel;
    public SpeedyChooseDeliveryPage(SpeedyChooseDeliveryViewModel vm, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
        viewmodel = vm;
        vm.ApplyQueryAttributes(parameters);

        CurierRadio.TextColor = Color.FromHex("#823189");
        OfficeRadio.TextColor = Color.FromHex("#000000");
        CurierText.TextColor = Color.FromHex("#823189");
        OfficeText.TextColor = Color.FromHex("#000000");
    }

    private void OfficeRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (OfficeRadio.IsChecked)
        {
            viewmodel.IsOffice = true;
            viewmodel.IsCurier = false;

            OfficeRadio.TextColor = Color.FromHex("#823189");
            CurierRadio.IsChecked = false;
            CurierRadio.TextColor = Color.FromHex("#000000");

            CurierText.TextColor = Color.FromHex("#000000");
            OfficeText.TextColor = Color.FromHex("#823189");
        }
    }

    private void CurierRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CurierRadio.IsChecked)
        {
            viewmodel.IsOffice = false;
            viewmodel.IsCurier = true;

            CurierRadio.TextColor = Color.FromHex("#823189");
            OfficeRadio.IsChecked = false;
            OfficeRadio.TextColor = Color.FromHex("#000000");

            CurierText.TextColor = Color.FromHex("#823189");
            OfficeText.TextColor = Color.FromHex("#000000");
        }
    }


    private void OfficeLabel_Tapped(object sender, TappedEventArgs e)
    {
        if (!OfficeRadio.IsChecked)
        {
            viewmodel.IsOffice = true;
            viewmodel.IsCurier = false;

            OfficeRadio.TextColor = Color.FromHex("#823189");
            CurierRadio.IsChecked = false;
            CurierRadio.TextColor = Color.FromHex("#000000");

            CurierText.TextColor = Color.FromHex("#000000");
            OfficeText.TextColor = Color.FromHex("#823189");

            OfficeRadio.IsChecked = true;
            CurierRadio.IsChecked = false;
        }
        
    }

    private void AddressLabel_Tapped(object sender, TappedEventArgs e)
    {
        if (!CurierRadio.IsChecked)
        {
            viewmodel.IsOffice = false;
            viewmodel.IsCurier = true;

            CurierRadio.TextColor = Color.FromHex("#823189");
            OfficeRadio.IsChecked = false;
            OfficeRadio.TextColor = Color.FromHex("#000000");

            CurierText.TextColor = Color.FromHex("#823189");
            OfficeText.TextColor = Color.FromHex("#000000");

            OfficeRadio.IsChecked = false;
            CurierRadio.IsChecked = true;
        }
    }
}