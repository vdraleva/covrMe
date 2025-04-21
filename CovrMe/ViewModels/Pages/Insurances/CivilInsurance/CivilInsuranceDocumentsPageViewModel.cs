using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Speedy;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceDocumentsPageViewModel : BaseViewModel
    {
        #region Fields
        
        private string _endDate;
        private string _startDate;
        private string _companyLogo;
        private string _companyName;
        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        private decimal _totalPrice;
        private int _installment = 1;
        
        //insurance
        private InsuranceOfferModel _selectedOffer;

        #endregion

        #region Props
        public InsuranceOfferModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }
        public string StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        public string EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        public string CompanyLogo
        {
            get { return _companyLogo; }
            set { SetProperty(ref _companyLogo, value); }
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
                var parameters = new Dictionary<string, object>
                    {
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                        {"selectedOffer", this._selectedOffer}
                    };
                await Navigation.PushAsync<SpeedyChooseDeliveryPage>(parameters);
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

        [RelayCommand]
        private async void GoToTermsInsurance(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.ObshtiUsloviaURL;
                await OpenBrowser(uri);
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

        [RelayCommand]
        private async void GoToInsuranceInfo(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GetInsuranceInfoUrl();
                await OpenBrowser(uri);
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

        [RelayCommand]
        private async void GoToPrivacyPolicy(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.PrivacyPolicyURL;
                await OpenBrowser(uri);
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
        [RelayCommand]
        private async void GoToContract(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.ContractURL;
                await OpenBrowser(uri);
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
        [RelayCommand]
        private async void GoToBankCardTerms(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.BankCardURL;
                await OpenBrowser(uri);
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
        private async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("#50E3C6"),
                    PreferredControlColor = Color.FromHex("#C0C0C0"),
                });
            }
            catch (Exception ex)
            {
                ;
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;

            this.CompanyLogo = this._selectedOffer.CompanyLogo;
            this._companyName = this._selectedOffer.CompanyName;
            
        }

        private string GetInsuranceInfoUrl()
        {
            switch (this._companyName)
            {
                case GlobalConstants.Dzi: return GlobalConstants.DziCivilInfo;
                case GlobalConstants.Euroins: return GlobalConstants.EuroinsCivilInfo;
                case GlobalConstants.Generali: return GlobalConstants.GeneraliCivilInfo;
                case GlobalConstants.Groupama: return GlobalConstants.GroupamaCivilInfo;
                case GlobalConstants.Ozk: return GlobalConstants.OzkCivilInfo;
                case GlobalConstants.Bulins: return GlobalConstants.BulinsCivilInfo;
                case GlobalConstants.Bulstrad: return GlobalConstants.BulstradCivilInfo;
                case GlobalConstants.Levins: return GlobalConstants.LevinsCivilInfo;
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
