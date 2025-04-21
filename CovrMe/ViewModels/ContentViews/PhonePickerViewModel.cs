using CommunityToolkit.Mvvm.ComponentModel;
using CountryData.Standard;
using System.Collections.ObjectModel;

namespace CovrMe.ViewModels.ContentViews
{
    public partial class PhonePickerViewModel : ObservableObject
    {
        public ObservableCollection<Country> Countries { get; private set; }
        public PhonePickerViewModel()
        {
           this.Countries = new ObservableCollection<Country>(new CountryHelper().GetCountryData());
        }
    }
}
