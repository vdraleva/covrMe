using CovrMe.Models;
using CovrMe.Observable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Windows.Input;

namespace CovrMe.Views.ContentViews;

public partial class CalendarView
{
    #region Props
    private DateTime _bufferDate;
    private DateTime _currentMonth;
    private bool _isBusy = false;

    public DateTime SelectedStartDate { get; set; }
    public DateTime SelectedEndDate { get; set; }
    public DateTime CurrentMonth
    {
        get => _currentMonth;
        set
        {
            _currentMonth = value;
            OnPropertyChanged(nameof(CurrentMonth));
        }
    }

    #endregion

    #region BindableProperty
    public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(
        nameof(SelectedDate),
        typeof(DateTime),
        typeof(CalendarView),
        DateTime.Now,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: SelectedDatePropertyChanged);

    public static readonly BindableProperty SelectedDateLabelYearLaterProperty = BindableProperty.Create(
       nameof(SelectedDateLabelYearLater),
       typeof(string),
       typeof(CalendarView),
       string.Empty);

    public string SelectedDateLabelYearLater
    {
        get => (string)GetValue(SelectedDateLabelYearLaterProperty);
        set => SetValue(SelectedDateLabelYearLaterProperty, value);
    }

    public static readonly BindableProperty SelectedDateLabelProperty = BindableProperty.Create(
       nameof(SelectedDateLabel),
       typeof(string),
       typeof(CalendarView),
       string.Empty);

    public string SelectedDateLabel
    {
        get => (string)GetValue(SelectedDateLabelProperty);
        set => SetValue(SelectedDateLabelProperty, value);
    }

    public static readonly BindableProperty IsNextMonthProperty = BindableProperty.Create(
      nameof(IsNextMonth),
      typeof(bool),
      typeof(CalendarView),
      true);

    public bool IsNextMonth
    {
        get => (bool)GetValue(IsNextMonthProperty);
        set => SetValue(IsNextMonthProperty, value);
    }

    public static readonly BindableProperty IsCurrentMonthProperty = BindableProperty.Create(
     nameof(IsCurrentMonth),
     typeof(bool),
     typeof(CalendarView),
     false);

    public bool IsCurrentMonth
    {
        get => (bool)GetValue(IsCurrentMonthProperty);
        set => SetValue(IsCurrentMonthProperty, value);
    }
    private static void SelectedDatePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var controls = (CalendarView)bindable;
        if (newvalue != null)
        {
            var newDate = (DateTime)newvalue;

            if (newDate.Date <= DateTime.Today.AddDays(45))
            {
                if (controls._bufferDate.Month == newDate.Month && controls._bufferDate.Year == newDate.Year)
                {
                    var currentDate = controls.Weeks.SelectMany(x => x.Value)
                                                     .FirstOrDefault(f => f.Date == newDate.Date);

                    if (currentDate != null)
                    {
                        controls.Weeks.ToList().ForEach(x => x.Value.ToList().ForEach(y => y.IsCurrentDate = false));
                        currentDate.IsCurrentDate = true;
                    }

                    if (oldvalue == null || (DateTime)oldvalue != newDate)
                    {
                        controls.SelectedStartDate = newDate;
                        controls.SelectedDateLabel = $"Валидна от: {controls.SelectedStartDate:dd.MM.yyyy г.}";

                        DateTime oneYearLater = newDate.AddYears(1).AddDays(-1);
                        controls.SelectedDateLabelYearLater = $"Валидна до: {oneYearLater:dd.MM.yyyy г.}";
                        controls.SelectedEndDate = oneYearLater;

                        App.CalendarStartDate = controls.SelectedStartDate;
                        App.CalendarEndDate = controls.SelectedEndDate;

                        var currentMonth = controls.SelectedDate.Month;
                        if (DateTime.Now.Year < controls.SelectedDate.Year || (DateTime.Now.Year == controls.SelectedDate.Year && DateTime.Now.Month < currentMonth))
                        {
                            controls.IsNextMonth = true;
                            controls.IsCurrentMonth = false;
                        }
                        else
                        {
                            controls.IsNextMonth = false;
                            controls.IsCurrentMonth = true;
                        }
                    }
                    else
                    {
                        controls.SelectedStartDate = DateTime.Now;
                        controls.SelectedDateLabel = $"Валидна от: {controls.SelectedStartDate:dd.MM.yyyy г.}";

                        DateTime oneYearLater = DateTime.Now.AddYears(1).AddDays(-1);
                        controls.SelectedDateLabelYearLater = $"Валидна до: {oneYearLater:dd.MM.yyyy г.}";
                        controls.SelectedEndDate = oneYearLater;
                    }
                }
                else
                {
                    controls.BindDates(newDate);
                }
            }
        }
    }

    public static readonly BindableProperty WeeksProperty =
        BindableProperty.Create(nameof(Weeks), typeof(ObservableDictionary<int, List<CalendarModel>>), typeof(CalendarView));
    public ObservableDictionary<int, List<CalendarModel>> Weeks
    {
        get => (ObservableDictionary<int, List<CalendarModel>>)GetValue(WeeksProperty);
        set => SetValue(WeeksProperty, value);
    }
    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly BindableProperty SelectedDateCommandProperty = BindableProperty.Create(
        nameof(SelectedDateCommand),
        typeof(ICommand),
        typeof(CalendarView));

    public ICommand SelectedDateCommand
    {
        get => (ICommand)GetValue(SelectedDateCommandProperty);
        set => SetValue(SelectedDateCommandProperty, value);
    }

    #endregion
    public CalendarView()
    {
        InitializeComponent();

        if (App.NewInsuranceStartDate.Date != DateTime.MinValue.Date)
        {
            this.SelectedStartDate = App.NewInsuranceStartDate;
            this.SelectedEndDate = this.SelectedStartDate.AddYears(1).AddDays(-1);
            this.CurrentMonth = DateTime.Today;
            App.NewInsuranceStartDate = DateTime.MinValue;
        }
        else
        {
            this.CurrentMonth = DateTime.Today;
            this.SelectedStartDate = DateTime.Now.AddDays(1);
            this.SelectedEndDate = this.SelectedStartDate.AddYears(1).AddDays(-1);
            App.CalendarStartDate = this.SelectedStartDate;
            App.CalendarEndDate = this.SelectedEndDate;
        }

        _bufferDate = CurrentMonth;

        this.SelectedDateLabel = $"Валидна от: {this.SelectedStartDate:dd.MM.yyyy г.}";
        this.SelectedDateLabelYearLater = $"Валидна до: {this.SelectedEndDate:dd.MM.yyyy г.}";

        BindDates(this.SelectedStartDate);

        BindingContext = this;
    }
    public void BindDates(DateTime date)
    {
        CurrentMonth = new DateTime(date.Year, date.Month, 1); // Set CurrentMonth to the first day of the month
        if (DateTime.Now.Year < this.SelectedDate.Year || (DateTime.Now.Year == this.SelectedDate.Year && DateTime.Now.Month < CurrentMonth.Month))
        {
            this.IsNextMonth = true;
            this.IsCurrentMonth = false;
        }
        else
        {
            this.IsNextMonth = false;
            this.IsCurrentMonth = true;
        }
        SetWeeks(date);

        //var choseDate = Weeks.SelectMany(x => x.Value)
        //                     .FirstOrDefault(f => f.Date.Date == date.Date && f.Date.Date <= DateTime.Today.AddDays(45));
        var choseDate = Weeks.SelectMany(x => x.Value).FirstOrDefault(f => f.Date.Date == date.Date);
        if (choseDate != null)
        {
            choseDate.IsCurrentDate = true;
            _bufferDate = choseDate.Date;

            if (SelectedDate == DateTime.MinValue)
            {
                SelectedDate = choseDate.Date;
            }

            this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
        }
    }

    private void SetWeeks(DateTime date)
    {
        DateTime today = DateTime.Today;
        DateTime maxAllowedDate = today.AddDays(46);

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

            bool isEnabled = firstDate.Date >= today.AddDays(1) && firstDate.Date <= maxAllowedDate;
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
            bool isEnabled = dateInMonth.Date >= today.AddDays(1) && dateInMonth.Date <= maxAllowedDate;
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
            bool isEnabled = lastDate.Date >= today.AddDays(1) && lastDate.Date <= maxAllowedDate;
            var calendarModel = new CalendarModel { Date = lastDate, IsEnabled = isEnabled };
            newWeeks[weekNumber].Add(calendarModel);
        }

        Weeks = newWeeks;
        OnPropertyChanged(nameof(Weeks));
    }


    #region Commands

    public ICommand TapCommand => new Command<CalendarModel>((selectedDate) =>
    {
        try
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            if (!selectedDate.IsEnabled)
            {
                return;
            }

            if (selectedDate.Date.Date < DateTime.Today.Date)
            {
                return;
            }

            this.DeselectAllDates();
            this._bufferDate = selectedDate.Date;
            this.SelectedDate = selectedDate.Date;
            this.SelectedStartDate = selectedDate.Date;

            this.MarkSelectedDates(this.SelectedStartDate, this.SelectedEndDate);
        }
        catch (Exception ex)
        {
        }
        finally
        {
            _isBusy = false;
        }
    });
    public ICommand CurrentDateCommand => new Command<CalendarModel>((currentDate) =>
    {
        _bufferDate = currentDate.Date;
        SelectedStartDate = currentDate.Date;
        SelectedDateCommand?.Execute(currentDate.Date);
    }, (currentDate) =>
    {
        return currentDate.Date >= DateTime.Today;
    });
    public ICommand NextMonthCommand => new Command(() =>
    {
        _bufferDate = _bufferDate.AddMonths(1);
        BindDates(_bufferDate);
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

    private void DeselectAllDates()
    {
        foreach (var week in Weeks)
        {
            foreach (var day in week.Value)
            {
                if (day.Date.Date <= DateTime.Today.Date)
                {
                    continue;
                }

                day.IsSelected = false;

            }
        }
    }
    #endregion
}