using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace CovrMe.ViewModels.Pages.Profile
{
    public partial class PersonalDataViewModel : BaseViewModel
    {
        #region Fields

        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;
        private DateTime _birthDate;

        private bool _updatedRegionOnLoading = false;
        private bool _updatedMunicipalityOnLoading = false;
        private LocationDataModel _selectedRegion;
        private LocationDataModel _selectedMunicipality;
        private CityDataModel _selectedCityModel;

        private string fullName;
        private string fullNameEng;
        private string phone;
        private string address;
        private string email;
        private string oldPassword;
        private string password;
        private string confirmPassword;
        private string phoneNumberCode;
        private string latinAddress;
        private string companyName;
        private string latinCompanyName;
        private string uinNumber;
        private bool isVisibleCompanyField = false;

        private bool isError = false;
        private bool isFullPhoneVisible = false;

        private bool oldPasswordError;
        private bool passwordError;
        private bool confirmPasswordError;
        private bool emailError;
        private bool fullNameError;
        private bool fullNameEngError;
        private bool phoneError;
        private bool addressError;
        private bool latinAddressError;
        private bool companyNameError;
        private bool latinCompanyNameError;
        private bool _isPasswordChanged;

        private bool _isUpdatingCollection = false;

        //services
        private ILocationService _locationService;
        private IUserService _userService;
        private IAuthenticationService _authenticationService;

        //collections
        private ObservableCollection<LocationDataModel> _regionCollection;
        private ObservableCollection<LocationDataModel> _municipalityCollection;
        private ObservableCollection<CityDataModel> _cityCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;
        #endregion

        public PersonalDataViewModel(ILocationService locationService, IUserService userService, IAuthenticationService authenticationService)
        {
            this._locationService = locationService;
            this._userService = userService;
            this._authenticationService = authenticationService;

            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1900");
            this.BirthDate = DateTime.Parse("01/01/1990");

            this.RegionCollection = new ObservableCollection<LocationDataModel>();
            this.MunicipalityCollection = new ObservableCollection<LocationDataModel>();
            this.CityCollection = new ObservableCollection<CityDataModel>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();
           
            Task.Run(async () => { await GetCountries(); }).Wait();
            Task.Run(async () => { await GetRegions(); }).Wait();
            Task.Run(async () => { await GetUserInfo(); }).Wait();
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

        public DateTime MinPickerDate
        {
            get { return _minPickerDate; }
            set { SetProperty(ref _minPickerDate, value); }
        }

        public DateTime MaxPickerDate
        {
            get { return _maxPickerDate; }
            set { SetProperty(ref _maxPickerDate, value); }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { SetProperty(ref _birthDate, value); }
        }
        public string UinNumber
        {
            get { return this.uinNumber; }
            set { SetProperty(ref this.uinNumber, value); }
        }
        public string Email
        {
            get { return this.email; }
            set { SetProperty(ref this.email, value); }
        }
        public string OldPassword
        {
            get { return this.oldPassword; }
            set { SetProperty(ref this.oldPassword, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetProperty(ref this.password, value); }
        }
        public string ConfirmPassword
        {
            get { return this.confirmPassword; }
            set { SetProperty(ref this.confirmPassword, value); }
        }
        public string Address
        {
            get { return this.address; }
            set { SetProperty(ref this.address, value); }
        }
        public string FullName
        {
            get { return this.fullName; }
            set { SetProperty(ref this.fullName, value); }
        }
        public string FullNameEng
        {
            get { return this.fullNameEng; }
            set { SetProperty(ref this.fullNameEng, value); }
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
        public string LatinAddress
        {
            get { return this.latinAddress; }
            set { SetProperty(ref this.latinAddress, value); }
        }
        public string CompanyName
        {
            get { return this.companyName; }
            set { SetProperty(ref this.companyName, value); }
        }
        public string LatinCompanyName
        {
            get { return this.latinCompanyName; }
            set { SetProperty(ref this.latinCompanyName, value); }
        }
        public bool IsVisibleCompanyField
        {
            get { return this.isVisibleCompanyField; }
            set { SetProperty(ref this.isVisibleCompanyField, value); }
        }
        public bool IsError
        {
            get { return isError; }
            set
            {
                if (isError != value)
                {
                    isError = value;
                    OnPropertyChanged(nameof(IsError));
                }
            }
        }
        public bool EmailError
        {
            get { return emailError; }
            set
            {
                if (emailError != value)
                {
                    emailError = value;
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }
        public bool OldPasswordError
        {
            get { return oldPasswordError; }
            set
            {
                if (oldPasswordError != value)
                {
                    oldPasswordError = value;
                    OnPropertyChanged(nameof(OldPasswordError));
                }
            }
        }
        public bool PasswordError
        {
            get { return passwordError; }
            set
            {
                if (passwordError != value)
                {
                    passwordError = value;
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }
        public bool ConfirmPasswordError
        {
            get { return confirmPasswordError; }
            set
            {
                if (confirmPasswordError != value)
                {
                    confirmPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmPasswordError));
                }
            }
        }
        public bool FullNameError
        {
            get { return fullNameError; }
            set
            {
                if (fullNameError != value)
                {
                    fullNameError = value;
                    OnPropertyChanged(nameof(FullNameError));
                }
            }
        }
        public bool FullNameEngError
        {
            get { return fullNameEngError; }
            set
            {
                if (fullNameEngError != value)
                {
                    fullNameEngError = value;
                    OnPropertyChanged(nameof(FullNameEngError));
                }
            }
        }
        public bool PhoneError
        {
            get { return phoneError; }
            set
            {
                if (phoneError != value)
                {
                    phoneError = value;
                    OnPropertyChanged(nameof(PhoneError));
                }
            }
        }
        public bool AddressError
        {
            get { return addressError; }
            set
            {
                if (addressError != value)
                {
                    addressError = value;
                    OnPropertyChanged(nameof(AddressError));
                }
            }
        }

        public bool CompanyNameError
        {
            get { return companyNameError; }
            set
            {
                if (companyNameError != value)
                {
                    companyNameError = value;
                    OnPropertyChanged(nameof(CompanyNameError));
                }
            }
        }

        public bool LatinCompanyNameError
        {
            get { return latinCompanyNameError; }
            set
            {
                if (latinCompanyNameError != value)
                {
                    latinCompanyNameError = value;
                    OnPropertyChanged(nameof(LatinCompanyNameError));
                }
            }
        }

        public bool LatinAddressError
        {
            get { return latinAddressError; }
            set
            {
                if (latinAddressError != value)
                {
                    latinAddressError = value;
                    OnPropertyChanged(nameof(LatinAddressError));
                }
            }
        }

        public bool IsFullPhoneVisible
        {
            get { return isFullPhoneVisible; }
            set
            {
                if (isFullPhoneVisible != value)
                {
                    isFullPhoneVisible = value;
                    OnPropertyChanged(nameof(IsFullPhoneVisible));
                }
            }
        }

        public bool UpdatedRegionOnLoading
        {
            get { return this._updatedRegionOnLoading; }
            set { SetProperty(ref this._updatedRegionOnLoading, value); }
        }

        public bool UpdatedMunicipalityOnLoading
        {
            get { return this._updatedMunicipalityOnLoading; }
            set { SetProperty(ref this._updatedMunicipalityOnLoading, value); }
        }

        public bool IsUpdatingCollection
        {
            get { return _isUpdatingCollection; }
            set { SetProperty(ref _isUpdatingCollection, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async void Save()
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

                var phone = this.PhoneNumberCode + this.Phone;

                var httpClient = HttpClientFactory.Create();
                string jwt = App.JwtToken;

                if (!string.IsNullOrEmpty(this.OldPassword) && !string.IsNullOrEmpty(this.Password))
                {

                    var changePasswordResult = await this._authenticationService.ChangeUserPassword(this.Email, this.OldPassword, this.Password, httpClient);

                    if (changePasswordResult.Code != (int)GeneralStatusEnum.Success)
                    {
                        throw new Exception(MessageConstants.ChangePasswordError);
                    }

                    this._isPasswordChanged = true;
                }

                var nameArr = this.FullName.Split(" ").ToArray();
                var latinList = new List<string>();
                if (!string.IsNullOrEmpty(this.FullNameEng))
                {

                    latinList = this.FullNameEng.Split(" ").ToList();
                }


                var req = new EditUserInfoInput
                {
                    UserId = App.UserId,
                    FirstName = nameArr[0].ToString().TrimEnd(),
                    SurName = nameArr[1].ToString().TrimEnd(),
                    LastName = nameArr[2].ToString().TrimEnd(),
                    LatinFirstName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[0].ToString().TrimEnd(),
                    LatinSurName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[1].ToString().TrimEnd(),
                    LatinLastName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[2].ToString().TrimEnd(),
                    Phone = string.IsNullOrEmpty(this.Phone) ? null : this.Phone.TrimEnd(),
                    Address = string.IsNullOrEmpty(this.Address) ? null : this.Address.TrimEnd(),
                    Email = string.IsNullOrEmpty(this.Email) ? null : this.Email.TrimEnd(),
                    CountryId = this.CountryCollection.FirstOrDefault(x => x.Id == GlobalConstants.BgCountryCode).Id,
                    CityId = this.SelectedCityModel.Id,
                    RegionId = this.SelectedRegion.Id,
                    MuniciplalityId = this.SelectedMunicipality.Id,
                    PostCode = this.SelectedCityModel.PostCode,
                    CompanyName = string.IsNullOrEmpty(this.CompanyName) ? null : this.CompanyName.TrimEnd(),
                    LatinCompanyName = string.IsNullOrEmpty(this.LatinCompanyName) ? null : this.LatinCompanyName.TrimEnd(),
                    LatinAddress = string.IsNullOrEmpty(this.LatinAddress) ? null : this.LatinAddress.TrimEnd(),
                    BirthDate = this.BirthDate,
                };

                var editUserInfoResult = await this._userService.EditUserInfo(req, httpClient, jwt);

                if (string.IsNullOrEmpty(editUserInfoResult.Id))
                {
                    if (!string.IsNullOrEmpty(editUserInfoResult.Message))
                    {
                        throw new Exception(editUserInfoResult.Message);
                    }
                    throw new Exception(MessageConstants.EditUserError);
                }


                if (this._isPasswordChanged)
                {
                    await App.DisplayAlert(App.MESSAGE_HEADER_ОК, MessageConstants.ChangePasswordSuccess, App.MESSAGE_OK);
                }

                App.Email = this.Email;
                await this.ShowSuccessToast();
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
        public async Task GetUserInfo()
        {
            if (!string.IsNullOrEmpty(App.UserId))
            {
                var httpClient = HttpClientFactory.Create();
                var userId = App.UserId;
                var jwt = App.JwtToken;

                var currentUser = await this._userService.GetUserById(userId, httpClient, jwt);

                if (currentUser != null)
                {
                    this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == currentUser.RegionId);
                    this.SelectedMunicipality = this.MunicipalityCollection.FirstOrDefault(x => x.Id == currentUser.MunicipalityId);
                    this.SelectedCityModel = this.CityCollection.FirstOrDefault(x => x.Id == currentUser.CityId);

                    if (!string.IsNullOrEmpty(currentUser.FirstName) && !string.IsNullOrEmpty(currentUser.SurName) && !string.IsNullOrEmpty(currentUser.LastName))
                    {
                        this.FullName = currentUser.FirstName + " " + currentUser.SurName + " " + currentUser.LastName;
                    }
                        
                    if (!string.IsNullOrEmpty(currentUser.LatinFirstName) && !string.IsNullOrEmpty(currentUser.LatinSurName) && !string.IsNullOrEmpty(currentUser.LatinLastName))
                    {
                        this.FullNameEng = currentUser.LatinFirstName + " " + currentUser.LatinSurName + " " + currentUser.LatinLastName;
                    }

                    this.Email = currentUser.Email;
                    this.UinNumber = currentUser.UiNumber;

                    if (!string.IsNullOrEmpty(currentUser.PhoneNumber))
                    {
                        this.Phone = currentUser.PhoneNumber;
                        IsFullPhoneVisible = true;
                    }
                    this.Address = currentUser.Address;
                    this.LatinAddress = currentUser.LatinAddress;

                    if (currentUser.BirthDate.HasValue)
                    {
                        this.BirthDate = currentUser.BirthDate.Value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currentUser.UiNumber))
                        {
                            ulong uiNumber = 0;

                            bool parsed = ulong.TryParse(currentUser.UiNumber.TrimEnd(), out uiNumber);

                            if (parsed)
                            {
                                var date = Helpers.GetDateFromEgn(uiNumber);

                                this.BirthDate = date;
                            }
                        }
                        else { this.BirthDate = DateTime.Now; }
                    }

                    if (!string.IsNullOrEmpty(currentUser.CompanyName))
                    {
                        this.CompanyName = currentUser.CompanyName;
                        this.LatinCompanyName = currentUser.LatinCompanyName;
                        this.IsVisibleCompanyField = true;
                    }

                    if (!string.IsNullOrEmpty(currentUser.RegionId))
                    {                        
                        this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == currentUser.RegionId);

                        if (this.SelectedRegion != null)
                        {
                            await this.GetMunicipality(this.SelectedRegion);
                            this._updatedRegionOnLoading = true;
                           
                            this.SelectedMunicipality = this.MunicipalityCollection.FirstOrDefault(x => x.Id == currentUser.MunicipalityId);
                            
                            if (this.SelectedMunicipality != null)
                            {
                                await this.GetCities(this.SelectedMunicipality);
                                _updatedMunicipalityOnLoading = true;

                                this.SelectedCityModel = this.CityCollection.FirstOrDefault(x => x.Id == currentUser.CityId);
                                
                            }
                        }
                    }
                }
            }
        }
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
            if (this.UpdatedRegionOnLoading)
            {
                this.UpdatedRegionOnLoading = false;
                return;
            }

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
            if (this.UpdatedMunicipalityOnLoading)
            {
                this.UpdatedMunicipalityOnLoading = false;
                return;
            }

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

            if (string.IsNullOrEmpty(this.Phone))
            {
                this.PhoneError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredPhone;
            }
            else
            {
                if (!this.PhoneValidation())
                {
                    this.PhoneError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidPhone;
                }
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
                var validFormat = this.ValidateBulgarianName();

                if (!validFormat)
                {
                    this.FullNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.Invalid;
                }
            }

            if (!string.IsNullOrEmpty(this.FullNameEng))
            {
                var validFormat = this.ValidateLatinName();

                if (!validFormat)
                {
                    this.FullNameEngError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.Invalid;
                }
            }

            if (!string.IsNullOrEmpty(this.OldPassword) && !string.IsNullOrEmpty(this.Password))
            {
                if (this.Password != this.ConfirmPassword)
                {
                    this.PasswordError = true;
                    this.IsError = true;
                    this.ConfirmPasswordError = true;
                    res.Message = MessageConstants.PassNotMatch;
                }
            }

            if (!string.IsNullOrEmpty(this.Password) && !string.IsNullOrEmpty(this.ConfirmPassword) && string.IsNullOrEmpty(this.OldPassword))
            {
                this.OldPasswordError = true;
                this.IsError = true;
                res.Message = MessageConstants.CurrentPasswordError;
            }

            if (!string.IsNullOrEmpty(this.Address))
            {
                var valid = this.ValidateAddress();

                if (!valid)
                {
                    this.AddressError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.AddressError;
                }
            }

            if (!string.IsNullOrEmpty(this.LatinAddress))
            {
                var valid = this.ValidateLatinAddress();

                if (!valid)
                {
                    this.LatinAddressError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.LatinAddressError;
                }
            }

            if (!string.IsNullOrEmpty(this.CompanyName))
            {
                var valid = this.ValidateBulgarianCompanyName();

                if (!valid)
                {
                    this.CompanyNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (!string.IsNullOrEmpty(this.LatinCompanyName))
            {
                var valid = this.ValidateLatinCompanyName();

                if (!valid)
                {
                    this.LatinCompanyNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.LatinnError;
                }
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
        private bool ValidateLatinName()
        {
            if (Regex.IsMatch(this.FullNameEng.TrimEnd(), GlobalConstants.LatinFullNamesRegex))
            {
                return true;
            }

            return false;
        }

        private bool ValidateLatinCompanyName()
        {
            if (Regex.IsMatch(this.LatinCompanyName.TrimEnd(), GlobalConstants.LatinCompanyRegex))
            {
                return true;
            }

            return false;
        }

        private bool ValidateBulgarianName()
        {
            if (Regex.IsMatch(this.FullName.TrimEnd(), GlobalConstants.BulgarianFullNamesRegex))
            {
                return true;
            }

            return false;
        }

        private bool ValidateBulgarianCompanyName()
        {
            if (Regex.IsMatch(this.CompanyName.TrimEnd(), GlobalConstants.BulgarianCompanyRegex))
            {
                return true;
            }

            return false;
        }
        private bool ValidateBulgarian(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.BulgarianNameRegex))
            {
                return true;
            }

            return false;
        }
        private bool CheckEgnAndBirthDateCompatibility()
        {
            ulong uiNumber = 0;

            bool parsed = ulong.TryParse(this.UinNumber.TrimEnd(), out uiNumber);

            if (!parsed)
            {
                return false;
            }

            bool isValid = Helpers.ValidateEgnAndBirthDate(this.BirthDate, uiNumber);

            return isValid;
        }
        private bool ValidateAddress()
        {
            if (Regex.IsMatch(this.Address.TrimEnd(), GlobalConstants.AddressRegex))
            {
                return true;
            }

            return false;
        }

        private bool ValidateLatinAddress()
        {
            if (Regex.IsMatch(this.LatinAddress.TrimEnd(), GlobalConstants.LatinAddressRegex))
            {
                return true;
            }

            return false;
        }
        private async Task ShowSuccessToast()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string clipBoardText = MessageConstants.EditProfilSuccess;

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(clipBoardText, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
        #endregion
    }
}
