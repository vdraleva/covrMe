using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;

namespace CovrMe.ViewModels.Pages.Insurances.Casco
{
    public partial class CascoInsuranceThankYouViewModel : BaseViewModel
    {
        public CascoInsuranceThankYouViewModel()
        {

        }

        #region Commands
        [RelayCommand]
        private async void Continue()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                await Navigation.PushAsync<HomePage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }
        #endregion
    }
}
