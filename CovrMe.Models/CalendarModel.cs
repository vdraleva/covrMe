using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models
{
    public class CalendarModel : PropertyChangedModel
    {
        public DateTime Date { get; set; }
        private bool _isCurrentDate;
        private bool _isEnabled;
        private bool _isEndDate;
        private bool _isStartDate;
        private bool _isSelected;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetField(ref _isEnabled, value);
        }
        public bool IsCurrentDate
        {
            get => _isCurrentDate;
            set => SetField(ref _isCurrentDate, value);
        }
        public bool IsStartDate
        {
            get => _isStartDate;
            set => SetField(ref _isStartDate, value);
        }
        public bool IsEndDate
        {
            get => _isEndDate;
            set => SetField(ref _isEndDate, value);
        }
        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }
    }
}
