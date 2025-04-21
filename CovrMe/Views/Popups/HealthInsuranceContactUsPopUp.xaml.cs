using CommunityToolkit.Maui.Views;
using CovrMe.ViewModels.Popups;

namespace CovrMe.Views.Popups;

public partial class HealthInsuranceContactUsPopUp : Popup
{
    private readonly HealthInsuranceContactUsPopUpViewModel viewModel;
    public HealthInsuranceContactUsPopUp(HealthInsuranceContactUsPopUpViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await CloseAsync(cts.Token);
    }
}