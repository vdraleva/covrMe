using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceSummaryViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private bool _firstCardOpened;
        private bool _secondCardOpened;
        private bool _thirdCardOpened;
        private bool _forthCardOpened;

        private string _firstCardArrow = GlobalConstants.PlusImg;
        private string _secondCardArrow = GlobalConstants.PlusImg;
        private string _thirdCardArrow = GlobalConstants.PlusImg;
        private string _forthCardArrow = GlobalConstants.PlusImg;

        private string _endDate;
        private string _startDate;

        private string logoSrc;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        private decimal _totalPrice;
        private int _installment = 1;

        //insurance
        private InsuranceOfferModel _selectedOffer;

        private string firstInstallmentFormatted;
        private string secondInstallmentFormatted;
        private string thirdInstallmentFormatted;
        private string fourthInstallmentFormatted;

        private string _secondInstallmentDate;
        private string _thirdInstallmentDate;
        private string _fourthInstallmentDate;

        private ObservableCollection<InstallmentModel> _installments;
        #endregion

        public CivilInsuranceSummaryViewModel()
        {
            var sDate = App.CalendarStartDate;
            StartDate = sDate.ToString("dd.MM.yyyy");

            var eDate = App.CalendarEndDate;
            EndDate = eDate.ToString("dd.MM.yyyy");

            this.Installments = new ObservableCollection<InstallmentModel>();
        }

        #region Collections

        public ObservableCollection<InstallmentModel> Installments
        {
            get => _installments;
            set => SetProperty(ref _installments, value);
        }

        #endregion

        #region Props
        public InsuranceOfferModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }
        public string StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        public string EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }
        public string LogoSrc
        {
            get { return logoSrc; }
            set { SetProperty(ref logoSrc, value); }
        }

        public bool FirstCardOpened
        {
            get { return this._firstCardOpened; }
            set { SetProperty(ref this._firstCardOpened, value); }
        }
        public bool SecondCardOpened
        {
            get { return this._secondCardOpened; }
            set { SetProperty(ref this._secondCardOpened, value); }
        }
        public bool ThirdCardOpened
        {
            get { return this._thirdCardOpened; }
            set { SetProperty(ref this._thirdCardOpened, value); }
        }
        public bool ForthCardOpened
        {
            get { return this._forthCardOpened; }
            set { SetProperty(ref this._forthCardOpened, value); }
        }

        public string FirstCardArrow
        {
            get { return this._firstCardArrow; }
            set { SetProperty(ref this._firstCardArrow, value); }
        }
        public string SecondCardArrow
        {
            get { return this._secondCardArrow; }
            set { SetProperty(ref this._secondCardArrow, value); }
        }
        public string ThirdCardArrow
        {
            get { return this._thirdCardArrow; }
            set { SetProperty(ref this._thirdCardArrow, value); }
        }
        public string ForthCardArrow
        {
            get { return this._forthCardArrow; }
            set { SetProperty(ref this._forthCardArrow, value); }
        }

        #endregion

        #region Methods
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;

            this._installment = this.SelectedOffer.Installment;

            //this.SecondInstallmentDate = this._selectedOffer.SecondInstallmentDate;
            //this.ThirdInstallmentDate = this._selectedOffer.ThirdInstallmentDate;
            //this.FourthInstallmentDate = this._selectedOffer.FirstInstallmentDate;

            this.Installments = this._selectedOffer.Installments;
            //InstallmentToggle();
        }

        //private void InstallmentToggle()
        //{
        //    FirstInstallmentFormatted = SelectedOffer.FirstInstallmentPriceFormatted;
        //    SecondInstallmentFormatted = SelectedOffer.SecondInstallmentPriceFormatted;
        //    ThirdInstallmentFormatted = SelectedOffer.ThirdInstallmentPriceFormatted;
        //    FourthInstallmentFormatted = SelectedOffer.FourthInstallmentPriceFormatted;
        //    LogoSrc = SelectedOffer.CompanyLogo;

        //    switch (_installment)
        //    {
        //        case 1:
        //            IsOneInstallment = true;
        //            IsTwoInstallment = false;
        //            IsFourInstallment = false;
        //            break;
        //        case 2:
        //            IsOneInstallment = false;
        //            IsTwoInstallment = true;
        //            IsFourInstallment = false;
        //            break;
        //        case 4:
        //            IsOneInstallment = false;
        //            IsTwoInstallment = false;
        //            IsFourInstallment = true;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region Commands
        [RelayCommand]
        void CardToggle(string toggleName)
        {
            switch (toggleName)
            {
                case "first":
                    this.FirstCardOpened = !this.FirstCardOpened;
                    this.FirstCardArrow = this.FirstCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;

                case "second":
                    this.SecondCardOpened = !this.SecondCardOpened;
                    this.SecondCardArrow = this.SecondCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;

                case "third":
                    this.ThirdCardOpened = !this.ThirdCardOpened;
                    this.ThirdCardArrow = this.ThirdCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;

                case "forth":
                    this.ForthCardOpened = !this.ForthCardOpened;
                    this.ForthCardArrow = this.ForthCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    break;

                default:
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;
            }
        }

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

                this._selectedOffer.StartDateFormatted = this._startDate;
                this._selectedOffer.EndDateFormatted = this._endDate;

                var parameters = new Dictionary<string, object>
                    {                                                                        
                        {"selectedOffer", this._selectedOffer},
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo}
                    };

                await Navigation.PushAsync<CivilInsuranceDocumentsPage>(parameters);
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
    }
}
