using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceUsualDriverViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        
        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private UserFamilyAndFriendsPicker _selectedUser;
        

        private string _usualFirstName;
        private string _usualSurName;
        private string _usualLastName;
        private string _usualUin;
        private string _usualAddress;
        private string _usualUserId;
        private DateTime _usualBirthDate;

        private LocationDataModel _selectedRegionUsual;
        private LocationDataModel _selectedMunicipalityUsual;
        private CityDataModel _selectedCityModelUsual;
        private LocationDataModel _selectedNationalityUsual;
        private DrivingExperiencePickerModel _selectedDrivingExperienceUsual;
        private BaseDataModel _selectedGuiltType;

        private bool _isError = false;
        private bool _usualFirstNameError;
        private bool _usualLastNameError;
        private bool _usualSurNameError;
        private bool _usualUinError;
        private bool _usualAddressError;
        private bool _birthDateError;
        private bool _save;
        private bool _isUpdatingCollection = false;

        //services
        private ILocationService _locationService;
        private IUserService _userService;

        //collections

        private ObservableCollection<LocationDataModel> _regionCollection;
        private ObservableCollection<LocationDataModel> _municipalityCollection;
        private ObservableCollection<CityDataModel> _cityCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;
        private ObservableCollection<DrivingExperiencePickerModel> _drivingExperienceCollection;
        private ObservableCollection<UserFamilyAndFriendsPicker> _userCollection;
        private ObservableCollection<BaseDataModel> _userGuiltCollection;

        #endregion

        public CivilInsuranceUsualDriverViewModel(ILocationService locationService, IUserService userService)
        {
            this._locationService = locationService;
            this._userService = userService;
            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1900");
            this.UsualBirthDate = DateTime.Parse("01/01/1990");

            this.RegionCollection = new ObservableCollection<LocationDataModel>();
            this.MunicipalityCollection = new ObservableCollection<LocationDataModel>();
            this.CityCollection = new ObservableCollection<CityDataModel>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();
            this.DrivingExperienceCollection = new ObservableCollection<DrivingExperiencePickerModel>();
            this.UserCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();
            this.UserGuiltCollection = new ObservableCollection<BaseDataModel>();

            Task.Run(async () => { await GetCountries(); }).Wait();
            Task.Run(async () => { await GetRegions(); }).Wait();
            Task.Run(async () => { await PopulateDrivingExperienceCollection(); }).Wait();
            Task.Run(async () => { await GetGuiltTypes(); }).Wait();
            Task.Run(async () => { await GetCurrentUserInfo(); }).Wait();
            Task.Run(async () => { await GetUserFamilyAndFriends(); }).Wait();
        }

        #region Collections

        public ObservableCollection<BaseDataModel> UserGuiltCollection
        {
            get => _userGuiltCollection;
            set => SetProperty(ref _userGuiltCollection, value);
        }
        public ObservableCollection<LocationDataModel> RegionCollection
        {
            get => _regionCollection;
            set => SetProperty(ref _regionCollection, value);
        }

        public ObservableCollection<LocationDataModel> MunicipalityCollection
        {
            get => _municipalityCollection;
            set => SetProperty(ref _municipalityCollection, value);
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
        public ObservableCollection<UserFamilyAndFriendsPicker> UserCollection
        {
            get => _userCollection;
            set => SetProperty(ref _userCollection, value);
        }

        public ObservableCollection<DrivingExperiencePickerModel> DrivingExperienceCollection
        {
            get => _drivingExperienceCollection;
            set => SetProperty(ref _drivingExperienceCollection, value);
        }

        #endregion

        #region Props

        public BaseDataModel SelectedGuiltType
        {
            get { return _selectedGuiltType; }
            set { SetProperty(ref _selectedGuiltType, value); }
        }
        public bool IsUpdatingCollection
        {
            get { return _isUpdatingCollection; }
            set { SetProperty(ref _isUpdatingCollection, value); }
        }

        public UserFamilyAndFriendsPicker SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public LocationDataModel SelectedRegionUsual
        {
            get { return _selectedRegionUsual; }
            set { SetProperty(ref _selectedRegionUsual, value); }
        }

        public LocationDataModel SelectedMunicipalityUsual
        {
            get { return _selectedMunicipalityUsual; }
            set { SetProperty(ref _selectedMunicipalityUsual, value); }
        }

        public CityDataModel SelectedCityModelUsual
        {
            get { return _selectedCityModelUsual; }
            set { SetProperty(ref _selectedCityModelUsual, value); }
        }

        public LocationDataModel SelectedNationalityUsual
        {
            get { return _selectedNationalityUsual; }
            set { SetProperty(ref _selectedNationalityUsual, value); }
        }

        public DrivingExperiencePickerModel SelectedDrivingExperienceUsual
        {
            get { return _selectedDrivingExperienceUsual; }
            set { SetProperty(ref _selectedDrivingExperienceUsual, value); }
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

        public DateTime UsualBirthDate
        {
            get { return _usualBirthDate; }
            set { SetProperty(ref _usualBirthDate, value); }
        }

        public string UsualUin
        {
            get { return this._usualUin; }
            set { SetProperty(ref this._usualUin, value); }
        }

        public string UsualAddress
        {
            get { return this._usualAddress; }
            set { SetProperty(ref this._usualAddress, value); }
        }
        public string UsualFirstName
        {
            get { return this._usualFirstName; }
            set { SetProperty(ref this._usualFirstName, value); }
        }

        public string UsualLastName
        {
            get { return this._usualLastName; }
            set { SetProperty(ref this._usualLastName, value); }
        }
        public string UsualSurName
        {
            get { return this._usualSurName; }
            set { SetProperty(ref this._usualSurName, value); }
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

        public bool UsualFirstNameError
        {
            get { return _usualFirstNameError; }
            set
            {
                if (_usualFirstNameError != value)
                {
                    _usualFirstNameError = value;
                    OnPropertyChanged(nameof(UsualFirstNameError));
                }
            }
        }

        public bool UsualLastNameError
        {
            get { return _usualLastNameError; }
            set
            {
                if (_usualLastNameError != value)
                {
                    _usualLastNameError = value;
                    OnPropertyChanged(nameof(UsualLastNameError));
                }
            }
        }

        public bool UsualSurNameError
        {
            get { return _usualSurNameError; }
            set
            {
                if (_usualSurNameError != value)
                {
                    _usualSurNameError = value;
                    OnPropertyChanged(nameof(UsualSurNameError));
                }
            }
        }

        public bool UsualAddressError
        {
            get { return _usualAddressError; }
            set
            {
                if (_usualAddressError != value)
                {
                    _usualAddressError = value;
                    OnPropertyChanged(nameof(UsualAddressError));
                }
            }
        }

        public bool UsualUinError
        {
            get { return _usualUinError; }
            set
            {
                if (_usualUinError != value)
                {
                    _usualUinError = value;
                    OnPropertyChanged(nameof(UsualUinError));
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

        public bool Save
        {
            get { return this._save; }
            set { SetProperty(ref this._save, value); }
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

                if (this.SelectedRegionUsual == null)
                {
                    throw new Exception(MessageConstants.RegionNotSelected);
                }

                if (this.SelectedMunicipalityUsual == null)
                {
                    throw new Exception(MessageConstants.MuniciplalityNotSelected);
                }

                if (this.SelectedGuiltType == null)
                {
                    throw new Exception(MessageConstants.GuiltNotSelected);
                }

                if (this.SelectedCityModelUsual == null)
                {
                    throw new Exception(MessageConstants.CityNotSelected);
                }

                if (this.SelectedDrivingExperienceUsual == null)
                {
                    throw new Exception(MessageConstants.DrivingExpNotSelected);
                }

                if (this.SelectedNationalityUsual == null)
                {
                    throw new Exception(MessageConstants.NationalityNotSelected);
                }

                if (this.Save)
                {
                    var saveResult = new UserModel(); ;

                    if (this.SelectedUser != null && this.SelectedUser.Id == App.UserId)
                    {
                        saveResult = await this.EditUserInfo();
                    }
                    else if (this.SelectedUser == null || this.SelectedUser.Id == GlobalConstants.New)
                    {
                        saveResult = await this.AddUser();
                    }
                    else
                    {
                        saveResult = await this.EditUserInfo(this.SelectedUser.Id);
                    }

                    if (string.IsNullOrEmpty(saveResult.Id))
                    {
                        throw new Exception(MessageConstants.GeneralError);
                    }

                    await this.UpdateUserCollection(saveResult);
                }

                var userInfo = new InsuranceUserInfoModel
                {
                    UserId = this.SelectedUser.Id == GlobalConstants.New ? string.Empty : this.SelectedUser.Id,
                    FirstName = this.UsualFirstName.TrimEnd(),
                    LastName = this.UsualLastName.TrimEnd(),
                    SurName = this.UsualSurName.TrimEnd(),
                    Uin = this.UsualUin.TrimEnd(),
                    Address = this.UsualAddress.TrimEnd(),
                    BirthDateString = this.UsualBirthDate.ToString("dd.MM.yyyy"),
                    RegionCode = this.SelectedRegionUsual.Id,
                    MunicipalityCode = this.SelectedMunicipalityUsual.Id,
                    CityCode = this.SelectedCityModelUsual.Id,
                    CountryCode = SelectedNationalityUsual.Id,
                    DrivingExperiance = this.SelectedDrivingExperienceUsual != null ? this.SelectedDrivingExperienceUsual.Number : 0,
                    PostalCode = this.SelectedCityModelUsual.PostCode,
                    RegionName = this.SelectedRegionUsual.Name,
                    MunicipalityName = this.SelectedMunicipalityUsual.Name,
                    CityName = this.SelectedCityModelUsual.Name,
                    GuiltTypeId = this.SelectedGuiltType.Id
                };

                var parameters = new Dictionary<string, object>
                    {

                        {"usualUserInfo", userInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},

                    };                

                await Navigation.PushAsync<CivilInsuranceOffersPage>(parameters);
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
        private async Task PopulateDrivingExperienceCollection()
        {
            var expCollection = new ObservableCollection<DrivingExperiencePickerModel>();

            this.DrivingExperienceCollection.Clear();

            for (int i = 1; i < 11; i++)
            {
                var current = new DrivingExperiencePickerModel()
                {
                    Number = i,
                    Text = $"{i} год"
                };

                expCollection.Add(current);
            }

            var over10 = new DrivingExperiencePickerModel()
            {
                Number = 11,
                Text = "Над 10 год"
            };

            expCollection.Add(over10);

            this.DrivingExperienceCollection = expCollection;
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
        public async Task GetCurrentUserInfo()
        {
            if (!string.IsNullOrEmpty(App.UserId))
            {
                var httpClient = HttpClientFactory.Create();
                var userId = App.UserId;
                var jwt = App.JwtToken;

                var currentUser = await this._userService.GetUserById(userId, httpClient, jwt);

                if (currentUser != null)
                {
                    this.IsUpdatingCollection = true;
                    this.SelectedNationalityUsual = this.CountryCollection.FirstOrDefault(x => x.Id == currentUser.CountryId);
                    this.SelectedRegionUsual = this.RegionCollection.FirstOrDefault(x => x.Id == currentUser.RegionId);
                                       
                    this.SelectedDrivingExperienceUsual = this.DrivingExperienceCollection.FirstOrDefault(x => x.Number == currentUser.DrivingExperience);

                    this.UsualFirstName = currentUser.FirstName;
                    this.UsualSurName = currentUser.SurName;
                    this.UsualLastName = currentUser.LastName;
                    this.UsualUin = currentUser.UiNumber;
                    this.UsualAddress = currentUser.Address;
                    this.UsualBirthDate = currentUser.BirthDate.HasValue ? currentUser.BirthDate.Value : DateTime.Now;

                    var guiltType = this.UserGuiltCollection.FirstOrDefault(x => x.Id == 1);
                    this.SelectedGuiltType = guiltType;

                    if (this.SelectedRegionUsual != null)
                    {
                        await GetMunicipality(this.SelectedRegionUsual);
                    }

                    this.SelectedMunicipalityUsual = this.MunicipalityCollection.FirstOrDefault(x => x.Id == currentUser.MunicipalityId);

                    if (this.SelectedMunicipalityUsual != null)
                    {
                        await GetCities(this.SelectedMunicipalityUsual);
                    }

                    this.SelectedCityModelUsual = this.CityCollection.FirstOrDefault(x => x.Id == currentUser.CityId);

                    if (string.IsNullOrEmpty(currentUser.CountryId) || string.IsNullOrEmpty(currentUser.MunicipalityId) || string.IsNullOrEmpty(currentUser.CityId))
                    {
                        this.IsUpdatingCollection = false;
                    }
                }
            }
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
                    Names = GlobalConstants.New
                };

                var itsMe = new UserFamilyAndFriendsPicker
                {
                    Id = App.UserId,
                    Names = GlobalConstants.ItsMe,
                    FirstName = this.UsualFirstName,
                    SurName = this.UsualSurName,
                    LastName = this.UsualLastName,
                    UiNumber = this.UsualUin,
                    Address = this.UsualAddress,
                    Municipality = this.SelectedMunicipalityUsual != null ? this.SelectedMunicipalityUsual.Id : string.Empty,
                    Region = this.SelectedRegionUsual != null ? SelectedRegionUsual.Id : string.Empty,
                    City = this.SelectedCityModelUsual != null ? SelectedCityModelUsual.Id : string.Empty,
                    Nationality = this.SelectedNationalityUsual != null ? this.SelectedNationalityUsual.Id : string.Empty,
                    BirthDate = this.UsualBirthDate,
                    DrivingExperience = this.SelectedDrivingExperienceUsual != null ? this.SelectedDrivingExperienceUsual.Number : 0,
                    GuiltTypeId = this.SelectedGuiltType.Id
                };

                this.SelectedUser = itsMe;

                userCollection.Add(itsMe);
                userCollection.Add(newUser);


                foreach (var user in users.FamilyAndFriends)
                {
                    var current = new UserFamilyAndFriendsPicker
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        SurName = user.SurName,
                        LastName = user.LastName,
                        VatNumber = user.VatNumber,
                        UiNumber = user.UiNumber,
                        Address = user.Address,
                        Municipality = user.MunicipalityId,
                        Region = user.RegionId,
                        City = user.CityId,
                        Nationality = user.CountryId,
                        BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Now,
                        Names = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)) ? user.FirstName + " " + user.LastName : ((!string.IsNullOrEmpty(user.LatinFirstName) && !string.IsNullOrEmpty(user.LatinLastName)) ? user.FirstName + " " + user.LastName : GlobalConstants.NoName),
                        DrivingExperience = user.DrivingExperience.HasValue ? user.DrivingExperience.Value : 0,
                        GuiltTypeId = 1
                    };

                    userCollection.Add(current);
                }
                this.UserCollection.Clear();
                this.UserCollection = userCollection;
            }
        }
        public async Task ChangeUser(UserFamilyAndFriendsPicker user)
        {
            try
            {
                ShowLoading();

                this.IsUpdatingCollection = true;

                this.SelectedNationalityUsual = this.CountryCollection.FirstOrDefault(x => x.Id == user.Nationality);
                this.SelectedRegionUsual = this.RegionCollection.FirstOrDefault(x => x.Id == user.Region);
                
                this.SelectedDrivingExperienceUsual = this.DrivingExperienceCollection.FirstOrDefault(x => x.Number == user.DrivingExperience);

                this.UsualFirstName = user.FirstName;
                this.UsualSurName = user.SurName;
                this.UsualLastName = user.LastName;
                this.UsualUin = user.UiNumber;
                this.UsualAddress = user.Address;

                if (user.BirthDate.HasValue)
                {
                    this.UsualBirthDate = user.BirthDate.Value;
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.UiNumber))
                    {
                        PopulateBirthDate();
                    }
                    else
                    {
                        this.UsualBirthDate = DateTime.Parse("01/01/1990");
                    }
                }
                if (this.SelectedRegionUsual != null)
                {
                    await GetMunicipality(this.SelectedRegionUsual);
                }

                this.SelectedMunicipalityUsual = this.MunicipalityCollection.FirstOrDefault(x => x.Id == user.Municipality);
                

                if (this.SelectedMunicipalityUsual != null)
                {
                    await GetCities(this.SelectedMunicipalityUsual);
                }

                this.SelectedCityModelUsual = this.CityCollection.FirstOrDefault(x => x.Id == user.City);

                var guiltType = this.UserGuiltCollection.FirstOrDefault(x => x.Id == user.GuiltTypeId);
                this.SelectedGuiltType = guiltType;

                this.SelectedUser = user;

            }
            catch (Exception ex)
            {

                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                //this.IsUpdatingCollection = false;
                HideLoading();
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
        }

        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.UsualFirstName))
            {
                this.UsualFirstNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredFirstName;
            }
            else
            {
                var valid = this.ValidateBulgarian(this.UsualFirstName.TrimEnd());

                if (!valid)
                {
                    this.UsualFirstNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (string.IsNullOrEmpty(this.UsualSurName))
            {
                this.UsualSurNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredSurName;
            }
            else
            {
                var valid = this.ValidateBulgarian(this.UsualSurName.TrimEnd());

                if (!valid)
                {
                    this.UsualSurNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (string.IsNullOrEmpty(this.UsualLastName))
            {
                this.UsualLastNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredLastName;
            }
            else
            {
                var valid = this.ValidateBulgarian(this.UsualLastName.TrimEnd());

                if (!valid)
                {
                    this.UsualLastNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (string.IsNullOrEmpty(this.UsualUin))
            {
                this.UsualUinError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredUin;
            }
            else
            {
                if (this.SelectedNationalityUsual.Id != GlobalConstants.BgCountryCode)
                {
                    if (!ForeignerNumberValidation())
                    {
                        this.UsualUinError = true;
                        this.IsError = true;
                        res.IsValid = false;
                        res.Message = MessageConstants.InvalidUin;
                    }
                }
                else
                {
                    if (!UiNumberValidation())
                    {
                        this.UsualUinError = true;
                        this.IsError = true;
                        res.IsValid = false;
                        res.Message = MessageConstants.InvalidUin;
                    }
                    else
                    {
                        if (!CheckEgnAndBirthDateCompatibility())
                        {
                            this.UsualUinError = true;
                            this.BirthDateError = true;
                            this.IsError = true;
                            res.IsValid = false;
                            res.Message = MessageConstants.EgnAndDateIncompatible;
                        }
                    }
                }
                
            }

            if (string.IsNullOrEmpty(this.UsualAddress))
            {
                this.UsualAddressError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredAddress;
            }
            else
            {
                var valid = this.ValidateAddress();

                if (!valid)
                {
                    this.UsualAddressError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.AddressError;
                }
            }
            
            return res;
        }
        private bool UiNumberValidation()
        {
            ulong uiNumber = 0;

            bool parsed = ulong.TryParse(this.UsualUin.TrimEnd(), out uiNumber);

            if (!parsed)
            {
                return false;
            }

            bool isValid = Helpers.EgnValidation(uiNumber);

            return isValid;
        }
        private bool ForeignerNumberValidation()
        {
            bool isValid = Helpers.ValidateForeignerNumber(this.UsualUin.TrimEnd());

            return isValid;
        }
        private bool ValidateAddress()
        {
            if (Regex.IsMatch(this.UsualAddress.TrimEnd(), GlobalConstants.AddressRegex))
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
        private async Task<UserModel> EditUserInfo(string userId = null)
        {
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var req = new EditUserInfoInput
            {
                UserId = string.IsNullOrEmpty(userId) ? App.UserId : userId,
                Email = string.IsNullOrEmpty(userId) ? App.Email : null,
                ParentUserId = string.IsNullOrEmpty(userId) ? null : App.UserId,
                FirstName = this.UsualFirstName.TrimEnd(),
                SurName = this.UsualSurName.TrimEnd(),
                LastName = this.UsualLastName.TrimEnd(),
                Address = this.UsualAddress.TrimEnd(),
                CityId = this.SelectedCityModelUsual == null ? null : this.SelectedCityModelUsual.Id,
                CountryId = this.SelectedNationalityUsual == null ? null : this.SelectedNationalityUsual.Id,
                RegionId = this.SelectedRegionUsual == null ? null : this.SelectedRegionUsual.Id,
                MuniciplalityId = this.SelectedMunicipalityUsual == null ? null : this.SelectedMunicipalityUsual.Id,
                PostCode = this.SelectedCityModelUsual == null ? null : this.SelectedCityModelUsual.PostCode,
                UiNumber = this.UsualUin.TrimEnd(),
                BirthDate = this.UsualBirthDate,
                DrivingExperience = this.SelectedDrivingExperienceUsual != null ? this.SelectedDrivingExperienceUsual.Number : 0
            };

            var editUserInfoResult = await this._userService.EditUserInfo(req, httpClient, jwt);

            return editUserInfoResult;
        }
        private async Task<UserModel> AddUser()
        {
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var req = new AddUserToFamilyAndFriendsInput
            {
                UserId = App.UserId,
                FirstName = this.UsualFirstName.TrimEnd(),
                SurName = this.UsualSurName.TrimEnd(),
                LastName = this.UsualLastName.TrimEnd(),
                Address = this.UsualAddress.TrimEnd(),
                CityId = this.SelectedCityModelUsual == null ? null : this.SelectedCityModelUsual.Id,
                CountryId = this.SelectedNationalityUsual == null ? null : this.SelectedNationalityUsual.Id,
                RegionId = this.SelectedRegionUsual == null ? null : this.SelectedRegionUsual.Id,
                MuniciplalityId = this.SelectedMunicipalityUsual == null ? null : this.SelectedMunicipalityUsual.Id,
                PostCode = this.SelectedCityModelUsual == null ? null : this.SelectedCityModelUsual.PostCode,
                UiNumber = this.UsualUin.TrimEnd(),
                Birthdate = this.UsualBirthDate,
                DrivingExperience = this.SelectedDrivingExperienceUsual != null ? this.SelectedDrivingExperienceUsual.Number : 0
            };

            var resut = await this._userService.AddUserToFamilyAndFriends(req, httpClient, jwt);

            return resut;
        }
        private async Task UpdateUserCollection(UserModel user)
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
                DrivingExperience = user.DrivingExperience.HasValue ? user.DrivingExperience.Value : 0,
                BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Parse("01/01/1990"),
                VatNumber = user.VatNumber,
                GuiltTypeId = this.SelectedGuiltType.Id
            };

            this.UserCollection.Add(current);
            await this.ChangeUser(current);
        }
        private async Task GetGuiltTypes()
        {
            var guiltCollection = new ObservableCollection<BaseDataModel>();
            var guiltColl = new List<BaseDataModel>();

            var guiltJson = await AssetsHelper.LoadAssets(GlobalConstants.GuiltTypes);
            var guilts = JsonConvert.DeserializeObject<UserGuiltResultModel>(guiltJson);
            guiltColl = guilts.Guilts;

            this.UserGuiltCollection.Clear();

            foreach (var guilt in guiltColl)
            {
                guiltCollection.Add(guilt);
            }

            this.UserGuiltCollection = guiltCollection;
        }
        private bool CheckEgnAndBirthDateCompatibility()
        {
            ulong uiNumber = 0;

            bool parsed = ulong.TryParse(this.UsualUin.TrimEnd(), out uiNumber);

            if (!parsed)
            {
                return false;
            }

            bool isValid = Helpers.ValidateEgnAndBirthDate(this.UsualBirthDate, uiNumber);

            return isValid;
        }
        public void PopulateBirthDate()
        {
            if (this.SelectedNationalityUsual != null && this.SelectedNationalityUsual.Id == GlobalConstants.BgCountryCode)
            {
                ulong uiNumber = 0;

                bool parsed = ulong.TryParse(this.UsualUin.TrimEnd(), out uiNumber);

                if (parsed)
                {
                    if (Helpers.EgnValidation(uiNumber))
                    {
                        var date = Helpers.GetDateFromEgn(uiNumber);

                        this.UsualBirthDate = date;
                    }
                }
            }               
        }

        #endregion
    }
}
