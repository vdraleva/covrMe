using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.TravelInsurance
{
    public partial class TravelInsuredUsersPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;
        private UserFamilyAndFriendsPicker _selectedUser;
        private double _displayWidth;

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private InsuranceOfferModel _selectedOffer;
        private InsuranceUserInfoModel _userInfo;
        private UserModel _currentUser;
        private ObservableCollection<InsuredUserDataModel> _insuredUsers;

        private ObservableCollection<UserFamilyAndFriendsPicker> _userCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;

        //services
        private IUserService _userService;

        #endregion
        public TravelInsuredUsersPageViewModel(IUserService userService)
        {
            this._userService = userService;
            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1900");
            this._currentUser = new UserModel();
            this.SetScreenWidth();
            this.UserCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();

            Task.Run(async () => { await GetCountries(); }).Wait();
            Task.Run(async () => { await GetCurrentUserInfo(); }).Wait();
            Task.Run(async () => { await GetUserFamilyAndFriends(); }).Wait();
        }

        #region Collections

        public ObservableCollection<UserFamilyAndFriendsPicker> UserCollection
        {
            get => _userCollection;
            set => SetProperty(ref _userCollection, value);
        }

        public ObservableCollection<InsuredUserDataModel> InsuredUsers
        {
            get => _insuredUsers;
            set => SetProperty(ref _insuredUsers, value);
        }

        public ObservableCollection<LocationDataModel> CountryCollection
        {
            get => _countryCollection;
            set => SetProperty(ref _countryCollection, value);
        }

        #endregion

        #region Props

        public double DisplayWidth
        {
            get { return this._displayWidth; }
            set { SetProperty(ref this._displayWidth, value); }
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

        #endregion

        #region Commands

        [RelayCommand]
        private async Task CardToggle(InsuredUserDataModel input)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var currentOpen = this.InsuredUsers.FirstOrDefault(x => x.IsOpen == true);

                if (currentOpen != null)
                {
                    currentOpen = await CheckIfUserInfoIsFilled(currentOpen);

                    if (currentOpen.IsFilled)
                    {
                        currentOpen = this.ValidateInput(currentOpen);
                        if (!currentOpen.IsValid)
                        {
                            throw new Exception(currentOpen.ValidationMessage);
                        }
                    }

                    currentOpen.IsOpen = false;
                }

                var toOpen = this.InsuredUsers[input.Index];
                toOpen.IsOpen = true;
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
        private async Task Continue()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;
                ShowLoading();

                bool isValid = this.AreValidUsers();

                if (!isValid)
                {
                    var invalidUser = this.InsuredUsers.FirstOrDefault(x => x.IsValid == false);

                    if (invalidUser != null)
                    {
                        this.InsuredUsers = this.InsuredUsers.Select(x => { x.IsOpen = false; return x; }).ToObservableCollection();
                        invalidUser = this.ValidateInput(invalidUser);
                        invalidUser.IsOpen = true;

                        throw new Exception(invalidUser.ValidationMessage);
                    }
                }

                if (!CheckForDuplicatedEgn())
                {
                    throw new Exception(MessageConstants.DuplicatedEgnError);
                }

                Task.Run(async () => { await this.SaveUsers(); });

                var insuredUsers = this.InsuredUsers.ToList();

                var parameters = new Dictionary<string, object>
                    {
                        {"insuredUsers", insuredUsers},
                        {"selectedOffer", this._selectedOffer},
                        {"userInfo", this._userInfo},
                    };

                await Navigation.PushAsync<TravelInsuranceSummaryPage>(parameters);
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

        private async Task GetUserFamilyAndFriends()
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
                    BirthDate = DateTime.Parse("01/01/1990")
                };

                var itsMe = new UserFamilyAndFriendsPicker
                {
                    Id = App.UserId,
                    Names = GlobalConstants.ItsMe,
                    FirstName = this._currentUser.LatinFirstName,
                    LastName = this._currentUser.LatinLastName,
                    UiNumber = this._currentUser.UiNumber,
                    Nationality = this._currentUser.CountryId,
                };

                if (itsMe.BirthDate.HasValue)
                {
                    itsMe.BirthDate = this._currentUser.BirthDate.Value;
                }
                else
                {
                    if (this._currentUser.CountryId == GlobalConstants.BgCountryCode)
                    {
                        ulong uiNumber = 0;

                        bool parsed = ulong.TryParse(itsMe.UiNumber, out uiNumber);

                        if (parsed)
                        {
                            var date = Helpers.GetDateFromEgn(uiNumber);

                            itsMe.BirthDate = date;
                        }
                    }
                    else
                    {
                        itsMe.BirthDate = DateTime.Parse("01/01/1990");
                    }
                        
                }

                userCollection.Add(itsMe);
                userCollection.Add(newUser);


                foreach (var user in users.FamilyAndFriends)
                {
                    var current = new UserFamilyAndFriendsPicker
                    {
                        Id = user.Id,
                        FirstName = user.LatinFirstName,
                        LastName = user.LatinLastName,
                        UiNumber = user.UiNumber,
                        BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Parse("01/01/1990"),
                        Names = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)) ? user.FirstName + " " + user.LastName : ((!string.IsNullOrEmpty(user.LatinFirstName) && !string.IsNullOrEmpty(user.LatinLastName)) ? user.LatinFirstName + " " + user.LatinLastName : GlobalConstants.NoName),
                        Nationality = user.CountryId
                    };

                    userCollection.Add(current);
                }
                this.UserCollection.Clear();
                this.UserCollection = userCollection;
            }
        }
        private async Task GetCurrentUserInfo()
        {
            if (!string.IsNullOrEmpty(App.UserId))
            {
                var httpClient = HttpClientFactory.Create();
                var userId = App.UserId;
                var jwt = App.JwtToken;

                var currentUser = await this._userService.GetUserById(userId, httpClient, jwt);

                if (currentUser != null)
                {
                    this._currentUser = currentUser;
                }
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;
            this._userInfo = query.FirstOrDefault(x => x.Key == "userInfo").Value as InsuranceUserInfoModel;

            this.InsuredUsers = new InsuredUserDataModel[this._usersAge.Count].ToObservableCollection();

            this.SetInsuredUsersNumberIndex();
            this.PopulateInsurerInfoAsInsured();
        }
        private void SetInsuredUsersNumberIndex()
        {
            for (int i = 0; i < this.InsuredUsers.Count; i++)
            {
                this.InsuredUsers[i] = new InsuredUserDataModel();
                this.InsuredUsers[i].Number = i + 1;
                this.InsuredUsers[i].Index = i;
                this.InsuredUsers[i].TextColor = GlobalConstants.EmptyColor;
                this.InsuredUsers[i].FamilyAndFriendsCollection = this.UserCollection;
                this.InsuredUsers[i].CountryCollection = this.CountryCollection;
            }
        }
        public async Task ChangeUser(UserFamilyAndFriendsPicker user, int index)
        {
            try
            {
                ShowLoading();

                var current = this.InsuredUsers[index];
                current.FirstName = user.FirstName;
                current.LastName = user.LastName;
                current.Uin = user.UiNumber;
                current.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == user.Nationality);

                if (user.BirthDate.HasValue)
                {
                    current.BirthDate = user.BirthDate.Value;
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.UiNumber) &&
                        current.SelectedNationality != null && current.SelectedNationality.Id == GlobalConstants.BgCountryCode)
                    {
                        ulong uiNumber = 0;

                        bool parsed = ulong.TryParse(current.Uin.TrimEnd(), out uiNumber);

                        if (parsed)
                        {
                            var date = Helpers.GetDateFromEgn(uiNumber);

                            current.BirthDate = date;
                        }
                    }
                    else
                    {
                        current.BirthDate = DateTime.Parse("01/01/1990");
                    }
                }
                current.TextColor = GlobalConstants.FilledColor;
            }
            catch (Exception ex)
            {

                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
            }

        }
        private void PopulateInsurerInfoAsInsured()
        {
            if (this._userInfo.IsInsured)
            {
                var insurer = this.InsuredUsers.FirstOrDefault();

                if (insurer != null)
                {
                    insurer.FirstName = this._userInfo.FirstName;
                    insurer.LastName = this._userInfo.LastName;
                    insurer.Uin = string.IsNullOrEmpty(this._userInfo.Uin) ? string.Empty : this._userInfo.Uin;
                    insurer.BirthDate = this._userInfo.BirthDate;
                    insurer.TextColor = GlobalConstants.FilledColor;
                    insurer.IsValid = true;
                    insurer.FamilyAndFriendsCollection = this.UserCollection;
                    insurer.CountryCollection = this.CountryCollection;
                    insurer.SelectedNationality = this.CountryCollection.FirstOrDefault(x => x.Id == this._userInfo.CountryCode);
                }

                var user = this.UserCollection.FirstOrDefault(x => x.Id == this._userInfo.UserId && this._userInfo.UserId != GlobalConstants.New);
                if (user != null)
                {
                    insurer.SelectedUser = user;
                }
            }
        }
        private void SetScreenWidth()
        {
            this.DisplayWidth = (DeviceDisplay.MainDisplayInfo.Width / 2) - 200;
        }
        public async Task EmptySelectedUserFields(int index)
        {
            var current = this.InsuredUsers[index];

            if ((current.SelectedUser != null && current.SelectedUser.Id != GlobalConstants.New) || current.SelectedUser == null)
            {
                current.FirstName = string.Empty;
                current.LastName = string.Empty;
                current.Uin = string.Empty;
                current.BirthDate = DateTime.Parse("01/01/1990");
                current.TextColor = GlobalConstants.EmptyColor;
                current.SelectedUser = this.UserCollection.FirstOrDefault(x => x.Id == GlobalConstants.New);
                current.SelectedNationality = null;
            }
            else
            {
                if (!current.IsFilled)
                {
                    current.FirstName = string.Empty;
                    current.LastName = string.Empty;
                    current.Uin = string.Empty;
                    current.BirthDate = DateTime.Parse("01/01/1990");
                    current.TextColor = GlobalConstants.EmptyColor;
                    current.SelectedUser = this.UserCollection.FirstOrDefault(x => x.Id == GlobalConstants.New);
                    current.SelectedNationality = null;
                }
            }
        }
        public async Task<InsuredUserDataModel> CheckIfUserInfoIsFilled(InsuredUserDataModel user)
        {
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName) &&
                !string.IsNullOrEmpty(user.Uin) && user.BirthDate != DateTime.Parse("01/01/1990") && user.SelectedNationality != null)
            {
                user.TextColor = GlobalConstants.FilledColor;
                user.IsFilled = true;

            }
            else
            {
                user.TextColor = GlobalConstants.EmptyColor;
                user.IsFilled = false;
            }

            return user;
        }
        private InsuredUserDataModel ValidateInput(InsuredUserDataModel user)
        {
            user.IsValid = true;
            if (string.IsNullOrEmpty(user.FirstName))
            {
                user.FirstNameError = true;
                user.IsValid = false;
                user.ValidationMessage = MessageConstants.RequiredFirstName;
            }
            else
            {
                bool valid = this.ValidateLatin(user.FirstName.TrimEnd());

                if (!valid)
                {
                    user.FirstNameError = true;
                    user.IsValid = false;
                    user.ValidationMessage = MessageConstants.LatinnError;

                }
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                user.LastNameError = true;
                user.IsValid = false;
                user.ValidationMessage = MessageConstants.RequiredLastName;
            }
            else
            {
                bool valid = this.ValidateLatin(user.LastName.TrimEnd()); ;


                if (!valid)
                {
                    user.LastNameError = true;
                    user.IsValid = false;
                    user.ValidationMessage = MessageConstants.LatinnError;
                }
            }

            if (user.SelectedNationality == null)
            {
                user.NationalityError = true;
                user.IsValid = false;
                user.ValidationMessage = MessageConstants.NationalityNotSelected;
            }

            if (string.IsNullOrEmpty(user.Uin))
            {
                user.UiNumberError = true;
                user.IsValid = false;
                user.ValidationMessage = MessageConstants.RequiredUin;
            }
            else
            {
                if (user.SelectedNationality.Id != GlobalConstants.BgCountryCode)
                {
                    if (!ForeignerNumberValidation(user.Uin.TrimEnd()))
                    {
                        user.UiNumberError = true;
                        user.IsValid = false;
                        user.ValidationMessage = MessageConstants.InvalidUin;
                    }
                }
                else
                {
                    if (!UiNumberValidation(user.Uin.TrimEnd()))
                    {
                        user.UiNumberError = true;
                        user.IsValid = false;
                        user.ValidationMessage = MessageConstants.InvalidUin;
                    }
                    else
                    {
                        if (!CheckEgnAndBirthDateCompatibility(user.Uin.TrimEnd(), user.BirthDate))
                        {
                            user.UiNumberError = true;
                            user.BirthDateError = true;
                            user.IsValid = false;
                            user.ValidationMessage = MessageConstants.EgnAndDateIncompatible;
                        }
                        else
                        {
                            int userIndex = user.Index;
                            var currentAge = this.CalculateUserAge(user.BirthDate);
                            var userAge = this._usersAge.FirstOrDefault(x => x.Index == userIndex);

                            if (userAge != null)
                            {
                                int ageInput = int.Parse(userAge.Age);

                                if (currentAge != ageInput)
                                {
                                    user.UiNumberError = true;
                                    user.BirthDateError = true;
                                    user.IsValid = false;
                                    user.ValidationMessage = string.Format(MessageConstants.IncorectAgeGroup, user.Number, ageInput, currentAge);
                                }
                            }
                        }
                    }
                }
              
            }

            return user;
        }
        private bool UiNumberValidation(string uin)
        {
            ulong uiNumber = 0;

            bool parsed = ulong.TryParse(uin, out uiNumber);

            if (!parsed)
            {
                return false;
            }

            bool isValid = Helpers.EgnValidation(uiNumber);

            return isValid;
        }
        private bool ForeignerNumberValidation(string uin)
        {
            bool isValid = Helpers.ValidateForeignerNumber(uin);

            return isValid;
        }
        private bool ValidateLatin(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.LatinNameRegex))
            {
                return true;
            }

            return false;
        }
        private bool AreValidUsers()
        {
            bool isValid = true;
            foreach (var user in this.InsuredUsers)
            {
                var current = this.ValidateInput(user);

                if (!current.IsValid)
                {
                    user.IsValid = false;
                    isValid = false;
                }
            }

            return isValid;
        }
        private async Task<int> SaveUsers()
        {
            int result = (int)GeneralStatusEnum.Success;
            var usersToSave = this.InsuredUsers.Where(x => x.Save == true).ToList();

            if (usersToSave.Count > 0)
            {
                var usersToAdd = usersToSave.Where(x => x.SelectedUser == null || x.SelectedUser.Id == GlobalConstants.New).ToList();
                var usersToEdit = usersToSave.Where(x => x.SelectedUser != null && x.SelectedUser.Id != GlobalConstants.New).ToList();

                if (usersToAdd.Count > 0)
                {
                    var usersToAddResult = await this.AddUsers(usersToAdd);
                    result = usersToAddResult;
                }

                if (usersToEdit.Count > 0)
                {
                    var usersToEditResult = await this.EditUsers(usersToEdit);
                    result = usersToEditResult;
                }
            }

            return result;
        }
        private async Task<int> AddUsers(List<InsuredUserDataModel> users)
        {
            var jwt = App.JwtToken;
            var httpclient = HttpClientFactory.Create();

            var input = new AddMultipleUserToFamilyAndFriendsInput();

            foreach (var user in users)
            {
                var current = new AddUserToFamilyAndFriendsInput();

                current.Birthdate = user.BirthDate;
                current.UserId = App.UserId;
                current.UiNumber = user.Uin.TrimEnd();
                current.LatinFirstName = user.FirstName.TrimEnd();
                current.LatinLastName = user.LastName.TrimEnd();
                current.FirstName = user.FirstName.TrimEnd();
                current.LastName = user.LastName.TrimEnd();
                current.CountryId = user.SelectedNationality == null ? null : user.SelectedNationality.Id;

                input.Users.Add(current);
            }

            var result = await this._userService.AddMultipleUserToFamilyAndFriends(input, httpclient, jwt);

            return result;
        }
        private async Task<int> EditUsers(List<InsuredUserDataModel> users)
        {
            var jwt = App.JwtToken;
            var httpclient = HttpClientFactory.Create();

            var input = new EditMultipleFamilyFriendsUserInput();

            foreach (var user in users)
            {
                var current = new EditUserInfoInput();

                current.UiNumber = user.Uin.TrimEnd();
                current.BirthDate = user.BirthDate;
                current.UserId = user.SelectedUser != null ? user.SelectedUser.Id : null;

                current.LatinFirstName = user.FirstName.TrimEnd();
                current.LatinLastName = user.LastName.TrimEnd();
                current.CountryId = user.SelectedNationality == null ? null : user.SelectedNationality.Id;

                input.Users.Add(current);
            }


            var result = await this._userService.EditMultipleUserToFamilyAndFriends(input, httpclient, jwt);

            return result;
        }
        private int CalculateUserAge(DateTime birtDate)
        {
            int age = 0;
            age = DateTime.Now.Year - birtDate.Year;
            if (DateTime.Now.DayOfYear < birtDate.DayOfYear)
            {
                age = age - 1;
            }

            return age;
        }
        private bool CheckEgnAndBirthDateCompatibility(string uin, DateTime birthDate)
        {
            ulong uiNumber = 0;

            bool parsed = ulong.TryParse(uin, out uiNumber);

            if (!parsed)
            {
                return false;
            }

            bool isValid = Helpers.ValidateEgnAndBirthDate(birthDate, uiNumber);

            return isValid;
        }    
        public void PopulateBirthDate(int index)
        {
            var currentUser = this.InsuredUsers.FirstOrDefault(x => x.Index == index);

            if (currentUser != null && currentUser.SelectedNationality != null &&
                currentUser.SelectedNationality.Id == GlobalConstants.BgCountryCode && !string.IsNullOrEmpty(currentUser.Uin))
            {
                ulong uiNumber = 0;

                bool parsed = ulong.TryParse(currentUser.Uin, out uiNumber);

                if (parsed)
                {
                    var date = Helpers.GetDateFromEgn(uiNumber);

                    currentUser.BirthDate = date;
                }
            }
        }
        private bool CheckForDuplicatedEgn()
        {
            bool isValid = true;
            var duplicatedEgn = this.InsuredUsers.GroupBy(x => x.Uin)
                                               .Where(x => x.Count() > 1)
                                               .Select(x => x.Key)
                                               .ToList();

            if (duplicatedEgn.Any())
            {
                isValid = false;
            }

            return isValid;
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

        #endregion

    }
}
