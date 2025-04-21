using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Insurances.Request.Casco;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Request;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.Casco;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CovrMe.ViewModels.Pages.Insurances.Casco
{
    public partial class CascoInsuranceOwnerViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;
        private InsuranceVehicleInfo _vehicleInfo;
        private DrivingExperiencePickerModel _selectedDrivingExperience;
        private UserFamilyAndFriendsPicker _selectedUser;
        private LocationDataModel _selectedNationality;
        private string _firstName;
        private string _surName;
        private string _lastName;
        private string _phone;
        private int _age;
        private string phoneNumberCode;
        private string _email;
        private bool _save;
        private string _uin;
        private DateTime _birthDate;
        private RegCertificateResultModel _regCertificateModel;

        private bool _isError = false;
        private bool _emailError;
        private bool _phoneError;
        private bool _ageError;
        private bool _firstNameError;
        private bool _lastNameError;
        private bool _surNameError;
        private bool _uiNumberError;
        private bool _birthDateError;

        private bool _isUpdatingCollection = false;

        //services
        private IUserService _userService;
        private IInsuranceService _insuranceService;

        //collections
        private ObservableCollection<DrivingExperiencePickerModel> _drivingExperienceCollection;
        private ObservableCollection<UserFamilyAndFriendsPicker> _userCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;

        #endregion

        public CascoInsuranceOwnerViewModel(IUserService userService, IInsuranceService insuranceService)
        {
            this._userService = userService;
            this._insuranceService = insuranceService;

            this.DrivingExperienceCollection = new ObservableCollection<DrivingExperiencePickerModel>();
            this.UserCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();

            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1900");
            this.BirthDate = DateTime.Parse("01/01/1990");

            Task.Run(async () => { await GetCountries(); }).Wait();
            Task.Run(async () => { await PopulateDrivingExperienceCollection(); }).Wait();
            Task.Run(async () => { await GetCurrentUserInfo(); }).Wait();
            Task.Run(async () => { await GetUserFamilyAndFriends(); }).Wait();
        }

        #region Collections

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
        public ObservableCollection<LocationDataModel> CountryCollection
        {
            get => _countryCollection;
            set => SetProperty(ref _countryCollection, value);
        }

        #endregion

        #region Props
        public RegCertificateResultModel RegCertificateModel
        {
            get { return _regCertificateModel; }
            set { SetProperty(ref _regCertificateModel, value); }
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
        public DrivingExperiencePickerModel SelectedDrivingExperience
        {
            get { return _selectedDrivingExperience; }
            set { SetProperty(ref _selectedDrivingExperience, value); }
        }
        public LocationDataModel SelectedNationality
        {
            get { return _selectedNationality; }
            set { SetProperty(ref _selectedNationality, value); }
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
        public string Email
        {
            get { return this._email; }
            set { SetProperty(ref this._email, value); }
        }
        public DateTime BirthDate
        {
            get { return this._birthDate; }
            set { SetProperty(ref this._birthDate, value); }
        }
        public string Phone
        {
            get { return this._phone; }
            set { SetProperty(ref this._phone, value); }
        }
        public string Uin
        {
            get { return this._uin; }
            set { SetProperty(ref this._uin, value); }
        }
        public int Age
        {
            get { return this._age; }
            set { SetProperty(ref this._age, value); }
        }
        public string PhoneNumberCode
        {
            get { return this.phoneNumberCode; }
            set { SetProperty(ref this.phoneNumberCode, value); }
        }
        public bool Save
        {
            get { return this._save; }
            set { SetProperty(ref this._save, value); }
        }
        public bool EmailError
        {
            get { return _emailError; }
            set
            {
                if (_emailError != value)
                {
                    _emailError = value;
                    OnPropertyChanged(nameof(EmailError));
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

                if (string.IsNullOrEmpty(this.PhoneNumberCode))
                {
                    this.PhoneNumberCode = "+359";
                }
                var phone = this.PhoneNumberCode + this.Phone.TrimEnd();
                this.Phone = phone;

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

                var userInfo = new InsuranceUserEmailModelInput
                {
                    UserId = this.SelectedUser.Id == GlobalConstants.New ? string.Empty : this.SelectedUser.Id,
                    FirstName = this.FirstName.TrimEnd(),
                    LastName = this.LastName.TrimEnd(),
                    SurName = this.SurName.TrimEnd(),
                    DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0,
                    Email = this.Email == null ? this.SelectedUser.Email : this.Email,
                    Phone = this.Phone == null ? this.SelectedUser.PhoneNumber : this.Phone,
                    Uin = this.Uin.TrimEnd(),
                };

                var vehicleInfo = new InsuranceVehicleEmailModelInput
                {
                    VehicleBrand = _vehicleInfo.VehicleBrand,
                    VehicleModel = _vehicleInfo.VehicleModel,
                    VehicleUsage = _vehicleInfo.VehicleUsage,
                    VehicleType = _vehicleInfo.VehicleType,
                    EngineType = _vehicleInfo.EngineType,
                    EngineVolume = _vehicleInfo.EngineVolume,
                    EngineTypeText = _vehicleInfo.EngineTypeText,
                    FirstRegistrationDate = _vehicleInfo.FirstRegistrationDate,
                    BatteryCapacity = _vehicleInfo.BatteryCapacity,
                    VehicleKilowatts = _vehicleInfo.VehicleKilowatts
                };

                var sendEmailInput = new CascoRequestEmailInput
                {
                    UserInfo = userInfo,
                    VehicleInfo = vehicleInfo
                };
                HttpClient client = new HttpClient();
                var result = await _insuranceService.SendEmailCasco(sendEmailInput, client, App.JwtToken);

                if (result.Code == (int)GeneralStatusEnum.Success)
                {
                    await Navigation.PushAsync<CascoInsuranceThankYouPage>();
                }
                else
                {
                    await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.GeneralError, App.MESSAGE_OK);
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

                    this.FirstName = currentUser.FirstName;
                    this.SurName = currentUser.SurName;
                    this.LastName = currentUser.LastName;
                    this.Phone = currentUser.PhoneNumber;
                    if (!string.IsNullOrEmpty(this.Phone))
                    {
                        if (this.Phone.StartsWith("+359"))
                        {
                            this.Phone = this.Phone.Substring(4);
                        }
                    }

                    this.Email = currentUser.Email;                    
                    this.Uin = currentUser.UiNumber;                  

                    if (currentUser.BirthDate.HasValue)
                    {
                        this.BirthDate = currentUser.BirthDate.Value;
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
                    
                    this.SelectedDrivingExperience = this.DrivingExperienceCollection.FirstOrDefault(x => x.Number == currentUser.DrivingExperience);
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
                    Email = this.Email,
                    PhoneNumber = this.Phone,
                    BirthDate = this.BirthDate,
                    DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0,
                    UiNumber = this.Uin,
                    Nationality = this.SelectedNationality != null ? this.SelectedNationality.Id : string.Empty,
                };

                this.SelectedUser = itsMe;

                userCollection.Add(itsMe);
                userCollection.Add(newUser);

                if (users != null && users.FamilyAndFriends.Any())
                {
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
                            Names = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)) ? user.FirstName + " " + user.LastName : ((!string.IsNullOrEmpty(user.LatinFirstName) && !string.IsNullOrEmpty(user.LatinLastName)) ? user.LatinFirstName + " " + user.LatinLastName : GlobalConstants.NoName),
                            DrivingExperience = user.DrivingExperience.HasValue ? user.DrivingExperience.Value : 0,
                            CompanyName = user.CompanyName,
                            GuiltTypeId = 1
                        };

                        userCollection.Add(current);
                    }
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

                this.FirstName = user.FirstName;
                this.SurName = user.SurName;
                this.LastName = user.LastName;
                this.Email = user.Email;
                this.Phone = user.PhoneNumber;
                if (!string.IsNullOrEmpty(this.Phone))
                {
                    if (this.Phone.StartsWith("+359"))
                    {
                        this.Phone = this.Phone.Substring(4);
                    }
                }

                this.SelectedDrivingExperience = this.DrivingExperienceCollection.FirstOrDefault(x => x.Number == user.DrivingExperience);
                this.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == user.Nationality);
                this.Uin = user.UiNumber;

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

                this.Age = this.CalculateUserAge();
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
            RegCertificateModel = query.FirstOrDefault(x => x.Key == "ocrRegCertificateModel").Value as RegCertificateResultModel;

            if (RegCertificateModel != null)
            {
                ParseOcrData();
            }
        }

        private void ParseOcrData()
        {
            SelectedDrivingExperience = new DrivingExperiencePickerModel();
            SelectedUser = new UserFamilyAndFriendsPicker();
            FirstName = string.Empty;
            SurName = string.Empty;
            LastName = string.Empty;
            Uin = string.Empty;
            Phone = String.Empty;
            Email = String.Empty;
            Age = 0;

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

            if (string.IsNullOrEmpty(this.Email))
            {
                this.EmailError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredEmail;
            }
            else
            {
                if (!ValidateEmail())
                {
                    this.EmailError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidEmail;
                }
            }

            if (!this.PhoneValidation())
            {
                this.PhoneError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.InvalidPhone;
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
        private bool ValidateEmail()
        {
            if (Regex.IsMatch(this.Email.TrimEnd(), GlobalConstants.EmailRegex))
            {
                return true;
            }

            return false;
        }
        private bool ValidateAge()
        {
            if (this.Age > 18)
            {
                return true;
            }

            return false;
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
        private async Task<UserModel> EditUserInfo(string userId = null)
        {
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var req = new EditUserInfoInput
            {
                UserId = string.IsNullOrEmpty(userId) ? App.UserId : userId,
                Phone = string.IsNullOrEmpty(this.Phone) ? null : this.Phone.TrimEnd(),
                Email = string.IsNullOrEmpty(this.Email) ? App.Email : this.Email.TrimEnd(),
                ParentUserId = string.IsNullOrEmpty(userId) ? null : App.UserId,
                FirstName = this.FirstName.TrimEnd(),
                SurName = this.SurName.TrimEnd(),
                LastName = this.LastName.TrimEnd(),
                DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0,
                CountryId = this.SelectedNationality == null ? null : this.SelectedNationality.Id,
                UiNumber = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd(),
                BirthDate = this.BirthDate
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
                DrivingExperience = this.SelectedDrivingExperience != null ? this.SelectedDrivingExperience.Number : 0,
                Email = this.Email.TrimEnd(),
                Phone = this.Phone.TrimEnd(),
                Birthdate = this.BirthDate,
                CountryId = this.SelectedNationality == null ? null : this.SelectedNationality.Id,
                UiNumber = string.IsNullOrEmpty(this.Uin) ? null : this.Uin.TrimEnd()
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
                Names = user.FirstName + " " + user.LastName,
                DrivingExperience = user.DrivingExperience.HasValue ? user.DrivingExperience.Value : 0,
                BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Parse("01/01/1990"),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UiNumber = user.UiNumber
            };

            this.UserCollection.Add(current);
            await this.ChangeUser(current);
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
        private int CalculateUserAge()
        {
            var today = DateTime.Today;

            var age = today.Year - this.BirthDate.Year;
            if (this.BirthDate.Date > today.AddYears(-age)) age--;

            return age;
        }
        #endregion
    }
}
