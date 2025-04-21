using CommunityToolkit.Maui.Views;
using CovrMe.ViewModels.Popups;

namespace CovrMe.Views.Popups;

public partial class MountainXtreemInfoPopUp  : Popup
{
	private readonly MountainXtreemInfoPopUpViewModel viewModel;
    public MountainXtreemInfoPopUp(MountainXtreemInfoPopUpViewModel vm)
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