using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.HealthInsurance
{
    public partial class HealthInsurancePeriodPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private DateTime _startDate;
        private DateTime _endDate;

        private string _startDateFormatted;
        private string _endDateFormatted;

        private ObservableCollection<YearsPickerModel> _yearsCollection;
        private ObservableCollection<MonthsPickerModel> _monthsCollection;

        private YearsPickerModel _selectedYear;
        private MonthsPickerModel _selectedMonth;

        private int _nextMonthYear;
        private string _nextMonth;

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private HealthInsuranceOfferModel _healthInfo;

        string[] monthNames = { "Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември" };
        #endregion

        public HealthInsurancePeriodPageViewModel()
        {
            this.YearsCollection = new ObservableCollection<YearsPickerModel>();
            this.MonthsCollection = new ObservableCollection<MonthsPickerModel>();

            this.PopulateYearsCollection(); 
            SetStartEndDate();
            //this.PopulateMonthsCollection(DateTime.Now.Year);

            //this.SetStartEndDate();
        }

        #region Collections

        public ObservableCollection<YearsPickerModel> YearsCollection
        {
            get => _yearsCollection;
            set => SetProperty(ref _yearsCollection, value);
        }

        public ObservableCollection<MonthsPickerModel> MonthsCollection
        {
            get => _monthsCollection;
            set => SetProperty(ref _monthsCollection, value);
        }

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
        public int NextMonthYear
        {
            get { return _nextMonthYear; }
            set { SetProperty(ref _nextMonthYear, value); }
        }
        public string NextMonth
        {
            get { return _nextMonth; }
            set { SetProperty(ref _nextMonth, value); }
        }
        public YearsPickerModel SelectedYear
        {
            get { return _selectedYear; }
            set { SetProperty(ref _selectedYear, value); }
        }

        public MonthsPickerModel SelectedMonth
        {
            get { return _selectedMonth; }
            set { SetProperty(ref _selectedMonth, value); }
        }

        #endregion

        #region Command

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

                App.CalendarStartDate = this._startDate;
                App.CalendarEndDate = this._endDate;

                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", this._usersAge},
                        {"healthInfo", _healthInfo}
                    };


                await Navigation.PushAsync<HealthInsuranceOffersPage>(parameters);
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

        public void PopulateYearsCollection()
        {
            //var yearsCollection = new ObservableCollection<YearsPickerModel>();

            //var currentYear = DateTime.Now.Year;

            //var currentYearElement = new YearsPickerModel
            //{
            //    YearFormatted = $"{currentYear} г.",
            //    Year = currentYear,
            //};

            //this.SelectedYear = currentYearElement;
            //yearsCollection.Add(currentYearElement);

            //var nextYear = DateTime.Now.AddYears(1).Year;
            //var nextYearElement = new YearsPickerModel
            //{
            //    YearFormatted = $"{nextYear} г.",
            //    Year = nextYear,
            //};
            //yearsCollection.Add(nextYearElement);

            //this.YearsCollection = yearsCollection;

            var nextMonth = DateTime.Now.AddMonths(1).Month;
            var nextMonthYear = DateTime.Now.AddMonths(1).Year;

            NextMonth = monthNames[nextMonth - 1];
            NextMonthYear = nextMonthYear;
        }

        //public void PopulateMonthsCollection(int year)
        //{
        //    var currentYear = DateTime.Now.Year;
        //    int currentMonth = 0;
        //    var monthsCollection = new ObservableCollection<MonthsPickerModel>();

        //    if (currentYear == year)
        //    {
        //        currentMonth = DateTime.Now.AddMonths(1).Month;
        //    }
        //    else
        //    {
        //        currentMonth = 1;
        //    }

        //    for (int month = currentMonth; month <= 12; month++)
        //    {
        //        var current = new MonthsPickerModel
        //        {
        //            Month = month,
        //            MonthFormatted = monthNames[month - 1]
        //        };

        //        monthsCollection.Add(current);
        //    }
        //    var selected = monthsCollection.FirstOrDefault();

        //    this.SelectedMonth = selected;

        //    this.MonthsCollection.Clear();
        //    this.MonthsCollection = monthsCollection;
        //}

        public void SetStartEndDate()
        {
            string month = DateTime.Now.AddMonths(1).Month.ToString();
            int year = DateTime.Now.AddMonths(1).Year;

            if (month.Length == 1)
            {
                month = $"0{month}";
            }

            string startDate = $"{month}-01-{year}";
            this._startDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture);
            this.StartDateFormatted = this._startDate.ToString("dd.MM.yyyy");

            this._endDate = this._startDate.AddYears(1).AddDays(-1);
            this.EndDateFormatted = this._endDate.ToString("dd.MM.yyyy");
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            _healthInfo = query.FirstOrDefault(x => x.Key == "healthInfo").Value as HealthInsuranceOfferModel;
            _usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;

        }

        #endregion
    }
}
