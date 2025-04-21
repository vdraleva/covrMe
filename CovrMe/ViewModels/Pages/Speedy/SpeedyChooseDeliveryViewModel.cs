using CovrMe.Views.Pages.Speedy;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances.CivilInsurance;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Insurances;

namespace CovrMe.ViewModels.Pages.Speedy
{
    public partial class SpeedyChooseDeliveryViewModel : BaseViewModel
    {
        #region Fields
        private string _endDate;
        private string _startDate;
        private string _companyName;

        private decimal _totalPrice;
        private int _installment = 1;
        private string _installmentAmount;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        //insurance
        private InsuranceOfferModel _selectedOffer;
        private bool _isCurier;
        private bool _isOffice;

        #endregion

        #region Props
        public InsuranceOfferModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }
        public bool IsCurier
        {
            get { return _isCurier; }
            set { SetProperty(ref _isCurier, value); }
        }
        public bool IsOffice
        {
            get { return _isOffice; }
            set { SetProperty(ref _isOffice, value); }
        }
        #endregion

        public SpeedyChooseDeliveryViewModel(){}
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

                var parameters = new Dictionary<string, object>
                    {
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                        {"selectedOffer", this._selectedOffer}
                    };
                if (IsOffice)
                {
                    await Navigation.PushAsync<SpeedyDeliveryOfficePage>(parameters);
                }
                else
                {
                    await Navigation.PushAsync<SpeedyDeliveryUserAddressPage>(parameters);
                }
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

        #region Private methods
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
            
        }
        #endregion
    }
}
