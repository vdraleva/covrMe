using CommunityToolkit.Mvvm.ComponentModel;
using Controls.UserDialogs.Maui;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CovrMe.ViewModels
{
    public partial class BaseViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Properties

        [ObservableProperty]
        private bool _isBusy = false;

        public INavigation Navigation { get; set; }

        protected static void ShowLoading(string message = null)
        {
            string msg = "Зареждане";
            UserDialogs.Instance.ShowLoading(msg);

        }
        protected static void HideLoading()
        {
            UserDialogs.Instance.HideHud();
        }

        #endregion

        protected bool SetProperty<T>(ref T backingStore, T value,
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

        #region INotifyPropertyChanged

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

        #endregion
    }
}
