using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Result;
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
    public partial class TravelInsuranceSummaryViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private ObservableCollection<InsuredUserModel> _insuredUsers;
        private string _startDate;
        private string _endDate;
        private string _price;

        private InsuranceOfferModel _selectedOffer;
        private InsuranceUserInfoModel _userInfo;
        private List<InsuredUserDataModel> _insuredUsersInfo;
        #endregion
        public TravelInsuranceSummaryViewModel()
        {
            this.InsuredUsers = new ObservableCollection<InsuredUserModel>();
            
        }

        #region Props
        public ObservableCollection<InsuredUserModel> InsuredUsers
        {
            get { return _insuredUsers; }
            set { SetProperty(ref _insuredUsers, value); }
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

        public string Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
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
                        {"selectedOffer", this._selectedOffer},
                        {"userInfo", this._userInfo},
                        {"insuredUsers", this._insuredUsersInfo},
                    };

                await Navigation.PushAsync<TravelInsuranceDocumentsPage>(parameters);
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
            this._userInfo = query.FirstOrDefault(x => x.Key == "userInfo").Value as InsuranceUserInfoModel;
            this._insuredUsersInfo = query.FirstOrDefault(x => x.Key == "insuredUsers").Value as List<InsuredUserDataModel>;

            this.StartDate = this._selectedOffer.StartDateFormatted;
            this.EndDate = this._selectedOffer.EndDateFormatted;
            this.Price = this._selectedOffer.PriceFormatted;

            Task.Run(async () => { await PopulateInsuredUsers(); }).Wait();
        }

        private async Task PopulateInsuredUsers()
        {
            var collection = new ObservableCollection<InsuredUserModel>();

            foreach (var user in this._insuredUsersInfo)
            {
                var current = new InsuredUserModel
                {
                    FirstName = user.FirstName + " " + user.LastName,
                };

                collection.Add(current);
            }

            this.InsuredUsers.Clear();
            this.InsuredUsers = collection;
        }

        #endregion
    }
}
