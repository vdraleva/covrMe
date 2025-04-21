using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages
{
    public partial class ErrorPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private bool _isError;
        private string _errorId;
        private string _errorMessage;

        #endregion

        #region Props
        public string ErrorId
        {
            get { return this._errorId; }
            set { SetProperty(ref this._errorId, value); }
        }

        public bool IsError
        {
            get { return this._isError; }
            set { SetProperty(ref this._isError, value); }
        }

        public string ErrorMessage
        {
            get { return this._errorMessage; }
            set { SetProperty(ref this._errorMessage, value); }
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

            this.ErrorId = query.FirstOrDefault(x => x.Key == "errorId").Value as string;
            this.ErrorMessage = query.FirstOrDefault(x => x.Key == "errorMessage").Value as string;

            if (!string.IsNullOrEmpty(this._errorId))
            {
                this.IsError = true;
            }
        }
        #endregion

    }
}
