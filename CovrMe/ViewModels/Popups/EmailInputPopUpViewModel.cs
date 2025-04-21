using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Deliveries.Result;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Popups
{
    public partial class EmailInputPopUpViewModel : BaseViewModel
    {
        #region Fields
        private string _email;
        private bool emailError;
        private double _displayWidth;
        private IUserService _userService;

        #endregion

        public EmailInputPopUpViewModel(IUserService userService)
        {
            _userService = userService;
            SetScreenWidth();
        }

        #region Props

        public double DisplayWidth
        {
            get { return this._displayWidth; }
            set { SetProperty(ref this._displayWidth, value); }
        }

        public string Email
        {
            get { return this._email; }
            set { SetProperty(ref this._email, value); }
        }

        public bool EmailError
        {
            get { return emailError; }
            set
            {
                if (emailError != value)
                {
                    emailError = value;
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }

        #endregion

        #region Commands

        #endregion

        #region Methods
        public async Task<UserModel?> UpdateEmail()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Email))
                {
                    this.EmailError = true;
                    throw new InvalidOperationException(MessageConstants.InvalidEmail);
                }

                var httpClient = HttpClientFactory.Create();
                var req = new EditUserInfoInput
                {
                    UserId = App.UserId,
                    Email = Email,
                };

                ShowLoading();

                var res = await _userService.EditUserInfo(req, httpClient, App.JwtToken);

                if (res.Message != null) 
                {
                    throw new InvalidOperationException(res.Message);
                }

                return res;
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
                return null;
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        private void SetScreenWidth()
        {
            this.DisplayWidth = (DeviceDisplay.MainDisplayInfo.Width / 2) - 200;
        }

        #endregion

    }
}
