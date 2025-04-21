using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.HealthInsurance
{
    public partial class HealthInsuranceInsuredUsersViewModel : BaseViewModel
    {
        #region Fields

        private int _age;
        private bool _isFamily;
        private HealthInsuranceOfferModel _healthInfo;
        private ObservableCollection<InsuredUsersAgeTemplateModel> _insuredUsersAges;
        #endregion

        public HealthInsuranceInsuredUsersViewModel()
        {
            this.InsuredUsersAgesList = new ObservableCollection<InsuredUsersAgeTemplateModel>();
            this._healthInfo = new HealthInsuranceOfferModel();

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

        public bool IsFamily
        {
            get { return _isFamily; }
            set { SetProperty(ref _isFamily, value); }
        }

        #endregion

        #region Command

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

                if (this.IsFamily)
                {
                    if (userAgeList.Count == 1)
                    {
                        throw new Exception(MessageConstants.FamilyPolicyMoreThanOneUsersError);
                    }
                    if(userAgeList.Count > 5)
                    {
                        throw new Exception(MessageConstants.FamilyPolicyMoreThanFiveUsersError);
                    }
                }
                else
                {
                    if (userAgeList.Count > 1)
                    { 
                        throw new Exception(MessageConstants.NonFamilyPolicyOnlyOneUserError);
                    }
                }

                this._healthInfo.IsFamily = this.IsFamily;

                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", userAgeList},
                        {"healthInfo", _healthInfo}
                    };


                await Navigation.PushAsync<HealthInsurancePacketsPage>(parameters);
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
