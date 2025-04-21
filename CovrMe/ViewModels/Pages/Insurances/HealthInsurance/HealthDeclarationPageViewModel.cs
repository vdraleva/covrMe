using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.ViewModels.Popups;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.HealthInsurance
{
    public partial class HealthDeclarationPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private InsuranceOfferModel _selectedOffer;

        private bool _firstQuestionAnswer = false;
        private bool _secondQuestionAnswer = false;
        private bool _thirdQuestionAnswer = false;

        private readonly IPopupService _popupService;

        #endregion

        public HealthDeclarationPageViewModel(IPopupService popupService)
        {
            this._popupService = popupService;
        }

        #region Props

        public bool FirstQuestionAnswer
        {
            get { return _firstQuestionAnswer; }
            set { SetProperty(ref _firstQuestionAnswer, value); }
        }

        public bool SecondQuestionAnswer
        {
            get { return _secondQuestionAnswer; }
            set { SetProperty(ref _secondQuestionAnswer, value); }
        }

        public bool ThirdQuestionAnswer
        {
            get { return _thirdQuestionAnswer; }
            set { SetProperty(ref _thirdQuestionAnswer, value); }
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

                if(this.FirstQuestionAnswer || this.SecondQuestionAnswer || this.ThirdQuestionAnswer)
                {
                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        await this._popupService.ShowPopupAsync<HealthInsuranceContactUsPopUpViewModel>(CancellationToken.None);
                    });

                    //var result = await this._popupService.ShowPopupAsync<HealthInsuranceContactUsPopUpViewModel>(CancellationToken.None);
                }
                else
                {
                    var questions = this.PopulateQuestionModel();
                    var questionnaire = new QuestionnaireModel();
                    questionnaire.Questionnaire = questions;

                    this._selectedOffer.HealthInsuranceInfo.Questionnaire = questionnaire;
                    var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", this._usersAge},
                        {"selectedOffer", _selectedOffer},
                    };

                    await Navigation.PushAsync<InsuranceOwnerPage>(parameters);
                }
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

            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;
            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
        }

        private List<QuestionModel> PopulateQuestionModel()
        {
            var result = new List<QuestionModel>();

            var question1 = new QuestionModel
            {
                QuestionId = 1,
                Answer = "не"
            };

            var question2 = new QuestionModel
            {
                QuestionId = 2,
                Answer = "не"
            };

            var question3 = new QuestionModel
            {
                QuestionId = 3,
                Answer = "не"
            };

            result.Add(question1);
            result.Add(question2);
            result.Add(question3);
            return result;
        }

        #endregion
    }
}
