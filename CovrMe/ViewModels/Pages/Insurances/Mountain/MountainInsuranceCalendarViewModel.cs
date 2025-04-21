using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.MountainInsurance;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.Mountain;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.Mountain
{
    public partial class MountainInsuranceCalendarViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private MountainInsuranceOfferModel _mountainInfo;

        private DateTime allowedStartDate;
        private DateTime allowedEndDate;

        private string _startDateFormatted;
        private string _endDateFormatted;
        private string _periodFormatted;
        #endregion
        public MountainInsuranceCalendarViewModel()
        {
            SetStartEndDate();
        }

        #region Collections

        #endregion

        #region Props
        public string StartDateFormatted
        {
            get { return _startDateFormatted; }
            set { SetProperty(ref _startDateFormatted, value); }
        }

        public string EndDateFormatted
        {
            get { return _endDateFormatted; }
            set { SetProperty(ref _endDateFormatted, value); }
        }
        public string PeriodFormatted
        {
            get { return _periodFormatted; }
            set { SetProperty(ref _periodFormatted, value); }
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
                        {"userAgeList", _usersAge},
                        {"mountainInfo", _mountainInfo}
                    };

                await Navigation.PushAsync<MountainInsuranceOffersPage>(parameters);
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

            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;
            this._mountainInfo = query.FirstOrDefault(x => x.Key == "mountainInfo").Value as MountainInsuranceOfferModel;
        }

        public void SetStartEndDate()
        {
            var startDate = DateTime.Now.AddDays(1);
            this.StartDateFormatted = startDate.ToString("dd.MM.yyyy");
            this.allowedStartDate = startDate;

            this.allowedEndDate = this.allowedStartDate.AddMonths(1).AddDays(-1);
            this.EndDateFormatted = this.allowedEndDate.ToString("dd.MM.yyyy");

            this.PeriodFormatted = $"{StartDateFormatted} - {EndDateFormatted}";
        }
        #endregion
    }
}
