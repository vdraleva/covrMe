using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;

namespace CovrMe.ViewModels.Pages.Insurances.TravelInsurance
{
    public partial class TravelInsuranceLocationViewModel : BaseViewModel
    {
        #region Fields

        private byte _territory;
        private byte _tripPurpose;
        #endregion

        public TravelInsuranceLocationViewModel()
        {
            
        }

        #region Props

        public byte Territory
        {
            get { return this._territory; }
            set { SetProperty(ref this._territory, value); }
        }

        public byte TripPurpose
        {
            get { return this._tripPurpose; }
            set { SetProperty(ref this._tripPurpose, value); }
        }
        #endregion
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

                var travelInfo = new TravelInsuranceOfferModel
                {
                    TripPurpose = this.TripPurpose,
                    Territory = this.Territory
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"travelInfo", travelInfo},
                    };

                await Navigation.PushAsync<TravelInsuranceInsuredUsersPage>(parameters);
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

        #region Methods
        #endregion
    }
}
