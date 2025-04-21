using CommunityToolkit.Mvvm.Input;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using CovrMe.Views.Pages.Profile;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CovrMe.ViewModels.ContentViews
{
    public  partial class NavBarViewModel : BaseViewModel
    {

        public NavBarViewModel()
        {
        }

        [RelayCommand]
        private async void GoToHomePage()
        {
            try
            {
                if (this.Navigation.NavigationStack.LastOrDefault() is HomePage)
                {
                    return;
                }

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is HomePage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is HomePage)));
                }
                await Navigation.PushAsync<HomePage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async void GoToProfilePage()
        {
            try
            {
                if (this.Navigation.NavigationStack.LastOrDefault() is ProfilePage)
                {
                    return;
                }

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is ProfilePage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is ProfilePage)));
                }
                await Navigation.PushAsync<ProfilePage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async void GoToMyInsurancesPage()
        {
            try
            {
                if (this.Navigation.NavigationStack.LastOrDefault() is MyInsurancesPage)
                {
                    return;
                }

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is MyInsurancesPage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is MyInsurancesPage)));
                }
                await Navigation.PushAsync<MyInsurancesPage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async void CreateNewInsurance()
        {
            try
            {
                if (this.Navigation.NavigationStack.LastOrDefault() is NewInsurancePage)
                {
                    return;
                }

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is NewInsurancePage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is NewInsurancePage)));
                }
                await Navigation.PushAsync<NewInsurancePage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
        }
    }

   
}
