using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;

namespace CovrMe.ViewModels.Pages.Insurances.TravelInsurance
{
    public partial class TravelInsuranceCalendarViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private TravelInsuranceOfferModel _travelInfo;
        private List<InsuredUsersAgeTemplateModel> _usersAge;

        #endregion
        public TravelInsuranceCalendarViewModel()
        {

        }

        #region Props
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
                        {"travelInfo", this._travelInfo},
                        {"userAgeList", _usersAge}
                    };

                await Navigation.PushAsync<TravelInsuranceOffersPage>(parameters);
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

            this._travelInfo = query.FirstOrDefault(x => x.Key == "travelInfo").Value as TravelInsuranceOfferModel;
            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;
        }

        #endregion
    }
}
