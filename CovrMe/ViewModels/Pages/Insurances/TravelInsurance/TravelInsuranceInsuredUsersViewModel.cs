using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;
using System.Collections.ObjectModel;


namespace CovrMe.ViewModels.Pages.Insurances.TravelInsurance
{
    public partial class TravelInsuranceInsuredUsersViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private int _hightPackageIOS = 10;
        private string _age;
        private TravelInsuranceOfferModel _travelInfo;
        //collections
        private ObservableCollection<InsuredUsersAgeTemplateModel> _insuredUsersAges;

        #endregion

        public TravelInsuranceInsuredUsersViewModel()
        {
            this._insuredUsersAges = new ObservableCollection<InsuredUsersAgeTemplateModel>();

            HeightPackageIOS = 10;

            SetFirstInsuredUserAsDefault();
        }

        #region Props
        public int HeightPackageIOS
        {
            get { return _hightPackageIOS; }
            set { SetProperty(ref _hightPackageIOS, value); }
        }
        public string Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }
        #endregion

        #region Collections

        public ObservableCollection<InsuredUsersAgeTemplateModel> InsuredUsersAgesList
        {
            get => _insuredUsersAges;
            set => SetProperty(ref _insuredUsersAges, value);
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

                List<InsuredUsersAgeTemplateModel> userAgeList = this.InsuredUsersAgesList.ToList();

                if (userAgeList.Count == 0)
                {
                    throw new Exception(MessageConstants.NoInsuredUsersError);
                }

                if (userAgeList.Any(x => string.IsNullOrEmpty(x.Age)))
                {
                    throw new Exception(MessageConstants.AgeIsZeroError);
                }

                if (userAgeList.Any(x => x.Age != null && int.Parse(x.Age) > GlobalConstants.InappropriateAge))
                {
                    throw new Exception(string.Format(MessageConstants.InappropriatedAgeError, GlobalConstants.InappropriateAge));
                }

                var parameters = new Dictionary<string, object>
                    {
                        {"travelInfo", this._travelInfo},
                        {"userAgeList", userAgeList}
                    };


                await Navigation.PushAsync<TravelInsuranceCalendarPage>(parameters);
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

            this._travelInfo = query.FirstOrDefault(x => x.Key == "travelInfo").Value as TravelInsuranceOfferModel;
        }

        private async void SetFirstInsuredUserAsDefault()
        {
            var newInsuredUser = new InsuredUsersAgeTemplateModel
            {
                Age = "18",
                Index = 0,
                Number = 1
            };

            this.InsuredUsersAgesList.Add(newInsuredUser);
        }

        #endregion
    }
}
