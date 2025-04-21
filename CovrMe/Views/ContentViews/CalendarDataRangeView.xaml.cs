using CovrMe.Models;
using CovrMe.Observable;
using CovrMe.Shared.Constants;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CovrMe.Views.ContentViews;

public partial class CalendarDataRangeView
{
    private DateTime _bufferDate;
    private DateTime? _currentStartDate;
    private DateTime? _currentEndDate;

    private static void SelectedDatePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var calendarView = (CalendarDataRangeView)bindable;

        if (calendarView.SelectedStartDate != DateTime.MinValue && calendarView.SelectedEndDate != DateTime.MinValue)
        {
            TimeSpan dateRange = calendarView.SelectedEndDate - calendarView.SelectedStartDate;
            var insuranceValidDays = dateRange.Days + 1;

            if (insuranceValidDays < 0) insuranceValidDays = 0;

            calendarView.SelectedStartDateLabel = $"Валидна от: {calendarView.SelectedStartDate:dd.MM.yyyy г.}";
            calendarView.SelectedEndDateLabel = $"Валидна до: {calendarView.SelectedEndDate:dd.MM.yyyy г.}";

            calendarView.ValidDaysLabel = $"Валидност: {insuranceValidDays} дни";

            App.CalendarStartDate = calendarView.SelectedStartDate;
            App.CalendarEndDate = calendarView.SelectedEndDate;
        }
    }
    #region Binable properties
    public ObservableDictionary<int, List<CalendarModel>> Weeks
    {
        get => (ObservableDictionary<int, List<CalendarModel>>)GetValue(WeeksProperty);
        set
        {
            SetValue(WeeksProperty, value);
            OnPropertyChanged(nameof(Weeks));
        }
    }

    public static readonly BindableProperty WeeksProperty =
        BindableProperty.Create(nameof(Weeks), typeof(ObservableDictionary<int, List<CalendarModel>>), typeof(CalendarDataRangeView));

    public ICommand SelectedDateCommand
    {
        get => (ICommand)GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public static readonly BindableProperty SelectedStartDateProperty = BindableProperty.Create(
       nameof(SelectedStartDate),
       typeof(DateTime),
       typeof(CalendarDataRangeView),
       DateTime.Now,
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanged: SelectedDatePropertyChanged);

    public static readonly BindableProperty SelectedEndDateProperty = BindableProperty.Create(
        nameof(SelectedEndDate),
        typeof(DateTime),
        typeof(CalendarDataRangeView),
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
   typeof(CalendarDataRangeView),
   string.Empty);

    public static readonly BindableProperty SelectedCurrentDate = BindableProperty.Create(
    nameof(CurrentDate),
    typeof(DateTime),
    typeof(CalendarDataRangeView),
    DateTime.Now);

    public static readonly BindableProperty SelectedEndDateLabelProperty = BindableProperty.Create(
       nameof(SelectedEndDateLabel),
       typeof(string),
       typeof(CalendarDataRangeView),
       string.Empty);

    public string SelectedEndDateLabel
    {
        get => (string)GetValue(SelectedEndDateLabelProperty);
        set => SetValue(SelectedEndDateLabelProperty, value);
    }

    public static readonly BindableProperty SelectedStartDateLabelProperty = BindableProperty.Create(
       nameof(SelectedStartDateLabel),
       typeof(string),
       typeof(CalendarDataRangeView),
       string.Empty);

    public static readonly BindableProperty ValidDaysLabelProperty = BindableProperty.Create(
     nameof(ValidDaysLabel),
     typeof(string),
     typeof(CalendarDataRangeView),
     string.Empty);

    public string ValidDaysLabel
    {
        get => (string)GetValue(ValidDaysLabelProperty);
        set => SetValue(ValidDaysLabelProperty, value);
    }

    public static readonly BindableProperty IsNextMonthProperty = BindableProperty.Create(
      nameof(IsNextMonth),
      typeof(bool),
      typeof(CalendarDataRangeView),
      true);

    public bool IsNextMonth
    {
        get => (bool)GetValue(IsNextMonthProperty);
        set => SetValue(IsNextMonthProperty, value);
    }

    public static readonly BindableProperty IsCurrentMonthProperty = BindableProperty.Create(
     nameof(IsCurrentMonth),
     typeof(bool),
     typeof(CalendarDataRangeView),
     false);
    #endregion
    public bool IsCurrentMonth
    {
        get => (bool)GetValue(IsCurrentMonthProperty);
        set => SetValue(IsCurrentMonthProperty, value);
    }

    public CalendarDataRangeView()
    {
        InitializeComponent();
        BindingContext = this;

        this.CurrentDate = DateTime.Now.AddDays(1);
        SelectedStartDate = DateTime.Now.AddDays(1);
        SelectedEndDate = this.SelectedStartDate.AddDays(4);

        this._currentStartDate = this.SelectedStartDate;
        this._currentEndDate = this.SelectedEndDate;
        BindDates(DateTime.Now.AddDays(4));

        this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);

    }

    public void BindDates(DateTime date)
    {
        SetWeeks(date);
        var choseDate = Weeks.SelectMany(x => x.Value).FirstOrDefault(f => f.Date.Date == date.Date);
        if (choseDate != null)
        {
            choseDate.IsCurrentDate = true;
            _bufferDate = choseDate.Date;
            this.CurrentDate = choseDate.Date;
            this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
        }
    }

    private void SetWeeks(DateTime date)
    {
        DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        int weekNumber = 1;

        var newWeeks = new ObservableDictionary<int, List<CalendarModel>>();

        // Add days from previous month to the first week
        for (int i = 0; i < (int)firstDayOfMonth.DayOfWeek; i++)
        {
            DateTime firstDate = firstDayOfMonth.AddDays(-((int)firstDayOfMonth.DayOfWeek - i));
            if (!newWeeks.ContainsKey(weekNumber))
            {
                newWeeks.Add(weekNumber, new List<CalendarModel>());
            }

            bool isEnabled = firstDate.Date >= DateTime.Today.AddDays(1);
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
            bool isEnabled = dateInMonth.Date >= DateTime.Today.AddDays(1);
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
            bool isEnabled = lastDate.Date >= DateTime.Today.AddDays(1);
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
            OnPropertyChanged(nameof(CurrentDate));
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
        if (selectedDate.Date.Date < DateTime.Today.AddDays(1).Date)
        {
            return;
        }

        if (selectedDate.IsSelected)
        {
            this.DeselectDatesExceptSelectedOne(selectedDate);

            SelectedStartDate = selectedDate.Date;
            SelectedEndDate = DateTime.MinValue;

            this._currentStartDate = SelectedStartDate;
            this._currentEndDate = null;

            CurrentDate = selectedDate.Date;
            _bufferDate = CurrentDate;
        }
        else
        {
            this.DeselectDatesExceptSelectedOne(selectedDate);

            if (this._currentStartDate.HasValue && this._currentEndDate.HasValue)
            {
                selectedDate.IsSelected = true;
                SelectedStartDate = selectedDate.Date;
                SelectedEndDate = DateTime.MinValue;
                CurrentDate = selectedDate.Date;
                _bufferDate = CurrentDate;

                this._currentStartDate = selectedDate.Date;
                this._currentEndDate = null;
            }
            else
            {
                if (selectedDate.Date > SelectedStartDate)
                {
                    var startDate = SelectedStartDate.Date;
                    var endDate = selectedDate.Date;

                    this.MarkSelectedDates(startDate, endDate);

                    SelectedEndDate = selectedDate.Date;
                    CurrentDate = selectedDate.Date;
                    _bufferDate = CurrentDate;
                    this._currentEndDate = this.SelectedEndDate;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.StartDateMustBeAfterEndDate, App.MESSAGE_OK);
                    SelectedEndDateLabel = MessageConstants.StartDateMustBeAfterEndDate;
                }
            }
        }
    }); 
    public ICommand NextMonthCommand => new Command(() =>
    {
        try
        {
            _bufferDate = _bufferDate.AddMonths(1);
            BindDates(_bufferDate);
        }
        catch (Exception)
        {
            throw;
        }
    });

    public ICommand PreviousMonthCommand => new Command(() =>
    {
        _bufferDate = _bufferDate.AddMonths(-1);
        BindDates(_bufferDate);
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
    #endregion
}