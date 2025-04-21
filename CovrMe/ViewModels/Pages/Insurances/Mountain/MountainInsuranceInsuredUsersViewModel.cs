using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.MountainInsurance;
using CovrMe.Shared.Constants;
using CovrMe.ViewModels.Popups;
using CovrMe.Views.Pages.Insurances.Mountain;
using CovrMe.Views.Popups;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.Mountain
{
    public partial class MountainInsuranceInsuredUsersViewModel : BaseViewModel
    {
        #region Fields
        private int _age;
        private bool _isExtreme;
        //collections
        private ObservableCollection<InsuredUsersAgeTemplateModel> _insuredUsersAges;
        private MountainInsuranceOfferModel _mountainInfo;
        private readonly IPopupService _popupService;

        #endregion

        public MountainInsuranceInsuredUsersViewModel(IPopupService popupService)
        {
            this.InsuredUsersAgesList = new ObservableCollection<InsuredUsersAgeTemplateModel>();
            this._mountainInfo = new MountainInsuranceOfferModel();
            this._popupService = popupService;

            SetFirstInsuredUserAsDefault();
        }

        #region Collections

        public ObservableCollection<InsuredUsersAgeTemplateModel> InsuredUsersAgesList
        {
            get { return _insuredUsersAges; }
            set
            {
                _insuredUsersAges = value;
                OnPropertyChanged(nameof(InsuredUsersAgesList));
            }
        }


        #endregion

        #region Props

        public int Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }

        public bool IsExtreme
        {
            get { return _isExtreme; }
            set { SetProperty(ref _isExtreme, value); }
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

                List<InsuredUsersAgeTemplateModel> userAgeList = this.InsuredUsersAgesList.ToList();

                if (userAgeList.Count == 0)
                {
                    throw new Exception(MessageConstants.NoInsuredUsersError);
                }

                if (userAgeList.Any(x => string.IsNullOrEmpty(x.Age)))
                {
                    throw new Exception(MessageConstants.AgeIsZeroError);
                }

                if (userAgeList.Any(x => x.Age != null && int.Parse(x.Age) > GlobalConstants.InappropriateAge))
                {
                    throw new Exception(string.Format(MessageConstants.InappropriatedAgeError, GlobalConstants.InappropriateAge));
                }

                this._mountainInfo.IsExtreme = this.IsExtreme;

                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", userAgeList},
                        {"mountainInfo", _mountainInfo}
                    };

                if (userAgeList.Count >= 5)
                {
                    App.IsGroupInsurance = true; // group Mountain Insurance
                }
                else
                {
                    App.IsGroupInsurance = false;
                }

                await Navigation.PushAsync<MountainInsuranceCalendarPage>(parameters);
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

        [RelayCommand]
        private async void ShowInfo()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var result = await this._popupService.ShowPopupAsync<MountainXtreemInfoPopUpViewModel>(CancellationToken.None);
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

        private async void SetFirstInsuredUserAsDefault()
        {
            var newInsuredUser = new InsuredUsersAgeTemplateModel
            {
                Age = "18",
                Index = 0,
                Number = 1
            };

            this.InsuredUsersAgesList.Add(newInsuredUser);
        }

        #endregion


    }
}
