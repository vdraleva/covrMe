using CommunityToolkit.Maui.Views;
using CovrMe.Models.Users.Result;
using CovrMe.ViewModels.Popups;

namespace CovrMe.Views.Popups;

public partial class EmailInputPopUp : Popup
{
    private readonly EmailInputPopUpViewModel viewModel;
    public EmailInputPopUp(EmailInputPopUpViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var currentButton = sender as RadioButton;
        UserModel result = await this.viewModel.UpdateEmail();
        if (result != null) 
        {
            await CloseAsync(result);
        }
    }
}