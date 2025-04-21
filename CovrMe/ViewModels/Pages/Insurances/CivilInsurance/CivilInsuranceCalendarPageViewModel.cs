using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using System.Globalization;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceCalendarPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private int _installment = 1;

        private DateTime _startDate;
        private DateTime _endDate;

        private string _startDateFormatted;
        private string _endDateFormatted;
        private string _periodFormatted;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        //insurance
        private InsuranceOfferModel _selectedOffer;

        //services
        private IInsuranceService _insuranceService;

        #endregion
        public Dictionary<int, List<CalendarModel>> Dict { get; set; } = new Dictionary<int, List<CalendarModel>>();
        public CivilInsuranceCalendarPageViewModel()
        {
            SetStartEndDate();
        }

        #region Props

        public InsuranceOfferModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }
        public string StartDateFormatted
        {
            get { return _startDateFormatted; }
            set { SetProperty(ref _startDateFormatted, value); }
        }
        public string PeriodFormatted
        {
            get { return _periodFormatted; }
            set { SetProperty(ref _periodFormatted, value); }
        }
        public string EndDateFormatted
        {
            get { return _endDateFormatted; }
            set { SetProperty(ref _endDateFormatted, value); }
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
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                    };
                await Navigation.PushAsync<CivilInsuranceSummaryPage>(parameters);
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

        #region Private Methods
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;

            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
        }

        public void SetStartEndDate()
        {
            this._startDate = DateTime.Now.AddDays(1);
            this.StartDateFormatted = this._startDate.ToString("dd.MM.yyyy");

            this._endDate = this._startDate.AddDays(45);
            this.EndDateFormatted = this._endDate.ToString("dd.MM.yyyy");

            this.PeriodFormatted = $"{StartDateFormatted} - {EndDateFormatted}";
        }
        #endregion
    }
}
