using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Deliveries;
using CovrMe.Models.Insurances;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Popups;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using CovrMe.Views.Pages.Payment;
using Maui.Plugins.PageResolver;
using System.Text.RegularExpressions;

namespace CovrMe.ViewModels.Pages.Speedy
{
    public partial class SpeedyDeliveryOfficeViewModel : BaseViewModel
    {
        #region Fields
        private string _fullName;
        private string _phone;
        private string _city;
        private string _neighbourhood;
        private string phoneNumberCode;
        private string _endDate;
        private string _startDate;
        private decimal _totalPrice;
        private int _installment = 1;
        private int _selectedOfficeId = 0;
        private string _selectedOfficeName = string.Empty;
        private bool _selectedOffice = false;
        private string _installmentAmount;
        private string _companyName;

        private bool _isError = false;
        private bool _fullNameError;
        private bool _phoneError;
        private bool _cityError;
        private bool _neighbourhoodError;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        private readonly IDeliveryService _deliveryService;
        private readonly IPopupService _popupService;

        //insurance
        private InsuranceOfferModel _selectedOffer;
        #endregion

        public SpeedyDeliveryOfficeViewModel(IDeliveryService deliveryService, IPopupService popupService)
        {
            this._deliveryService = deliveryService;
            this._popupService = popupService;
        }

        #region Props

        public bool SelectedOffice
        {
            get { return _selectedOffice; }
            set { SetProperty(ref _selectedOffice, value); }
        }

        public string SelectedOfficeName
        {
            get { return _selectedOfficeName; }
            set { SetProperty(ref _selectedOfficeName, value); }
        }
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }
        public string Neighbourhood
        {
            get { return _neighbourhood; }
            set { SetProperty(ref _neighbourhood, value); }
        }
        public string PhoneNumberCode
        {
            get { return this.phoneNumberCode; }
            set { SetProperty(ref this.phoneNumberCode, value); }
        }
        public bool IsError
        {
            get { return _isError; }
            set
            {
                if (_isError != value)
                {
                    _isError = value;
                    OnPropertyChanged(nameof(IsError));
                }
            }
        }
        public bool FullNameError
        {
            get { return _fullNameError; }
            set
            {
                if (_fullNameError != value)
                {
                    _fullNameError = value;
                    OnPropertyChanged(nameof(FullNameError));
                }
            }
        }
        public bool PhoneError
        {
            get { return _phoneError; }
            set
            {
                if (_phoneError != value)
                {
                    _phoneError = value;
                    OnPropertyChanged(nameof(PhoneError));
                }
            }
        }
        public bool CityError
        {
            get { return _cityError; }
            set
            {
                if (_cityError != value)
                {
                    _cityError = value;
                    OnPropertyChanged(nameof(CityError));
                }
            }
        }
        public bool NeighbourhoodError
        {
            get { return _neighbourhoodError; }
            set
            {
                if (_neighbourhoodError != value)
                {
                    _neighbourhoodError = value;
                    OnPropertyChanged(nameof(NeighbourhoodError));
                }
            }
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

                var valResult = this.ValidateInput();

                if (!valResult.IsValid)
                {
                    throw new Exception(valResult.Message);
                }

                if (this._selectedOfficeId == 0)
                {
                    throw new Exception(MessageConstants.RequiredOfficeId);
                }

                if (string.IsNullOrEmpty(this.PhoneNumberCode))
                {
                    this.PhoneNumberCode = "+359";
                }
                var phone = this.PhoneNumberCode + this.Phone.TrimEnd();

                decimal deliveryPrice = await this.DeliveryCalculation();

                //decimal deliveryPrice = 300.50m;

                var deliveryInfo = new InsuranceDeliveryModel
                {
                    Name = this.FullName.TrimEnd(),
                    Phone = phone,
                    City = this._city.TrimEnd(),
                    Neighbourhood = string.IsNullOrEmpty(this._neighbourhood) ? null : this._neighbourhood.TrimEnd(),
                    DeliveryPrice = deliveryPrice,
                    Email = App.Email,
                    OfficeId = this._selectedOfficeId,
                };
                
                var parameters = new Dictionary<string, object>
                    {
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                        {"selectedOffer", this._selectedOffer},
                        {"deliveryInfo", deliveryInfo}
                    };
                await Navigation.PushAsync<PrePaymentPage>(parameters);
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
        private async void Search()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var valResult = this.ValidateInput();

                if (!valResult.IsValid)
                {
                    throw new Exception(valResult.Message);
                }

                if (string.IsNullOrEmpty(this.PhoneNumberCode))
                {
                    this.PhoneNumberCode = "+359";
                }
                var phone = !string.IsNullOrEmpty(this.Phone) ? this.PhoneNumberCode + this.Phone : string.Empty;

                var httpClient = HttpClientFactory.Create();
                var jwt = App.JwtToken;

                var offices = await this._deliveryService.FindOffice(this.City, this.Neighbourhood, jwt, httpClient);

                if(offices.Count == 0)
                {
                    this._selectedOfficeId = 0;
                    this.SelectedOfficeName = string.Empty;
                    this.SelectedOffice = false;

                    await App.DisplayAlert(App.MESSAGE_HEADER_ATT, "Няма намерени офиси", App.MESSAGE_OK);
                }
                else
                {
                    object? popupResult = null;

                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        popupResult = await this._popupService.ShowPopupAsync<SpeedyOfficesPopUpViewModel>(onPresenting: async viewModel => await viewModel.PopulateCollection(offices), CancellationToken.None);
                    });

                    //popupResult = await this._popupService.ShowPopupAsync<SpeedyOfficesPopUpViewModel>(onPresenting: async viewModel => await viewModel.PopulateCollection(offices), CancellationToken.None);

                    int selectedOfficeId = 0;

                    if (popupResult != null)
                    {
                        selectedOfficeId = (int)popupResult;
                    }
                    else
                    {
                        throw new Exception(MessageConstants.GeneralError);
                    }

                    this._selectedOfficeId = selectedOfficeId;
                    var selectedOfffice = offices.FirstOrDefault(x => x.Id == selectedOfficeId);

                    if (selectedOfffice != null)
                    {
                        this.SelectedOfficeName = selectedOfffice.Name;
                        this.SelectedOffice = true;
                    }
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
            await PopulateUserInfo();
        }

        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.City))
            {
                this.CityError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredCity;
            }
            else
            {
                this.CityError = false;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                this.PhoneError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredPhone;
            }
            else
            {
                this.PhoneError = false;
            }

            if (!this.PhoneValidation())
            {
                this.PhoneError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.InvalidPhone;
            }

            if (string.IsNullOrEmpty(this.FullName))
            {
                this.FullNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredFirstName;
            }
            else
            {
                this.FullNameError = false;
            }

            return res;
        }
        private bool PhoneValidation()
        {
            if (!string.IsNullOrEmpty(this.Phone))
            {
                if (Regex.IsMatch(this.Phone.TrimEnd(), GlobalConstants.PhoneRegex))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<decimal> DeliveryCalculation()
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            decimal deliveryPrice = 0;

            var deliveryCalcResult = await this._deliveryService.Calculation(string.Empty, this._selectedOfficeId, jwt, httpClient);

            if (deliveryCalcResult != null)
            {
                deliveryPrice = deliveryCalcResult.Total;
            }

            return deliveryPrice;
        }

        private async Task PopulateUserInfo()
        {
            this.FullName = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName;

            this.City = this._ownerUserInfo.CityName;

            var currentPhone = App.Phone;

            if (!string.IsNullOrEmpty(currentPhone))
            {
                if (currentPhone.StartsWith("+359"))
                {
                    currentPhone = currentPhone.Substring(4);

                    this.Phone = currentPhone;
                }
            }
        }

        #endregion
    }
}
