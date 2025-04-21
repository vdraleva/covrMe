using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Deliveries;
using CovrMe.Models.Insurances;
using CovrMe.Models.Payments.Request;
using CovrMe.Models.Payments.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Payment;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Payment
{
    public partial class PrePaymentPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private decimal _totalPrice;
        private string _premiumFormatted;
        private string _premiumWithoutTaxFormatted;
        private string _taxFormatted;
        private string _email;
        private bool _emailError;

        private InsuranceDeliveryModel _deliveryInfo;
        private decimal _deliveryPrice = 0;
        private IPaymentService _paymentService;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;
        private List<InsuredUserDataModel> _insuredUsersInfo;

        private string _vehiclePlateNumber;
        private string _policyNo;
        private string _paymentInfo;
        private string _companyName;
        private string _insurerNames;
        private bool _showPolicyNo;

        private bool _showVehicleNo;
        private bool _showInsurerName;
        private bool _showTaxInfo;

        private string _title;

        //insurance
        private InsuranceOfferModel _selectedOffer;



        #endregion

        public PrePaymentPageViewModel(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        #region Props
        public InsuranceOfferModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }
        public string Email
        {
            get { return this._email; }
            set { SetProperty(ref this._email, value); }
        }

        public string PremiumFormatted
        {
            get { return this._premiumFormatted; }
            set { SetProperty(ref this._premiumFormatted, value); }
        }

        public string PremiumWithoutTaxFormatted
        {
            get { return this._premiumWithoutTaxFormatted; }
            set { SetProperty(ref this._premiumWithoutTaxFormatted, value); }
        }

        public string TaxFormatted
        {
            get { return this._taxFormatted; }
            set { SetProperty(ref this._taxFormatted, value); }
        }
        public string VehiclePlateNumber
        {
            get { return this._vehiclePlateNumber; }
            set { SetProperty(ref this._vehiclePlateNumber, value); }
        }
        public string PolicyNo
        {
            get { return this._policyNo; }
            set { SetProperty(ref this._policyNo, value); }
        }
        public string PaymentInfo
        {
            get { return this._paymentInfo; }
            set { SetProperty(ref this._paymentInfo, value); }
        }

        public string InsurerNames
        {
            get { return this._insurerNames; }
            set { SetProperty(ref this._insurerNames, value); }
        }
        public string CompanyName
        {
            get { return this._companyName; }
            set { SetProperty(ref this._companyName, value); }
        }
        public bool ShowPolicyNo
        {
            get { return this._showPolicyNo; }
            set { SetProperty(ref this._showPolicyNo, value); }
        }
        public bool ShowVehicleNo
        {
            get { return this._showVehicleNo; }
            set { SetProperty(ref this._showVehicleNo, value); }
        }
        public bool ShowTaxInfo
        {
            get { return this._showTaxInfo; }
            set { SetProperty(ref this._showTaxInfo, value); }
        }
        public bool ShowInsurerName
        {
            get { return this._showInsurerName; }
            set { SetProperty(ref this._showInsurerName, value); }
        }

        public bool EmailError
        {
            get { return _emailError; }
            set
            {
                if (_emailError != value)
                {
                    _emailError = value;
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }

        public string Title
        {
            get { return this._title; }
            set { SetProperty(ref this._title, value); }
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
                if (string.IsNullOrEmpty(this.Email))
                {
                    this.EmailError = true;
                    throw new Exception(MessageConstants.RequiredEmail);
                }

                this._selectedOffer.Email = this.Email;

                var payment = await this.CreatePayment();

                var docs = new InsuranceDocumentModel
                {
                    StickerId = payment.StickerId,
                    GreenCardId = payment.GreencardId,
                    DocumentBatchId = payment.DocumentBatchId,
                    LocalOrderNumber = payment.LocalOrderNumber,
                    PaymentFormUrl = payment.FormUrl
                };
                
                this.SelectedOffer.Title = this.Title;

                if (this.Title == GlobalConstants.CivilInsurance)
                {
                    if (string.IsNullOrEmpty(docs.DocumentBatchId) || string.IsNullOrEmpty(docs.GreenCardId) || string.IsNullOrEmpty(docs.StickerId))
                    {
                        throw new Exception(MessageConstants.CivilInsuranceMissingStickersError);
                    }
                }

                if (!string.IsNullOrEmpty(payment.FormUrl))
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                        {"selectedOffer", this._selectedOffer},
                        {"insuranceDocuments", docs},
                        {"deliveryInfo", _deliveryInfo},
                        {"insuredUsers", this._insuredUsersInfo},
                    };

                    await Navigation.PushAsync<PaymentPage>(parameters);
                }
                else
                {
                    throw new Exception(MessageConstants.GeneralError);
                }
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
            this._insuredUsersInfo = query.FirstOrDefault(x => x.Key == "insuredUsers").Value as List<InsuredUserDataModel>;

            //insurance
            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;

            //this.PremiumFormatted = this._selectedOffer.Installment > 1 ? this._selectedOffer.FirstInstallmentPriceFormatted : _selectedOffer.PriceFormatted;

            this.PremiumFormatted = this._selectedOffer.PriceFormatted;
            this.PremiumWithoutTaxFormatted = this._selectedOffer.PriceWithoutTaxFormatted;
            this.TaxFormatted = this._selectedOffer.TaxFormatted;

            if (this._selectedOffer.InsuranceType != (byte)InsuranceTypeEnum.Civil)
            {
                this.ShowTaxInfo = true;
            }

            //delivery
            this._deliveryInfo = query.FirstOrDefault(x => x.Key == "deliveryInfo").Value as InsuranceDeliveryModel;

            if (_vehicleInfo != null)
            {
                this.ShowVehicleNo = true;
                this.VehiclePlateNumber = this._vehicleInfo.PlateNumber;
            }
            else
            {
                this.ShowInsurerName = true;
                this.InsurerNames = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName;
            }

            this.PolicyNo = this._selectedOffer.PolicyNo;
            this.PaymentInfo = this.GetPaymentInfo();
            this.CompanyName = this.GetCompanyName();

            if (!string.IsNullOrEmpty(this.PolicyNo))
            {
                this.ShowPolicyNo = true;
            }

            this.Email = App.Email;

            this.Title = Helpers.GetPageTitle(this._selectedOffer.InsuranceType);

        }

        public async Task<CreatePaymentResultModel> CreatePayment()
        {
            var result = new CreatePaymentResultModel();

            try
            {
                var httpClient = HttpClientFactory.Create();
                var jwt = App.JwtToken;
                var userId = App.UserId;

                var req = new CreatePaymentInput
                {
                    Amount = this.SelectedOffer.Price,
                    UserId = userId,
                    VehiclePlateNumber = this._vehicleInfo != null ? this._vehicleInfo.PlateNumber : string.Empty,
                    InsuranceCompanyName = this.SelectedOffer.CompanyName,
                    InsuranceType = this.SelectedOffer.InsuranceType
                };

                var paymentResult = await this._paymentService.Payment(req, jwt, httpClient);

                if (paymentResult != null)
                {
                    return paymentResult;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception)
            {
                return result;
            }
        }

        private string GetPaymentInfo()
        {
            string result = string.Empty;

            if (this._selectedOffer.NextInstallment)
            {
                switch (this._selectedOffer.InstallmentToPay)
                {
                    case 2:
                        result = "Втора вноска";
                        break;

                    case 3:
                        result = "Трета вноска";
                        break;

                    case 4:
                        result = "Четвърта вноска";
                        break;

                    case 5:
                        result = "Пета вноска";
                        break;

                    case 6:
                        result = "Шеста вноска";
                        break;

                    case 7:
                        result = "Седма вноска";
                        break;

                    case 8:
                        result = "Осма вноска";
                        break;

                    case 9:
                        result = "Девета вноска";
                        break;

                    case 10:
                        result = "Десета вноска";
                        break;

                    case 11:
                        result = "Единадесета вноска";
                        break;

                    case 12:
                        result = "Дванадесета вноска";
                        break;
                }
            }
            else
            {
                if (this._selectedOffer.Installment == 1)
                {
                    result = "Еднократно плащане";
                }
                else
                {
                    result = "Първа вноска";
                }
            }

            return result;

        }

        private string GetCompanyName()
        {
            InsuranceCompanyEnum insuranceCompanyEnum = (InsuranceCompanyEnum)Enum.Parse(typeof(InsuranceCompanyEnum), this._selectedOffer.CompanyName);
            return Helpers.GetEnumDescription(insuranceCompanyEnum);
        }
        #endregion
    }
}
