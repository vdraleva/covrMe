using CovrMe.ViewModels.Pages.Profile;

namespace CovrMe.Views.Pages.Profile;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel vm)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        BindingContext = vm;
    }

    private void ClickGestureRecognizer_Clicked(object sender, EventArgs e)
    {

    }
}