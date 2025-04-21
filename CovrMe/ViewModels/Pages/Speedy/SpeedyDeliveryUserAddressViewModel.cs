using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Deliveries;
using CovrMe.Models.Deliveries.Result;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using CovrMe.Views.Pages.Payment;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CovrMe.ViewModels.Pages.Speedy
{
    public partial class SpeedyDeliveryUserAddressViewModel : BaseViewModel
    {
        #region Fields

        private LocationDataModel _selectedRegion;
        private LocationDataModel _selectedMunicipality;
        private CityDataModel _selectedCityModel;
        private string _companyName;

        private string _fullName;
        private string phone;
        private string phoneNumberCode;
        private string _postCode;
        private string _street;
        private string _blok;
        private string _entrance;
        private string _apartment;
        private string _floor;
        private string _description;

        private bool _isError = false;
        private bool _fullNameError;
        private bool _phoneError;
        private bool _streetError;
        private bool _postCodeError;
        private bool _entranceError;
        private bool _blokError;
        private bool _apartmentError;
        private bool _floorError;

        //services
        private ILocationService _locationService;
        private IUserService _userService;
        private IDeliveryService _deliveryService;

        private string _endDate;
        private string _startDate;

        private decimal _totalPrice;
        private int _installment = 1;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        //insurance
        private InsuranceOfferModel _selectedOffer;

        //collections

        private ObservableCollection<LocationDataModel> _regionCollection;
        private ObservableCollection<LocationDataModel> _municipalityCollection;
        private ObservableCollection<CityDataModel> _cityCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;

        #endregion
        public SpeedyDeliveryUserAddressViewModel(ILocationService locationService, IUserService userService, IDeliveryService deliveryService)
        {
            this._locationService = locationService;
            this._userService = userService;
            _deliveryService = deliveryService;

            this.RegionCollection = new ObservableCollection<LocationDataModel>();
            this.MunicipalityCollection = new ObservableCollection<LocationDataModel>();
            this.CityCollection = new ObservableCollection<CityDataModel>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();


            Task.Run(async () => { await GetCountries(); }).Wait();
            Task.Run(async () => { await GetRegions(); }).Wait();
            
        }

        #region Collections

        public ObservableCollection<LocationDataModel> RegionCollection
        {
            get => _regionCollection;
            set => SetProperty(ref _regionCollection, value);
        }
        public ObservableCollection<CityDataModel> CityCollection
        {
            get => _cityCollection;
            set => SetProperty(ref _cityCollection, value);
        }
        public ObservableCollection<LocationDataModel> CountryCollection
        {
            get => _countryCollection;
            set => SetProperty(ref _countryCollection, value);
        }
        public ObservableCollection<LocationDataModel> MunicipalityCollection
        {
            get => _municipalityCollection;
            set => SetProperty(ref _municipalityCollection, value);
        }
        #endregion

        #region Props

        public LocationDataModel SelectedRegion
        {
            get { return _selectedRegion; }
            set { SetProperty(ref _selectedRegion, value); }
        }
        public LocationDataModel SelectedMunicipality
        {
            get { return _selectedMunicipality; }
            set { SetProperty(ref _selectedMunicipality, value); }
        }
        public CityDataModel SelectedCityModel
        {
            get { return _selectedCityModel; }
            set { SetProperty(ref _selectedCityModel, value); }
        }

        public string Description
        {
            get { return this._description; }
            set { SetProperty(ref this._description, value); }
        }

        public string FullName
        {
            get { return this._fullName; }
            set { SetProperty(ref this._fullName, value); }
        }
        public string Phone
        {
            get { return this.phone; }
            set { SetProperty(ref this.phone, value); }
        }
        public string PhoneNumberCode
        {
            get { return this.phoneNumberCode; }
            set { SetProperty(ref this.phoneNumberCode, value); }
        }
        public string PostCode
        {
            get { return this._postCode; }
            set { SetProperty(ref this._postCode, value); }
        }
        public string Street
        {
            get { return this._street; }
            set { SetProperty(ref this._street, value); }
        }
        public string Blok
        {
            get { return this._blok; }
            set { SetProperty(ref this._blok, value); }
        }
        public string Entrance
        {
            get { return this._entrance; }
            set { SetProperty(ref this._entrance, value); }
        }
        public string Apartment
        {
            get { return this._apartment; }
            set { SetProperty(ref this._apartment, value); }
        }
        public string Floor
        {
            get { return this._floor; }
            set { SetProperty(ref this._floor, value); }
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
        public bool StreetError
        {
            get { return _streetError; }
            set
            {
                if (_streetError != value)
                {
                    _streetError = value;
                    OnPropertyChanged(nameof(StreetError));
                }
            }
        }
        public bool PostCodeError
        {
            get { return _postCodeError; }
            set
            {
                if (_postCodeError != value)
                {
                    _postCodeError = value;
                    OnPropertyChanged(nameof(PostCodeError));
                }
            }
        }
        public bool EntranceError
        {
            get { return _entranceError; }
            set
            {
                if (_entranceError != value)
                {
                    _entranceError = value;
                    OnPropertyChanged(nameof(EntranceError));
                }
            }
        }
        public bool BlokError
        {
            get { return _blokError; }
            set
            {
                if (_blokError != value)
                {
                    _blokError = value;
                    OnPropertyChanged(nameof(BlokError));
                }
            }
        }
        public bool ApartmentError
        {
            get { return _apartmentError; }
            set
            {
                if (_apartmentError != value)
                {
                    _apartmentError = value;
                    OnPropertyChanged(nameof(ApartmentError));
                }
            }
        }
        public bool FloorError
        {
            get { return _floorError; }
            set
            {
                if (_floorError != value)
                {
                    _floorError = value;
                    OnPropertyChanged(nameof(FloorError));
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

                if (this.SelectedRegion == null)
                {
                    throw new Exception(MessageConstants.RegionNotSelected);
                }

                if (this.SelectedMunicipality == null)
                {
                    throw new Exception(MessageConstants.MuniciplalityNotSelected);
                }

                if (this.SelectedCityModel == null)
                {
                    throw new Exception(MessageConstants.CityNotSelected);
                }

                if (string.IsNullOrEmpty(this.PhoneNumberCode))
                {
                    this.PhoneNumberCode = "+359";
                }
                var phone = this.PhoneNumberCode + this.Phone.TrimEnd();

                decimal deliveryPrice = await this.DeliveryCalculation();

                var deliveryInfo = new InsuranceDeliveryModel()
                {
                    OfficeId = 0,
                    Name = this.FullName.TrimEnd(),
                    Phone = phone,
                    Email = App.Email,
                    PostalCode = string.IsNullOrEmpty(this.PostCode) ? null :  this.PostCode.TrimEnd(),
                    Street = string.IsNullOrEmpty(this.Street) ? null : this.Street.TrimEnd(),
                    BlockNo = string.IsNullOrEmpty(this.Blok) ? null : this.Blok.TrimEnd(),
                    Entrance = string.IsNullOrEmpty(this.Entrance) ? null : this.Entrance.TrimEnd(),
                    Floor = string.IsNullOrEmpty(this.Floor) ? null : this.Floor.TrimEnd(),
                    Appartment = string.IsNullOrEmpty(this.Apartment) ? null : this.Apartment.TrimEnd(),
                    Info = string.IsNullOrEmpty(this.Description) ? null : this.Description.TrimEnd(),
                    DeliveryPrice = deliveryPrice
                };

                
                var parameters = new Dictionary<string, object>
                    {
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                        {"selectedOffer", this._selectedOffer},
                        {"deliveryInfo", deliveryInfo},
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
        #endregion

        #region Private methods
        private async Task GetCountries()
        {
            var countriesCollection = new ObservableCollection<LocationDataModel>();
            var countriesColl = new List<LocationDataModel>();

            var countriesJson = await AssetsHelper.LoadAssets(GlobalConstants.Countries);
            var countries = JsonConvert.DeserializeObject<GetCountriesResultModel>(countriesJson);
            countriesColl = countries.Countries;

            countriesColl = countriesColl.OrderBy(c => c.Id != "BG")
                                 .ThenBy(c => c.Name)
                                 .ToList();

            this.CountryCollection.Clear();

            foreach (var countiry in countriesColl)
            {
                countriesCollection.Add(countiry);
            }

            this.CountryCollection = countriesCollection;
        }
        private async Task GetRegions()
        {
            var regionsCollection = new ObservableCollection<LocationDataModel>();
            var regionsColl = new List<LocationDataModel>();

            var regionsJson = await AssetsHelper.LoadAssets(GlobalConstants.Regions);
            var regions = JsonConvert.DeserializeObject<GetRegionsResultModel>(regionsJson);
            regionsColl = regions.Regions;

            regionsColl = regionsColl.OrderBy(r => r.Id != "SFO" && r.Id != "SOF")
                            .ThenBy(r => r.Name)
                            .ToList();

            this.RegionCollection.Clear();

            foreach (var region in regionsColl)
            {
                regionsCollection.Add(region);
            }

            this.RegionCollection = regionsCollection;
        }
        public async Task GetMunicipality(LocationDataModel region)
        {
            var municipalityCollection = new ObservableCollection<LocationDataModel>();
            var munColl = new List<LocationDataModel>();

            string munUrl = $"{GlobalConstants.Municipalities}{region.Id}.json";

            var municipalitiesJson = await AssetsHelper.LoadAssets(munUrl);
            var municipalities = JsonConvert.DeserializeObject<GetMunicipalityResultModel>(municipalitiesJson);
            munColl = municipalities.Municipalities;

            this.MunicipalityCollection.Clear();

            foreach (var mun in munColl)
            {
                municipalityCollection.Add(mun);
            }

            this.MunicipalityCollection = municipalityCollection;
        }
        public async Task GetCities(LocationDataModel municipality)
        {
            var cityCollection = new ObservableCollection<CityDataModel>();
            var cityColl = new List<CityDataModel>();

            string cityUrl = $"{GlobalConstants.Cities}{municipality.Id}.json";

            var citiesJson = await AssetsHelper.LoadAssets(cityUrl);
            var cities = JsonConvert.DeserializeObject<GetCityResultModel>(citiesJson);
            cityColl = cities.Cities;

            cityColl = cityColl.OrderBy(r => r.PostCode != "1000")
                            .ThenBy(r => r.Name)
                            .ToList();

            this.CityCollection.Clear();

            foreach (var city in cityColl)
            {
                cityCollection.Add(city);
            }

            this.CityCollection = cityCollection;
        }
        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.Street))
            {
                this.StreetError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredStreet;
            }

            if (string.IsNullOrEmpty(this.PostCode))
            {
                this.PostCodeError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredPostCode;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                this.PhoneError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredPhone;
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

            if (string.IsNullOrEmpty(this.Description) && (string.IsNullOrEmpty(this.Blok) || string.IsNullOrEmpty(this.Entrance)
                || string.IsNullOrEmpty(this.Floor) || string.IsNullOrEmpty(this.Apartment)))
            {
                this.BlokError = true;
                this.EntranceError = true;
                this.FloorError = true;
                this.ApartmentError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredInfo;
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
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
               
            await PopulateUserInfo();
        }
        private async Task PopulateUserInfo()
        {
            this.FullName = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName;
            this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == this._ownerUserInfo.RegionCode);

            if (this.SelectedRegion != null)
            {
                await this.GetMunicipality(this.SelectedRegion);
            }
                
            this.SelectedMunicipality = this.MunicipalityCollection.FirstOrDefault(x => x.Id == this._ownerUserInfo.MunicipalityCode);

            if (this.SelectedMunicipality != null)
            {
                await this.GetCities(this.SelectedMunicipality);
            }              

            this.SelectedCityModel = this.CityCollection.FirstOrDefault(x => x.Id == this._ownerUserInfo.CityCode);
            this.PostCode = this._ownerUserInfo.PostalCode;

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
        private async Task<decimal> DeliveryCalculation()
        {
            var officeId = 0;
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            decimal deliveryPrice = 0;

            var deliveryCalcResult = await this._deliveryService.Calculation(this.PostCode, officeId, jwt, httpClient);

            if(deliveryCalcResult != null)
            {
                deliveryPrice = deliveryCalcResult.Total;
            }

            return deliveryPrice;
        }
        #endregion
    }
}
