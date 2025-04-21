
using Controls.UserDialogs.Maui;
using CovrMe.Models;
using CovrMe.Observable;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CovrMe.Views.ContentViews;

public partial class CalendarPeriodPickerView
{
    private DateTime _bufferDate;
    private DateTime? _currentStartDate;
    private DateTime? _currentEndDate;
    private bool _deselectDates;
    private ObservableCollection<InsurancePeriodPickerModel> _periodCollection;
    private InsurancePeriodPickerModel _selectedPeriod;
    private bool _isBusy = false;


    private static void SelectedDatePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var calendarView = (CalendarPeriodPickerView)bindable;

        if (calendarView.SelectedStartDate != DateTime.MinValue && calendarView.SelectedEndDate != DateTime.MinValue)
        {
            TimeSpan dateRange = calendarView.SelectedEndDate - calendarView.SelectedStartDate;
            var insuranceValidDays = dateRange.Days + 1;

            if (insuranceValidDays < 0) insuranceValidDays = 0;

            calendarView.SelectedStartDateLabel = $"Валидна от: {calendarView.SelectedStartDate:dd.MM.yyyy г.}";
            calendarView.SelectedEndDateLabel = $"Валидна до: {calendarView.SelectedEndDate:dd.MM.yyyy г.}";

            if (insuranceValidDays >= 90 && insuranceValidDays <= 93)
            {
                calendarView.ValidDaysLabel = "Валидност: 3 месеца";
            }
            else if (insuranceValidDays >= 364 && insuranceValidDays <= 366)
            {
                calendarView.ValidDaysLabel = "Валидност: 12 месеца";
            }
            else
            {
                calendarView.ValidDaysLabel = $"Валидност: {insuranceValidDays} дни";
            }

            App.CalendarStartDate = calendarView.SelectedStartDate;
            App.CalendarEndDate = calendarView.SelectedEndDate;
        }
    }
    #region Binable properties
    public ObservableDictionary<int, List<CalendarModel>> Weeks
    {
        get => (ObservableDictionary<int, List<CalendarModel>>)GetValue(WeeksProperty);
        set => SetValue(WeeksProperty, value);
    }
    public ObservableCollection<InsurancePeriodPickerModel> PeriodCollection
    {
        get => (ObservableCollection<InsurancePeriodPickerModel>)GetValue(PeriodCollectionProperty);
        set => SetValue(PeriodCollectionProperty, value);
    }

    public InsurancePeriodPickerModel SelectedPeriod
    {
        get => (InsurancePeriodPickerModel)GetValue(SelectedPeriodProperty);
        set => SetValue(SelectedPeriodProperty, value);
    }


    public static readonly BindableProperty WeeksProperty =
        BindableProperty.Create(nameof(Weeks), typeof(ObservableDictionary<int, List<CalendarModel>>), typeof(CalendarPeriodPickerView));

    public static readonly BindableProperty PeriodCollectionProperty =
       BindableProperty.Create(nameof(PeriodCollection), typeof(ObservableCollection<InsurancePeriodPickerModel>), typeof(CalendarPeriodPickerView), null);

    public static readonly BindableProperty SelectedPeriodProperty =
       BindableProperty.Create(nameof(SelectedPeriod), typeof(InsurancePeriodPickerModel), typeof(CalendarPeriodPickerView), null);

    public static readonly BindableProperty InsuranceTypeProperty = BindableProperty.Create(
             nameof(InsuranceType), typeof(string), typeof(CalendarPeriodPickerView), string.Empty);

    public string InsuranceType
    {
        get => (string)GetValue(InsuranceTypeProperty);
        set => SetValue(InsuranceTypeProperty, value);
    }
    public ICommand SelectedDateCommand
    {
        get => (ICommand)GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public static readonly BindableProperty SelectedStartDateProperty = BindableProperty.Create(
       nameof(SelectedStartDate),
       typeof(DateTime),
       typeof(CalendarPeriodPickerView),
       DateTime.Now,
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanged: SelectedDatePropertyChanged);

    public static readonly BindableProperty SelectedEndDateProperty = BindableProperty.Create(
        nameof(SelectedEndDate),
        typeof(DateTime),
        typeof(CalendarPeriodPickerView),
        DateTime.Now,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: SelectedDatePropertyChanged);

    public string CalendarLabel
    {
        get => (string)GetValue(CalendarLabelProperty);
        set => SetValue(CalendarLabelProperty, value);
    }

    public string SelectedStartDateLabel
    {
        get => (string)GetValue(SelectedStartDateLabelProperty);
        set => SetValue(SelectedStartDateLabelProperty, value);
    }
    public static readonly BindableProperty CalendarLabelProperty = BindableProperty.Create(
   nameof(CalendarLabel),
   typeof(string),
   typeof(CalendarPeriodPickerView),
   string.Empty);

    public static readonly BindableProperty SelectedCurrentDate = BindableProperty.Create(
    nameof(CurrentDate),
    typeof(DateTime),
    typeof(CalendarPeriodPickerView),
    DateTime.Now);

    public static readonly BindableProperty SelectedEndDateLabelProperty = BindableProperty.Create(
       nameof(SelectedEndDateLabel),
       typeof(string),
       typeof(CalendarPeriodPickerView),
       string.Empty);

    public string SelectedEndDateLabel
    {
        get => (string)GetValue(SelectedEndDateLabelProperty);
        set => SetValue(SelectedEndDateLabelProperty, value);
    }

    public static readonly BindableProperty SelectedStartDateLabelProperty = BindableProperty.Create(
       nameof(SelectedStartDateLabel),
       typeof(string),
       typeof(CalendarPeriodPickerView),
       string.Empty);

    public static readonly BindableProperty ValidDaysLabelProperty = BindableProperty.Create(
     nameof(ValidDaysLabel),
     typeof(string),
     typeof(CalendarPeriodPickerView),
     string.Empty);

    public string ValidDaysLabel
    {
        get => (string)GetValue(ValidDaysLabelProperty);
        set => SetValue(ValidDaysLabelProperty, value);
    }

    public static readonly BindableProperty IsNextMonthProperty = BindableProperty.Create(
      nameof(IsNextMonth),
      typeof(bool),
      typeof(CalendarPeriodPickerView),
      true);

    public bool IsNextMonth
    {
        get => (bool)GetValue(IsNextMonthProperty);
        set => SetValue(IsNextMonthProperty, value);
    }

    public static readonly BindableProperty IsCurrentMonthProperty = BindableProperty.Create(
     nameof(IsCurrentMonth),
     typeof(bool),
     typeof(CalendarPeriodPickerView),
     false);
    #endregion
    public bool IsCurrentMonth
    {
        get => (bool)GetValue(IsCurrentMonthProperty);
        set => SetValue(IsCurrentMonthProperty, value);
    }

    public CalendarPeriodPickerView()
    {
        InitializeComponent();
        BindingContext = this;
        this.PeriodCollection = new ObservableCollection<InsurancePeriodPickerModel>();
        this.PopulatePeriodCollection();

        this.PeriodManagement();
    }

    public void BindDates(DateTime date)
    {
        SetWeeks(date);

        if (this._deselectDates)
        {
            this.DeselectAllDates();
            this._deselectDates = false;
        }

        var choseDate = Weeks.SelectMany(x => x.Value).FirstOrDefault(f => f.Date.Date == date.Date);
        if (choseDate != null)
        {
            choseDate.IsCurrentDate = true;
            _bufferDate = choseDate.Date;
            this.CurrentDate = choseDate.Date;

            this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);

            //if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Custom)
            //{
            //    this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
            //}
            //else
            //{
            //    this.MarkSelectedDate(this.SelectedStartDate);
            //}

        }
    }

    private void SetWeeks(DateTime date)
    {
        DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        int weekNumber = 1;

        var newWeeks = new ObservableDictionary<int, List<CalendarModel>>();

        DateTime today = DateTime.Today;
        DateTime maxAllowedDate = today.AddMonths(1);
        if (!string.IsNullOrEmpty(InsuranceType))
        {
            switch (InsuranceType)
            {
                case "Mountain":
                    maxAllowedDate = today.AddMonths(1);
                    break;
                case "MyThings":
                    maxAllowedDate = today.AddDays(90);
                    break;
                default:
                    break;
            }
        }

        bool isCustomPeriod = this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Custom;
        bool hasStartDate = this._currentStartDate.HasValue;
        DateTime? startDate = this._currentStartDate;

        // Add days from previous month to the first week
        for (int i = 0; i < (int)firstDayOfMonth.DayOfWeek; i++)
        {
            DateTime firstDate = firstDayOfMonth.AddDays(-((int)firstDayOfMonth.DayOfWeek - i));
            if (!newWeeks.ContainsKey(weekNumber))
            {
                newWeeks.Add(weekNumber, new List<CalendarModel>());
            }

            bool isEnabled = (isCustomPeriod && hasStartDate && firstDate.Date >= startDate) || (firstDate.Date >= today.AddDays(1) && firstDate.Date <= maxAllowedDate);
            var calendarModel = new CalendarModel { Date = firstDate, IsEnabled = isEnabled };
            newWeeks[weekNumber].Add(calendarModel);
        }

        // Add days from current month
        for (int day = 1; day <= daysInMonth; day++)
        {
            DateTime dateInMonth = new DateTime(date.Year, date.Month, day);
            if (dateInMonth.DayOfWeek == DayOfWeek.Sunday && day != 1)
            {
                weekNumber++;
            }
            if (!newWeeks.ContainsKey(weekNumber))
            {
                newWeeks.Add(weekNumber, new List<CalendarModel>());
            }
            bool isEnabled = (isCustomPeriod && hasStartDate && dateInMonth.Date >= startDate) || (dateInMonth.Date >= today.AddDays(1) && dateInMonth.Date <= maxAllowedDate);
            var calendarModel = new CalendarModel { Date = dateInMonth, IsEnabled = isEnabled };
            newWeeks[weekNumber].Add(calendarModel);
        }

        // Add days from next month to last week
        DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, daysInMonth);
        for (int i = 1; i <= 6 - (int)lastDayOfMonth.DayOfWeek; i++)
        {
            DateTime lastDate = lastDayOfMonth.AddDays(i);
            if (!newWeeks.ContainsKey(weekNumber))
            {
                newWeeks.Add(weekNumber, new List<CalendarModel>());
            }
            bool isEnabled = (isCustomPeriod && hasStartDate && lastDate.Date >= startDate) || (lastDate.Date >= today.AddDays(1) && lastDate.Date <= maxAllowedDate);
            var calendarModel = new CalendarModel { Date = lastDate, IsEnabled = isEnabled };
            newWeeks[weekNumber].Add(calendarModel);
        }

        Weeks = newWeeks;
        OnPropertyChanged(nameof(Weeks));
    }


    #region Props
    public DateTime SelectedStartDate
    {
        get => (DateTime)GetValue(SelectedStartDateProperty);
        set
        {
            SetValue(SelectedStartDateProperty, value);
            OnPropertyChanged(nameof(SelectedStartDate));
        }
    }

    public DateTime CurrentDate
    {
        get => (DateTime)GetValue(SelectedCurrentDate);
        set
        {
            SetValue(SelectedCurrentDate, value);
            OnPropertyChanged(nameof(SelectedCurrentDate));
        }
    }

    public DateTime SelectedEndDate
    {
        get => (DateTime)GetValue(SelectedEndDateProperty);
        set
        {
            SetValue(SelectedEndDateProperty, value);
            OnPropertyChanged(nameof(SelectedEndDate));
        }
    }

    #endregion

    #region Commands
    public ICommand CurrentDateCommand => new Command<CalendarModel>((currentDate) =>
    {
        _bufferDate = currentDate.Date;
        SelectedStartDate = currentDate.Date;
        SelectedDateCommand?.Execute(currentDate.Date);
    }, (currentDate) =>
    {
        return currentDate.Date >= DateTime.Today;
    });
    public ICommand TapCommand => new Command<CalendarModel>(async (selectedDate) =>
    {
        try
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            ShowLoading();

            DateTime today = DateTime.Today;
            var maxAllowedDate = DateTime.Today.AddMonths(1);
            if (!string.IsNullOrEmpty(InsuranceType))
            {
                switch (InsuranceType)
                {
                    case "Mountain":
                        maxAllowedDate = DateTime.Today.AddMonths(1);
                        break;
                    case "MyThings":
                        maxAllowedDate = DateTime.Today.AddDays(90);
                        break;
                    default:
                        break;
                }
            }

            if (selectedDate.Date.Date < DateTime.Today.AddDays(1).Date || !selectedDate.IsEnabled)
            {
                return;
            }

            if (App.IsGroupInsurance) // group Mountain Insurance
            {
                if (_currentStartDate != null && selectedDate.Date >= _currentStartDate.Value.AddDays(30))
                {
                    await App.Current.MainPage.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.MountainGroupMaxDateError, App.MESSAGE_OK);
                    return;
                }
            }
            if (selectedDate.IsSelected)
            {
                DeselectAllDates();
            }

            if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Custom)
            {
                if (_currentStartDate == null || _currentEndDate != null)
                {
                    if (selectedDate.Date > maxAllowedDate)
                    {
                        selectedDate.IsSelected = false;
                        await App.Current.MainPage.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.ChooseValidStartDateError, App.MESSAGE_OK);
                        return;
                    }

                    this.DeselectAllDates();

                    this.SelectedStartDate = selectedDate.Date;
                    this._currentStartDate = SelectedStartDate;

                    selectedDate.IsSelected = true;

                    foreach (var week in Weeks)
                    {
                        foreach (var day in week.Value)
                        {
                            if (day.Date.Date > today && day.Date.Date >= _currentStartDate)
                            {
                                day.IsEnabled = true;
                            }
                            else
                            {
                                day.IsEnabled = false;
                            }
                        }
                    }

                    this.SelectedEndDate = DateTime.MinValue;
                    this._currentEndDate = null;
                }
                else
                {
                    if (selectedDate.Date <= _currentStartDate)
                    {
                        await App.Current.MainPage.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.StartDateMustBeAfterEndDate, App.MESSAGE_OK);
                        return;
                    }

                    this.SelectedEndDate = selectedDate.Date;
                    this._currentEndDate = SelectedEndDate;

                    BindDates(_bufferDate);
                }
            }
            else if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Year)
            {
                this.SelectedStartDate = selectedDate.Date;
                this.SelectedEndDate = this.SelectedStartDate.AddYears(1).AddDays(-1);

                this._currentStartDate = this.SelectedStartDate;
                this._currentEndDate = this.SelectedEndDate;
                this.CurrentDate = selectedDate.Date;

                this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
            }
            else if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Months)
            {
                this.SelectedStartDate = selectedDate.Date;
                this.SelectedEndDate = this.SelectedStartDate.AddMonths(3).AddDays(-1);

                this._currentStartDate = this.SelectedStartDate;
                this._currentEndDate = this.SelectedEndDate;
                this.CurrentDate = selectedDate.Date;

                this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
        finally
        {
            _isBusy = false;
            HideLoading();
        }
    });
    public ICommand NextMonthCommand => new Command(() =>
    {
        try
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            ShowLoading();
            _bufferDate = _bufferDate.AddMonths(1);
            BindDates(_bufferDate);
        }
        catch (Exception)
        {
        }
        finally
        {
            _isBusy = false;
            HideLoading();
        }

    });
    public ICommand PreviousMonthCommand => new Command(() =>
    {
        try
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;
            ShowLoading();
            _bufferDate = _bufferDate.AddMonths(-1);
            BindDates(_bufferDate);
        }
        catch (Exception)
        {

        }
        finally
        {
            _isBusy = false;
            HideLoading();
        }

    });
    private void MarkSelectedDates(DateTime start, DateTime end)
    {
        foreach (var week in Weeks)
        {
            foreach (var day in week.Value)
            {
                if (day.Date.Date.Date >= start.Date && day.Date.Date.Date <= end.Date)
                {
                    day.IsSelected = true;
                }
            }
        }
    }
    private void DeselectDatesExceptSelectedOne(CalendarModel selected)
    {
        foreach (var week in Weeks)
        {
            foreach (var day in week.Value)
            {
                if (day.Date.Date < DateTime.Today.Date)
                {
                    continue;
                }

                // Deselect all dates except the selected date
                day.IsSelected = (day == selected);

            }
        }
    }
    private void DeselectAllDates()
    {
        foreach (var week in Weeks)
        {
            foreach (var day in week.Value)
            {
                if (day.Date.Date < DateTime.Today.Date)
                {
                    continue;
                }

                day.IsSelected = false;

            }
        }
    }
    private void PeriodManagement()
    {
        if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Year)
        {
            this.CurrentDate = DateTime.Now.AddDays(1);
            SelectedStartDate = DateTime.Now.AddDays(1);
            SelectedEndDate = this.SelectedStartDate.AddYears(1).AddDays(-1);

            this._currentStartDate = this.SelectedStartDate;
            this._currentEndDate = this.SelectedEndDate;

            //this.MarkSelectedDate(this.SelectedStartDate);
        }

        if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Months)
        {
            this.CurrentDate = DateTime.Now.AddDays(1);
            SelectedStartDate = DateTime.Now.AddDays(1);
            SelectedEndDate = this.SelectedStartDate.AddMonths(3).AddDays(-1);

            this._currentStartDate = this.SelectedStartDate;
            this._currentEndDate = this.SelectedEndDate;
            //this.MarkSelectedDate(this.SelectedStartDate);
        }

        if (this.SelectedPeriod.Id == (int)InsurancePeriodEnum.Custom)
        {
            this.CurrentDate = DateTime.Now.AddDays(1);
            SelectedStartDate = DateTime.Now.AddDays(1);
            SelectedEndDate = this.SelectedStartDate.AddDays(3);

            this._currentStartDate = this.SelectedStartDate;
            this._currentEndDate = this.SelectedEndDate;

            //this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
        }

        this._deselectDates = true;
        BindDates(this.SelectedStartDate);

    }
    private void PopulatePeriodCollection()
    {
        var year = new InsurancePeriodPickerModel
        {
            Text = "Една година",
            Id = 1
        };

        var months = new InsurancePeriodPickerModel
        {
            Text = "Три месеца",
            Id = 2
        };

        var custom = new InsurancePeriodPickerModel
        {
            Text = "Персонален",
            Id = 3
        };

        this.PeriodCollection.Add(year);
        this.PeriodCollection.Add(months);

        if (App.InsuranceType != (int)InsuranceTypeEnum.MyThings)
        {
            this.PeriodCollection.Add(custom);
            this.SelectedPeriod = custom;
        }
        else
        {
            App.InsuranceType = 0;
            this.SelectedPeriod = year;
        }

        if (App.IsGroupInsurance) //this means group MountainInsurance with only 30 days custom period
        {
            this.PeriodCollection.Clear();
            this.PeriodCollection.Add(custom);
            this.SelectedPeriod = custom;
            App.IsGroupInsurance = false;
        }
    }
    private void PeriodPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = PeriodPicker.SelectedItem as InsurancePeriodPickerModel;

        if (selectedOption != null)
        {
            this.SelectedPeriod = selectedOption;
            this.PeriodManagement();
        }
    }
    private void ShowLoading(string message = null)
    {
        string msg = "Зареждане";
        UserDialogs.Instance.ShowLoading(msg);

    }
    private void HideLoading()
    {
        UserDialogs.Instance.HideHud();
    }
    #endregion
}