using CommunityToolkit.Mvvm.ComponentModel;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Result.Pickers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users
{
    public class InsuredUserDataModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _uin;
        private DateTime _birthDate;
        private bool _save;
        private string _textColor;
        private bool _isOpen;
        private bool _isFilled;
        private UserFamilyAndFriendsPicker _selectedUser;
        private LocationDataModel _selectedNationality;

        private bool _firstNameError;
        private bool _lastNameError;
        private bool _uinNumberError;
        private bool _birthDateError;
        private bool _nationalityError;
        private ObservableCollection<UserFamilyAndFriendsPicker> _familyAndFriendsCollection;
        private ObservableCollection<LocationDataModel> _countryCollection;
        public InsuredUserDataModel()
        {
            this.FamilyAndFriendsCollection = new ObservableCollection<UserFamilyAndFriendsPicker>();
            this.CountryCollection = new ObservableCollection<LocationDataModel>();
            this.BirthDate = DateTime.Parse("01/01/1990");
        }

        public ObservableCollection<UserFamilyAndFriendsPicker> FamilyAndFriendsCollection
        {
            get => _familyAndFriendsCollection;
            set => SetProperty(ref _familyAndFriendsCollection, value);
        }
        public ObservableCollection<LocationDataModel> CountryCollection
        {
            get => _countryCollection;
            set => SetProperty(ref _countryCollection, value);
        }
        public string FirstName
        {
            get { return this._firstName; }
            set { SetProperty(ref this._firstName, value); }
        }
        public string LastName
        {
            get { return this._lastName; }
            set { SetProperty(ref this._lastName, value); }
        }
        public string Uin
        {
            get { return this._uin; }
            set { SetProperty(ref this._uin, value); }
        }
        public DateTime BirthDate
        {
            get { return this._birthDate; }
            set { SetProperty(ref this._birthDate, value); }
        }
        public bool Save
        {
            get { return this._save; }
            set { SetProperty(ref this._save, value); }
        }
        public string TextColor
        {
            get { return this._textColor; }
            set { SetProperty(ref this._textColor, value); }
        }
        public int Number { get; set; }
        public int Index { get; set; }
        public bool IsOpen
        {
            get { return this._isOpen; }
            set { SetProperty(ref this._isOpen, value); }
        }
        public bool IsFilled
        {
            get { return this._isFilled; }
            set { SetProperty(ref this._isFilled, value); }
        }
        public bool IsValid { get; set; }
        public string? ValidationMessage { get; set; }

        public UserFamilyAndFriendsPicker SelectedUser
        {
            get { return this._selectedUser; }
            set
            {
                if (value != null)
                {
                    this._selectedUser = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(this.SelectedUser)));
                    }
                }

            }
        }

        public LocationDataModel SelectedNationality
        {
            get { return this._selectedNationality; }
            set
            {
                if (value != null)
                {
                    this._selectedNationality = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(this.SelectedNationality)));
                    }
                }

            }
        }

        #region Error Props

        public bool FirstNameError
        {
            get { return _firstNameError; }
            set
            {
                if (_firstNameError != value)
                {
                    _firstNameError = value;
                    OnPropertyChanged(nameof(FirstNameError));
                }
            }
        }

        public bool LastNameError
        {
            get { return _lastNameError; }
            set
            {
                if (_lastNameError != value)
                {
                    _lastNameError = value;
                    OnPropertyChanged(nameof(LastNameError));
                }
            }
        }

        public bool UiNumberError
        {
            get { return _uinNumberError; }
            set
            {
                if (_uinNumberError != value)
                {
                    _uinNumberError = value;
                    OnPropertyChanged(nameof(UiNumberError));
                }
            }
        }

        public bool BirthDateError
        {
            get { return _birthDateError; }
            set
            {
                if (_birthDateError != value)
                {
                    _birthDateError = value;
                    OnPropertyChanged(nameof(BirthDateError));
                }
            }
        }

        public bool NationalityError
        {
            get { return _nationalityError; }
            set
            {
                if (_nationalityError != value)
                {
                    _nationalityError = value;
                    OnPropertyChanged(nameof(NationalityError));
                }
            }
        }

        #endregion

        public bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
