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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceOwnerViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private InsuranceVehicleInfo _vehicleInfo;
        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;
        private LocationDataModel _selectedRegion;
        private LocationDataModel _selectedMunicipality;
        private CityDataModel _selectedCityModel;
        private LocationDataModel _selectedNationality;
        private DrivingExperiencePickerModel _selectedDrivingExperience;
        private UserFamilyAndFriendsPicker _selectedUser;
        private BaseDataModel _selectedGuiltType;
        private string _firstName;
        private string _surName;
        private string _lastName;
        private string _uin;
        private string _address;
        private string _companyName;
        private string _vatNumber;
        private DateTime _birthDate;
        private bool _save;

        private bool _isError = false;
        private bool _firstNameError;
        private bool _lastNameError;
        private bool _surNameError;
        private bool _vatNumberError;
        private bool _uiNumberError;
        private bool _companyNameError;
        private bool _addressError;
        private bool _birthDateError;

        private bool _isUpdatingCollection = false;
        private bool _isUpdatingOnLoading = false;
        private RegCertificateResultModel _regCertificateModel;

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

        public CivilInsuranceOwnerViewModel(ILocationService locationService, IUserService userService)
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

        public RegCertificateResultModel RegCertificateModel
        {
            get { return _regCertificateModel; }
            set { SetProperty(ref _regCertificateModel, value); }
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

        public BaseDataModel SelectedGuiltType
        {
            get { return _selectedGuiltType; }
            set { SetProperty(ref _selectedGuiltType, value); }
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

        public DrivingExperiencePickerModel SelectedDrivingExperience
        {
            get { return _selectedDrivingExperience; }
            set { SetProperty(ref _selectedDrivingExperience, value); }
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

        public string CompanyName
        {
            get { return this._companyName; }
            set { SetProperty(ref this._companyName, value); }
        }

        public string Uin
        {
            get { return this._uin; }
            set { SetProperty(ref this._uin, value); }
        }

        public string Address
        {
            get { return this._address; }
            set { SetProperty(ref this._address, value); }
        }
        public string FirstName
        {
            get { return this._firstName; }
            set { SetProperty(ref this._firstName, value); }
        }

        public string LastName
        {
            get { return this._lastName; }
            set { SetProperty(ref this._lastName, value); }
        }
        public string SurName
        {
            get { return this._surName; }
            set { SetProperty(ref this._surName, value); }
        }

        public string VatNumber
        {
            get { return this._vatNumber; }
            set { SetProperty(ref this._vatNumber, value); }
        }

        public bool Save
        {
            get { return this._save; }
            set { SetProperty(ref this._save, value); }
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

        public bool FirstNameError
        {
            get { return _firstNameError; }
            set
            {
                if (_firstNameError != value)
                {
                    _firstNameError = value;
                    OnPropertyChanged(nameof(FirstNameError));
                }
            }
        }

        public bool LastNameError
        {
            get { return _lastNameError; }
            set
            {
                if (_lastNameError != value)
                {
                    _lastNameError = value;
                    OnPropertyChanged(nameof(LastNameError));
                }
            }
        }

        public bool SurNameError
        {
            get { return _surNameError; }
            set
            {
                if (_surNameError != value)
                {
                    _surNameError = value;
                    OnPropertyChanged(nameof(SurNameError));
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

        public bool UiNumberError
        {
            get { return _uiNumberError; }
            set
            {
                if (_uiNumberError != value)
                {
                    _uiNumberError = value;
                    OnPropertyChanged(nameof(UiNumberError));
                }
            }
        }

        public bool VatNumberError
        {
            get { return _vatNumberError; }
            set
            {
                if (_vatNumberError != value)
                {
                    _vatNumberError = value;
                    OnPropertyChanged(nameof(VatNumberError));
                }
            }
        }

        public bool CompanyNameError
        {
            get { return _companyNameError; }
            set
            {
                if (_companyNameError != value)
                {
                    _companyNameError = value;
                    OnPropertyChanged(nameof(CompanyNameError));
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

                if (this.SelectedNationality == null)
                {
                    throw new Exception(MessageConstants.NationalityNotSelected);
                }

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

                if (this.SelectedDrivingExperience == null)
                {
                    throw new Exception(MessageConstants.DrivingExpNotSelected);
                }

                if (this.SelectedGuiltType == null)
                {
                    throw new Exception(MessageConstants.GuiltNotSelected);
                }

                if (this.Save)
                {
                    var saveResult = new UserModel(); ;

                    if (this.SelectedUser == null || this.SelectedUser.Id == GlobalConstants.New)
                    {
                        saveResult = await this.AddUser();
                    }
                    else if (this.SelectedUser.Id == App.UserId)
                    {
                        saveResult = await this.EditUserInfo();
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
                    FirstName = this.FirstName.TrimEnd(),
                    LastName = this.LastName.TrimEnd(),
                    SurName = this.SurName.TrimEnd(),
                    Uin = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd(),
                    VatNumber = string.IsNullOrEmpty(this.VatNumber) ? null : this.VatNumber.TrimEnd(),
                    CompanyName = string.IsNullOrEmpty(this.CompanyName) ? this.FirstName.TrimEnd() : this.CompanyName.TrimEnd(),
                    Address = this.Address.TrimEnd(),
                    BirthDateString = this.BirthDate.ToString("dd.MM.yyyy"),
                    RegionCode = this.SelectedRegion.Id,
                    MunicipalityCode = this.SelectedMunicipality.Id,
                    CityCode = this.SelectedCityModel.Id,
                    CountryCode = SelectedNationality.Id,
                    DrivingExperiance = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0,
                    PostalCode = this.SelectedCityModel.PostCode,
                    RegionName = this.SelectedRegion.Name,
                    MunicipalityName = this.SelectedMunicipality.Name,
                    CityName = this.SelectedCityModel.Name,
                    GuiltTypeId = this.SelectedGuiltType.Id
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"ownerUserInfo", userInfo},
                        {"userVehicleInfo", _vehicleInfo},
                    };

                if (RegCertificateModel != null)
                {
                    parameters.Add("ocrRegCertificateModel", RegCertificateModel);
                }

                if (string.IsNullOrEmpty(this.VatNumber))
                {
                    await Navigation.PushAsync<CivilInsuranceOffersPage>(parameters);
                }
                else
                {
                    await Navigation.PushAsync<CivilInsuranceUsualDriverPage>(parameters);
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
                    this.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == currentUser.CountryId);
                    this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == currentUser.RegionId);

                    this.SelectedDrivingExperience = this.DrivingExperienceCollection.FirstOrDefault(x => x.Number == currentUser.DrivingExperience);

                    this.FirstName = currentUser.FirstName;
                    this.SurName = currentUser.SurName;
                    this.LastName = currentUser.LastName;
                    this.VatNumber = currentUser.VatNumber;
                    this.Uin = currentUser.UiNumber;
                    this.Address = currentUser.Address;
                    this.CompanyName = currentUser.CompanyName;

                    if (currentUser.BirthDate.HasValue)
                    {
                        this.BirthDate = currentUser.BirthDate.Value;
                    }
                    else
                    {
                        PopulateBirthDate();
                    }

                    var guiltType = this.UserGuiltCollection.FirstOrDefault(x => x.Id == 1);
                    this.SelectedGuiltType = guiltType;

                    if (this.SelectedRegion != null)
                    {
                        await GetMunicipality(this.SelectedRegion);
                    }

                    this.SelectedMunicipality = this.MunicipalityCollection.FirstOrDefault(x => x.Id == currentUser.MunicipalityId);

                    if (this.SelectedMunicipality != null)
                    {
                        await GetCities(this.SelectedMunicipality);
                    }

                    this.SelectedCityModel = this.CityCollection.FirstOrDefault(x => x.Id == currentUser.CityId);

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
                    FirstName = this.FirstName,
                    SurName = this.SurName,
                    LastName = this.LastName,
                    VatNumber = this.VatNumber,
                    CompanyName = this.CompanyName,
                    UiNumber = this.Uin,
                    Address = this.Address,
                    Municipality = this.SelectedMunicipality != null ? this.SelectedMunicipality.Id : string.Empty,
                    Region = this.SelectedRegion != null ? SelectedRegion.Id : string.Empty,
                    City = this.SelectedCityModel != null ? SelectedCityModel.Id : string.Empty,
                    Nationality = this.SelectedNationality != null ? this.SelectedNationality.Id : string.Empty,
                    BirthDate = this.BirthDate,
                    DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0,
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
                        CompanyName = user.CompanyName,
                        GuiltTypeId = 1,                        
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

                this.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == user.Nationality);
                this.SelectedRegion = this.RegionCollection.FirstOrDefault(x => x.Id == user.Region);

                this.SelectedDrivingExperience = this.DrivingExperienceCollection.FirstOrDefault(x => x.Number == user.DrivingExperience);

                this.FirstName = user.FirstName;
                this.SurName = user.SurName;
                this.LastName = user.LastName;
                this.VatNumber = user.VatNumber;
                this.Uin = user.UiNumber;
                this.Address = user.Address;

                if (user.BirthDate.HasValue)
                {
                    this.BirthDate = user.BirthDate.Value;
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

                this.CompanyName = user.CompanyName;

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

            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this.RegCertificateModel = query.FirstOrDefault(x => x.Key == "ocrRegCertificateModel").Value as RegCertificateResultModel;
            if (RegCertificateModel != null)
            {
                ParseOcrData();
            }
        }

        private async void ParseOcrData()
        {
            this.SelectedUser = new UserFamilyAndFriendsPicker();
            this.SelectedDrivingExperience = new DrivingExperiencePickerModel();
            this.SelectedGuiltType = new BaseDataModel();
            this.BirthDate = DateTime.Now;
            this.SelectedRegion = new LocationDataModel();
            this.SelectedCityModel = new CityDataModel();
            this.SelectedMunicipality = new LocationDataModel();
            this.Uin = String.Empty;
            this.Address = String.Empty;
            this.FirstName = String.Empty;
            this.SurName = String.Empty;
            this.LastName = String.Empty;

            if (RegCertificateModel.Region != null)
            {
                var region = RegionCollection.FirstOrDefault(m => m.Name == RegCertificateModel.Region);
                if (region != null)
                {
                    this.SelectedRegion = region;
                    await GetMunicipality(SelectedRegion);
                }
            }

            if (RegCertificateModel.Municipality != null)
            {
                var municipality = MunicipalityCollection.FirstOrDefault(m => m.Name == RegCertificateModel.Municipality);
                if (municipality != null)
                {
                    this.SelectedMunicipality = municipality;
                    await GetCities(SelectedMunicipality);
                }
            }

            if (RegCertificateModel.City != null)
            {
                var city = CityCollection.FirstOrDefault(m => m.Name == RegCertificateModel.City);
                if (city != null)
                {
                    this.SelectedCityModel = city;
                }
            }

            if (RegCertificateModel.FirstName != null)
            {
                this.FirstName = RegCertificateModel.FirstName;
            }

            if (RegCertificateModel.Surname != null)
            {
                this.SurName = RegCertificateModel.Surname;
            }

            if (RegCertificateModel.LastName != null)
            {
                this.LastName = RegCertificateModel.LastName;
            }

            if (RegCertificateModel.Uin != null)
            {
                this.Uin = RegCertificateModel.Uin;
                if (CheckEgnAndBirthDateCompatibility())
                {
                    PopulateBirthDate();
                }
            }

            if (RegCertificateModel.Address != null)
            {
                this.Address = RegCertificateModel.Address;
            }
        }

        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.FirstName))
            {
                this.FirstNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredFirstName;
            }
            else
            {
                var valid = this.ValidateBulgarian(this.FirstName.TrimEnd());

                if (!valid)
                {
                    this.FirstNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (string.IsNullOrEmpty(this.SurName))
            {
                this.SurNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredSurName;
            }
            else
            {
                var valid = this.ValidateBulgarian(this.SurName.TrimEnd());

                if (!valid)
                {
                    this.SurNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                this.LastNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredLastName;
            }
            else
            {
                var valid = this.ValidateBulgarian(this.LastName.TrimEnd());

                if (!valid)
                {
                    this.LastNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (!string.IsNullOrEmpty(this.CompanyName))
            {
                var valid = this.ValidateBulgarianCompany();

                if (!valid)
                {
                    this.CompanyNameError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.BulgarianError;
                }
            }

            if (string.IsNullOrEmpty(this.CompanyName) && !string.IsNullOrEmpty(this.VatNumber))
            {
                this.CompanyNameError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredCompanyName;
            }
            else if (!string.IsNullOrEmpty(this.CompanyName) && string.IsNullOrEmpty(this.VatNumber))
            {
                this.VatNumberError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredVat;
            }
            else if (!string.IsNullOrEmpty(this.CompanyName) && !string.IsNullOrEmpty(this.VatNumber))
            {
                if (!VatNumberValidation())
                {
                    this.VatNumberError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidVat;
                }
            }

            if (string.IsNullOrEmpty(this.Address))
            {
                this.AddressError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredAddress;
            }
            else
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

            if (string.IsNullOrEmpty(this.Uin))
            {
                this.UiNumberError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredUin;
            }
            else
            {
                if (this.SelectedNationality.Id != GlobalConstants.BgCountryCode)
                {
                    if (!ForeignerNumberValidation())
                    {
                        this.UiNumberError = true;
                        this.IsError = true;
                        res.IsValid = false;
                        res.Message = MessageConstants.InvalidUin;
                    }
                }
                else
                {
                    if (!UiNumberValidation())
                    {
                        this.UiNumberError = true;
                        this.IsError = true;
                        res.IsValid = false;
                        res.Message = MessageConstants.InvalidUin;
                    }
                    else
                    {
                        if (!CheckEgnAndBirthDateCompatibility())
                        {
                            this.UiNumberError = true;
                            this.BirthDateError = true;
                            this.IsError = true;
                            res.IsValid = false;
                            res.Message = MessageConstants.EgnAndDateIncompatible;
                        }
                    }
                }
            }

            return res;
        }

        private bool ValidateBulgarian(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.BulgarianNameRegex))
            {
                return true;
            }

            return false;
        }
        private bool ValidateBulgarianCompany()
        {
            if (Regex.IsMatch(this.CompanyName.TrimEnd(), GlobalConstants.BulgarianCompanyRegex))
            {
                return true;
            }

            return false;
        }
        private bool VatNumberValidation()
        {
            return Helpers.VatValidation(this.VatNumber.TrimEnd());
        }
        private bool ValidateAddress()
        {
            if (Regex.IsMatch(this.Address.TrimEnd(), GlobalConstants.AddressRegex))
            {
                return true;
            }

            return false;
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

            var date = Helpers.GetDateFromEgn(uiNumber);

            return isValid;
        }
        private bool ForeignerNumberValidation()
        {
            bool isValid = Helpers.ValidateForeignerNumber(this.Uin.TrimEnd());

            return isValid;
        }
        private bool CheckEgnAndBirthDateCompatibility()
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
        private async Task<UserModel> EditUserInfo(string userId = null)
        {
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var req = new EditUserInfoInput
            {
                UserId = string.IsNullOrEmpty(userId) ? App.UserId : userId,
                Email = string.IsNullOrEmpty(userId) ? App.Email : null,
                ParentUserId = string.IsNullOrEmpty(userId) ? null : App.UserId,
                FirstName = this.FirstName.TrimEnd(),
                SurName = this.SurName.TrimEnd(),
                LastName = this.LastName.TrimEnd(),
                Address = this.Address.TrimEnd(),
                CityId = this.SelectedCityModel == null ? null : this.SelectedCityModel.Id,
                CountryId = this.SelectedNationality == null ? null : this.SelectedNationality.Id,
                RegionId = this.SelectedRegion == null ? null : this.SelectedRegion.Id,
                MuniciplalityId = this.SelectedMunicipality == null ? null : this.SelectedMunicipality.Id,
                PostCode = this.SelectedCityModel == null ? null : this.SelectedCityModel.PostCode,
                UiNumber = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd(),
                VatNumber = string.IsNullOrEmpty(this.VatNumber) ? null : this.VatNumber.TrimEnd(),
                BirthDate = this.BirthDate,
                CompanyName = string.IsNullOrEmpty(this.CompanyName) ? null : this.CompanyName.TrimEnd(),
                DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0
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
                FirstName = this.FirstName.TrimEnd(),
                SurName = this.SurName.TrimEnd(),
                LastName = this.LastName.TrimEnd(),
                Address = this.Address.TrimEnd(),
                CityId = this.SelectedCityModel == null ? null : this.SelectedCityModel.Id,
                CountryId = this.SelectedNationality == null ? null : this.SelectedNationality.Id,
                RegionId = this.SelectedRegion == null ? null : this.SelectedRegion.Id,
                MuniciplalityId = this.SelectedMunicipality == null ? null : this.SelectedMunicipality.Id,
                PostCode = this.SelectedCityModel == null ? null : this.SelectedCityModel.PostCode,
                UiNumber = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd(),
                VatNumber = string.IsNullOrEmpty(this.VatNumber) ? null : this.VatNumber.TrimEnd(),
                Birthdate = this.BirthDate,
                CompanyName = string.IsNullOrEmpty(this.CompanyName) ? null : this.CompanyName.TrimEnd(),
                DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0
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
                CompanyName = user.CompanyName,
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

        #endregion
    }
}
