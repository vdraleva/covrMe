using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CovrMe.ViewModels.Pages.Profile
{
    public partial class FamilyFriendsDataPageViewModel : BaseViewModel
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
        private LocationDataModel _selectedNationality;

        private UserFamilyAndFriendsPicker _selectedUser;
        private string fullName;
        private string fullNameEng;
        private string _address;
        private string _uin;
        private string _latinAddress;

        private bool _isError = false;
        private bool _surNameError;
        private bool _uinError;
        private bool _UiNumberError;
        private bool _addressError;
        private bool _latinAddressError;
        private bool _fullNameError;
        private bool _fullNameEngError;
        private bool _birthDateError;

        private bool _isUpdatingCollection = false;
        private bool _isUpdatingOnLoading = false;

        //services
        private ILocationService _locationService;
        private IUserService _userService;
        private IAuthenticationService _authenticationService;

        //collections
        private ObservableCollection<LocationDataModel> _regionCollection;
        private ObservableCollection<LocationDataModel> _municipalityCollection;
        private ObservableCollection<CityDataModel> _cityCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;
        private ObservableCollection<UserFamilyAndFriendsPicker> _userCollection;
        #endregion
        public FamilyFriendsDataPageViewModel(ILocationService locationService, IUserService userService)
        {
            this._locationService = locationService;
            this._userService = userService;

            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1900");
            this.BirthDate = DateTime.Parse("01/01/1990");

            this.RegionCollection = new ObservableCollection<LocationDataModel>();
            this.MunicipalityCollection = new ObservableCollection<LocationDataModel>();
            this.CityCollection = new ObservableCollection<CityDataModel>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();
            this.UserCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();

            Task.Run(async () => { await GetCountries(); }).Wait();
            Task.Run(async () => { await GetRegions(); }).Wait();
            Task.Run(async () => { await GetUserFamilyAndFriends(); }).Wait();
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

        public ObservableCollection<UserFamilyAndFriendsPicker> UserCollection
        {
            get => _userCollection;
            set => SetProperty(ref _userCollection, value);
        }
        #endregion

        #region Props

        public bool IsUpdatingCollection
        {
            get { return _isUpdatingCollection; }
            set { SetProperty(ref _isUpdatingCollection, value); }
        }
        public bool IsUpdatingOnLoading
        {
            get { return _isUpdatingOnLoading; }
            set { SetProperty(ref _isUpdatingOnLoading, value); }
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

        public UserFamilyAndFriendsPicker SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

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
        public LocationDataModel SelectedNationality
        {
            get { return _selectedNationality; }
            set { SetProperty(ref _selectedNationality, value); }
        }

        public string Address
        {
            get { return this._address; }
            set { SetProperty(ref this._address, value); }
        }

        public string LatinAddress
        {
            get { return this._latinAddress; }
            set { SetProperty(ref this._latinAddress, value); }
        }

        public string Uin
        {
            get { return this._uin; }
            set { SetProperty(ref this._uin, value); }
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

        public bool UinError
        {
            get { return _uinError; }
            set
            {
                if (_uinError != value)
                {
                    _uinError = value;
                    OnPropertyChanged(nameof(UinError));
                }
            }
        }

        public bool AddressError
        {
            get { return _addressError; }
            set
            {
                if (_addressError != value)
                {
                    _addressError = value;
                    OnPropertyChanged(nameof(AddressError));
                }
            }
        }

        public bool LatinAddressError
        {
            get { return _latinAddressError; }
            set
            {
                if (_latinAddressError != value)
                {
                    _latinAddressError = value;
                    OnPropertyChanged(nameof(LatinAddressError));
                }
            }
        }

        public bool BirthDateError
        {
            get { return _birthDateError; }
            set
            {
                if (_birthDateError != value)
                {
                    _birthDateError = value;
                    OnPropertyChanged(nameof(BirthDateError));
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
        public bool FullNameEngError
        {
            get { return _fullNameEngError; }
            set
            {
                if (_fullNameEngError != value)
                {
                    _fullNameEngError = value;
                    OnPropertyChanged(nameof(FullNameEngError));
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

                var result = new UserModel();
                string message = string.Empty;

                if (this.SelectedUser == null || this.SelectedUser.Id == GlobalConstants.New)
                {
                    result = await this.AddUser();
                    message = MessageConstants.FamilyFriendsSuccess;
                }
                else
                {
                    result = await this.EditUser();
                    message = MessageConstants.EditProfilSuccess;
                }

                if (!string.IsNullOrEmpty(result.Id))
                {
                    this.UpdateUserCollection(result);
                    await this.ShowSuccessToast(message);
                }
                else
                {
                    throw new Exception(MessageConstants.GeneralError);
                }
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                this.IsUpdatingCollection = false;
                IsBusy = false;
                HideLoading();
            }
        }
        #endregion

        #region Methods

        public async Task ChangeUser(UserFamilyAndFriendsPicker user)
        {
            try
            {
                ShowLoading();

                this.IsUpdatingCollection = true;
                this.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == user.Nationality);
                this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == user.Region);

                if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.SurName) && !string.IsNullOrEmpty(user.LastName))
                {
                    this.FullName = user.FirstName + " " + user.SurName + " " + user.LastName;
                }
                    
                if (!string.IsNullOrEmpty(user.LatinFirstName) && !string.IsNullOrEmpty(user.LatinSurName) && !string.IsNullOrEmpty(user.LatinLastName))
                {
                    this.FullNameEng = user.LatinFirstName + " " + user.LatinSurName + " " + user.LatinLastName;
                }

                this.Uin = user.UiNumber;
                this.Address = user.Address;
                this.LatinAddress = user.LatinAddress;

                if (user.BirthDate.HasValue)
                {
                    this.BirthDate = user.BirthDate.Value;
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.UiNumber))
                    {
                        PopulateBirthDate();
                    }
                    else
                    {
                        this.BirthDate = DateTime.Parse("01/01/1990");
                    }
                }

                if (this.SelectedRegion != null)
                {
                    await GetMunicipality(this.SelectedRegion);
                }

                this.SelectedMunicipality = this.MunicipalityCollection.FirstOrDefault(x => x.Id == user.Municipality);

                if (this.SelectedMunicipality != null)
                {
                    await GetCities(this.SelectedMunicipality);
                }

                this.SelectedCityModel = this.CityCollection.FirstOrDefault(x => x.Id == user.City);

                this.SelectedUser = user;

            }
            catch (Exception ex)
            {

                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                this.IsUpdatingCollection = false;
                HideLoading();
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

        public async Task GetUserFamilyAndFriends()
        {
            if (!string.IsNullOrEmpty(App.UserId))
            {
                var userCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();
                var httpClient = HttpClientFactory.Create();
                var userId = App.UserId;
                var jwt = App.JwtToken;

                var users = await this._userService.GetUserFamilyAndFriends(userId, httpClient, jwt);

                var newUser = new UserFamilyAndFriendsPicker
                {
                    Id = GlobalConstants.New,
                    Names = GlobalConstants.New,
                    BirthDate = DateTime.Now
                };

                userCollection.Add(newUser);

                foreach (var user in users.FamilyAndFriends)
                {
                    var current = new UserFamilyAndFriendsPicker
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        SurName = user.SurName,
                        LastName = user.LastName,
                        LatinFirstName = user.LatinFirstName,
                        LatinSurName = user.LatinSurName,
                        LatinLastName = user.LatinLastName,
                        UiNumber = user.UiNumber,
                        Address = user.Address,
                        Municipality = user.MunicipalityId,
                        Region = user.RegionId,
                        City = user.CityId,
                        Nationality = user.CountryId,
                        Names = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)) ? user.FirstName + " " + user.LastName : ((!string.IsNullOrEmpty(user.LatinFirstName) && !string.IsNullOrEmpty(user.LatinLastName)) ? user.LatinFirstName + " " + user.LatinLastName : GlobalConstants.NoName),
                        LatinAddress = user.LatinAddress,
                        BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Parse("01/01/1990")
                    };

                    userCollection.Add(current);
                }
                this.UserCollection.Clear();
                this.UserCollection = userCollection;

                if (this.UserCollection.Count > 1)
                {
                    this.IsUpdatingOnLoading = true;

                    var current = this.UserCollection.FirstOrDefault(x => x.Id != GlobalConstants.New);

                    this.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == current.Nationality);

                    this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == current.Region);

                    if (!string.IsNullOrEmpty(current.FirstName) && !string.IsNullOrEmpty(current.SurName) && !string.IsNullOrEmpty(current.LastName))
                    {
                        this.FullName = current.FirstName + " " + current.SurName + " " + current.LastName;
                    }
                        
                    if (!string.IsNullOrEmpty(current.LatinFirstName) && !string.IsNullOrEmpty(current.LatinSurName) && !string.IsNullOrEmpty(current.LatinLastName))
                    {
                        this.FullNameEng = current.LatinFirstName + " " + current.LatinSurName + " " + current.LatinLastName;
                    }

                    this.Uin = current.UiNumber;
                    this.Address = current.Address;
                    this.LatinAddress = current.LatinAddress;

                    if (current.BirthDate.HasValue)
                    {
                        this.BirthDate = current.BirthDate.Value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.Uin))
                        {
                            PopulateBirthDate();
                        }
                        else
                        {
                            this.BirthDate = DateTime.Parse("01/01/1990");
                        }
                    }
                    //this.BirthDate = current.BirthDate.HasValue ? current.BirthDate.Value : DateTime.Now;

                    if (this.SelectedRegion != null)
                    {
                        await GetMunicipality(this.SelectedRegion);
                    }

                    this.SelectedMunicipality = this.MunicipalityCollection.FirstOrDefault(x => x.Id == current.Municipality);

                    if (this.SelectedMunicipality != null)
                    {
                        await GetCities(this.SelectedMunicipality);
                    }

                    this.SelectedCityModel = this.CityCollection.FirstOrDefault(x => x.Id == current.City);
                    this.SelectedUser = current;
                }
                else
                {
                    this.IsUpdatingOnLoading = true;

                    var current = this.UserCollection.FirstOrDefault(x => x.Id == GlobalConstants.New);

                    this.FullName = string.Empty;

                    this.Uin = string.Empty;
                    this.Address = string.Empty;
                    this.LatinAddress = string.Empty;
                    this.BirthDate = DateTime.Parse("01/01/1990");
                    
                    this.SelectedUser = current;
                }
            }
        }
        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

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
                    this.AddressError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.LatinAddressError;
                }
            }

            if (!string.IsNullOrEmpty(this.Uin))
            {
                if (!UiNumberValidation())
                {
                    this.UinError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidUin;
                }
                else
                {
                    if (!CheckEgnAndBirthDateCompatibility())
                    {
                        this.UinError = true;
                        this.BirthDateError = true;
                        this.IsError = true;
                        res.IsValid = false;
                        res.Message = MessageConstants.EgnAndDateIncompatible;
                    }
                }
                if (UinNumberExists())
                {
                    this.UinError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.ExistsUin;
                }
            }

            return res;
        }
        private bool UiNumberValidation()
        {
            ulong uiNumber = 0;

            bool parsed = ulong.TryParse(this.Uin.TrimEnd(), out uiNumber);

            if (!parsed)
            {
                return false;
            }

            bool isValid = Helpers.EgnValidation(uiNumber);

            return isValid;
        }
        private bool ValidateLatinName()
        {
            if (Regex.IsMatch(this.FullNameEng.TrimEnd(), GlobalConstants.LatinFullNamesRegex))
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

        private bool UinNumberExists()
        {
            var users = this.UserCollection;

            if (!string.IsNullOrEmpty(this.Uin))
            {
                return users.Any(x => x.UiNumber == this.Uin && x.Id != this.SelectedUser.Id);
            }

            return false;
        }

        private bool ValidateBulgarian(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.LatinNameRegex))
            {
                return true;
            }

            return false;
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

        private async Task ShowSuccessToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string clipBoardText = message;

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(clipBoardText, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        private async Task<UserModel> AddUser()
        {
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var nameArr = this.FullName.Split(" ").ToArray();

            var latinList = new List<string>();
            if (!string.IsNullOrEmpty(this.FullNameEng))
            {

                latinList = this.FullNameEng.Split(" ").ToList();
            }

            var req = new AddUserToFamilyAndFriendsInput
            {
                UserId = App.UserId,
                FirstName = nameArr[0].ToString().TrimEnd(),
                SurName = nameArr[1].ToString().TrimEnd(),
                LastName = nameArr[2].ToString().TrimEnd(),
                LatinFirstName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[0].ToString().TrimEnd(),
                LatinSurName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[1].ToString().TrimEnd(),
                LatinLastName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[2].ToString().TrimEnd(),
                Address = string.IsNullOrEmpty(this.Address) ? null : this.Address.TrimEnd(),
                LatinAddress = string.IsNullOrEmpty(this.LatinAddress) ? null : this.LatinAddress.TrimEnd(),
                UiNumber = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd(),
                CityId = this.SelectedCityModel == null ? null : this.SelectedCityModel.Id,
                RegionId = this.SelectedRegion == null ? null : this.SelectedRegion.Id,
                MuniciplalityId = this.SelectedMunicipality == null ? null : this.SelectedMunicipality.Id,
                PostCode = this.SelectedCityModel == null ? null : this.SelectedCityModel.PostCode,
                CountryId = this.CountryCollection.FirstOrDefault(x => x.Id == GlobalConstants.BgCountryCode).Id,
                Birthdate = this.BirthDate
            };

            var resut = await this._userService.AddUserToFamilyAndFriends(req, httpClient, jwt);

            return resut;
        }

        private async Task<UserModel> EditUser()
        {
            var nameArr = this.FullName.Split(" ").ToArray();
            var latinList = new List<string>();
            if (!string.IsNullOrEmpty(this.FullNameEng))
            {

                latinList = this.FullNameEng.Split(" ").ToList();
            }

            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var req = new EditUserInfoInput
            {
                UserId = this.SelectedUser.Id,
                ParentUserId = App.UserId,
                FirstName = nameArr[0].ToString().TrimEnd(),
                SurName = nameArr[1].ToString().TrimEnd(),
                LastName = nameArr[2].ToString().TrimEnd(),
                LatinFirstName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[0].ToString().TrimEnd(),
                LatinSurName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[1].ToString().TrimEnd(),
                LatinLastName = string.IsNullOrEmpty(this.FullNameEng) ? null : latinList[2].ToString().TrimEnd(),
                Address = string.IsNullOrEmpty(this.Address) ? null : this.Address.TrimEnd(),
                LatinAddress = string.IsNullOrEmpty(this.LatinAddress) ? null : this.LatinAddress.TrimEnd(),
                UiNumber = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd(),
                CityId = this.SelectedCityModel == null ? null : this.SelectedCityModel.Id,
                RegionId = this.SelectedRegion == null ? null : this.SelectedRegion.Id,
                MuniciplalityId = this.SelectedMunicipality == null ? null : this.SelectedMunicipality.Id,
                PostCode = this.SelectedCityModel == null ? null : this.SelectedCityModel.PostCode,
                CountryId = this.CountryCollection.FirstOrDefault(x => x.Id == GlobalConstants.BgCountryCode).Id,
                BirthDate = this.BirthDate
            };

            var result = await this._userService.EditUserInfo(req, httpClient, jwt);

            return result;
        }
        private bool CheckEgnAndBirthDateCompatibility()
        {
            if (!string.IsNullOrEmpty(this.Uin))
            {
                ulong uiNumber = 0;

                bool parsed = ulong.TryParse(this.Uin.TrimEnd(), out uiNumber);

                if (!parsed)
                {
                    return false;
                }

                bool isValid = Helpers.ValidateEgnAndBirthDate(this.BirthDate, uiNumber);

                return isValid;
            }
            else
            {
                return false;
            }
        }

        public void PopulateBirthDate()
        {
            if (this.SelectedNationality != null && this.SelectedNationality.Id == GlobalConstants.BgCountryCode && !string.IsNullOrEmpty(this.Uin))
            {
                ulong uiNumber = 0;

                bool parsed = ulong.TryParse(this.Uin.TrimEnd(), out uiNumber);

                if (parsed)
                {
                    if (Helpers.EgnValidation(uiNumber))
                    {
                        var date = Helpers.GetDateFromEgn(uiNumber);

                        this.BirthDate = date;
                    }
                }
            }               
        }

        private async void UpdateUserCollection(UserModel user)
        {
            var current = this.UserCollection.FirstOrDefault(x => x.Id.ToLower() == user.Id.ToLower());

            if (current != null)
            {
                this.UserCollection.Remove(current);
            }

            current = new UserFamilyAndFriendsPicker
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SurName = user.SurName,
                LastName = user.LastName,
                LatinFirstName = user.LatinFirstName,
                LatinSurName = user.LatinSurName,
                LatinLastName = user.LatinLastName,
                UiNumber = user.UiNumber,
                Address = user.Address,
                Municipality = user.MunicipalityId,
                Region = user.RegionId,
                City = user.CityId,
                Nationality = user.CountryId,
                Names = user.FirstName + " " + user.LastName,
                LatinAddress = user.LatinAddress,
                BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Now
            };

            this.UserCollection.Add(current);
            await this.ChangeUser(current);
        }

        #endregion
    }
}
