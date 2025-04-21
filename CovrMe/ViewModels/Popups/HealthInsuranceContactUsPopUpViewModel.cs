using CommunityToolkit.Mvvm.Input;
using CovrMe.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Popups
{
    public partial class HealthInsuranceContactUsPopUpViewModel : BaseViewModel
    {
        #region Fields

        private double _displayWidth;

        #endregion

        public HealthInsuranceContactUsPopUpViewModel()
        {
            SetScreenWidth();
        }

        #region Props

        public double DisplayWidth
        {
            get { return this._displayWidth; }
            set { SetProperty(ref this._displayWidth, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async void GoToContactForm(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.ContactUsURL;
                await OpenBrowser(uri);
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
        private void SetScreenWidth()
        {
            this.DisplayWidth = (DeviceDisplay.MainDisplayInfo.Width / 2) - 200;
        }

        private async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("#50E3C6"),
                    PreferredControlColor = Color.FromHex("#C0C0C0"),
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

    }
}
