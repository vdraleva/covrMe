using CovrMe.ViewModels.ContentViews;

namespace CovrMe.Views.ContentViews;

public partial class NavBar : ContentView
{
    private NavBarViewModel vm;
    public NavBar()
    {
        InitializeComponent();
        vm = new NavBarViewModel();
        vm.Navigation = Navigation;
        this.BindingContext = vm;
    }
}