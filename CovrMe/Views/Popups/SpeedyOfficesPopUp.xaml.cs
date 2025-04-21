using CommunityToolkit.Maui.Views;
using CovrMe.ViewModels.Popups;

namespace CovrMe.Views.Popups;

public partial class SpeedyOfficesPopUp : Popup
{
    private readonly SpeedyOfficesPopUpViewModel viewModel;
    public SpeedyOfficesPopUp(SpeedyOfficesPopUpViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    private async void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var currentButton = sender as RadioButton;
        var value = int.Parse(currentButton.Value.ToString());

        await this.viewModel.SelectOffice(value);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        int selectedOfficeId = 0;

        var selectedOffice = viewModel.OfficeCollection.FirstOrDefault(x => x.IsChecked == true);

        if(selectedOffice != null)
        {
            selectedOfficeId = selectedOffice.Id;
        }

        await CloseAsync(selectedOfficeId, cts.Token);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}