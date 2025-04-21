using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages
{
    public partial class SuccessfulRegistrationPageViewModel : BaseViewModel
    {
        public SuccessfulRegistrationPageViewModel()
        {
            
        }

        #region Commands
        [RelayCommand]
        private async Task GoToLoginPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();

                await this.Navigation.PopToRootAsync();

            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }

        #endregion
    }
}
