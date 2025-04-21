using CommunityToolkit.Mvvm.Input;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class ExistingCivilInsurancePageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private string _currentInsuranceEndDate; 
        #endregion

        public ExistingCivilInsurancePageViewModel()
        {
            
        }

        #region Props

        public string CurrentInsuranceEndDate
        {
            get { return _currentInsuranceEndDate; }
            set { SetProperty(ref _currentInsuranceEndDate, value); }
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

                await Navigation.PushAsync<HomePage>();
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

            this.CurrentInsuranceEndDate = query.FirstOrDefault(x => x.Key == "endDate").Value as string;
           
        }

        #endregion
    }
}
