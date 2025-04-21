using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Views.Pages.Insurances.MyThings;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.MyThings
{
    public partial class MyThingsInsuranceCalendarPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private DateTime _startDate;
        private DateTime _endDate;

        private string _startDateFormatted;
        private string _endDateFormatted;
        private string _periodFormatted;
        private MyThingsInsuranceOfferModel _myThingsInfo;

        #endregion

        public MyThingsInsuranceCalendarPageViewModel()
        {
            SetStartEndDate();
        }
        #region Props
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
                        {"myThingsInfo", this._myThingsInfo}
                    };

                await Navigation.PushAsync<MyThingsInsuranceOffersPage>(parameters);
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

            this._myThingsInfo = query.FirstOrDefault(x => x.Key == "myThingsInfo").Value as MyThingsInsuranceOfferModel;
        }

        public void SetStartEndDate()
        {
            var startDate = DateTime.Now.AddDays(1);
            this._startDate = startDate;
            this.StartDateFormatted = this._startDate.ToString("dd.MM.yyyy");

            this._endDate = this._startDate.AddDays(90);
            this.EndDateFormatted = this._endDate.ToString("dd.MM.yyyy");

            this.PeriodFormatted = $"{StartDateFormatted} - {EndDateFormatted}";
        }
        #endregion
    }
}
