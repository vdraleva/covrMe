using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Result.Pickers;
using CovrMe.Services.Contracts;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.TravelInsurance
{
    public partial class TravelInsuranceUserInfoViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private InsuranceOfferModel _selectedOffer;
        private InsuranceUserInfoModel _userInfo;

        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;

        private string _firstName;
        private string _lastName;
        private string _uin;
        private DateTime _birthDate;

        private bool _isError = false;
        private bool _firstNameError;
        private bool _lastNameError;
        private bool _uiNumberError;

        private bool _save;
        private bool _isUpdatingCollection = false;

        private ObservableCollection<UserFamilyAndFriendsPicker> _userCollection;

        private IUserService _userService;
        #endregion

        public TravelInsuranceUserInfoViewModel(IUserService userService)
        {
            this._userService = userService;
            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1900");

            this.UserCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();

            Task.Run(async () => { await GetCurrentUserInfo(); }).Wait();
        }

        #region Collections

        public ObservableCollection<UserFamilyAndFriendsPicker> UserCollection
        {
            get => _userCollection;
            set => SetProperty(ref _userCollection, value);
        }

        #endregion

        #region Props

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

        public string Uin
        {
            get { return this._uin; }
            set { SetProperty(ref this._uin, value); }
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


                await Navigation.PushAsync<TravelInsuranceSummaryPage>();
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

                    this.FirstName = currentUser.LatinFirstName;
                    this.LastName = currentUser.LatinLastName;
                    this.Uin = currentUser.UiNumber;
                    this.BirthDate = currentUser.BirthDate.HasValue ? currentUser.BirthDate.Value : DateTime.Now;
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
            this._userInfo = query.FirstOrDefault(x => x.Key == "userAgeList").Value as InsuranceUserInfoModel;
        }

        #endregion
    }
}
