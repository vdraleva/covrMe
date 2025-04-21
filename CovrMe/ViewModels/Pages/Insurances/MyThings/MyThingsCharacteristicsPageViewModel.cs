using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.MyThings;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.MyThings
{
    public partial class MyThingsCharacteristicsPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;

        private DateTime _purchaseDate;
        private string _brand;
        private string _model;
        private string _sum;
        private string _color;
        private bool _isElectric;
        private string _additionalEquipment;
        private string _additionalClarification;
        private bool _hasDamamge;
        private bool _hasUnrepairedDamage;
        private string _size;
        private string _unrepairedInfo;

        private bool _isError;
        private bool _brandError;
        private bool _modelError;
        private bool _colorError;
        private bool _sumError;
        private bool _sizeError;
        private bool _unrepairedInfoError;

        private MyThingsInsuranceOfferModel _myThingsInfo;

        private bool _isGlasses;
        private bool _isBycicle;
        private int _maxInsuranceSum = 5000;

        #endregion

        public MyThingsCharacteristicsPageViewModel()
        {
            DateTime now = DateTime.Now;
            DateTime targetDate = now.AddYears(-6).AddMonths(-11); //uniqa wants purchase date to be max before 6 years and 11 months
            DateTime firstDayOfTargetMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
            this.MaxPickerDate = now;
            this.MinPickerDate = firstDayOfTargetMonth;


            this.PurchaseDate = DateTime.Now;
        }

        #region Props
        public string Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        public int MaxInsuranceSum
        {
            get { return _maxInsuranceSum; }
            set { SetProperty(ref _maxInsuranceSum, value); }
        }

        public string AdditionalClarification
        {
            get { return _additionalClarification; }
            set { SetProperty(ref _additionalClarification, value); }
        }

        public string AdditionalEquipment
        {
            get { return _additionalEquipment; }
            set { SetProperty(ref _additionalEquipment, value); }
        }

        public string UnrepairedInfo
        {
            get { return _unrepairedInfo; }
            set { SetProperty(ref _unrepairedInfo, value); }
        }

        public string Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public string Sum
        {
            get { return _sum; }
            set { SetProperty(ref _sum, value); }
        }

        public string Brand
        {
            get { return _brand; }
            set { SetProperty(ref _brand, value); }
        }

        public string Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public DateTime MinPickerDate
        {
            get { return _minPickerDate; }
            set { SetProperty(ref _minPickerDate, value); }
        }

        public DateTime MaxPickerDate
        {
            get { return _maxPickerDate; }
            set { SetProperty(ref _maxPickerDate, value); }
        }

        public DateTime PurchaseDate
        {
            get { return _purchaseDate; }
            set { SetProperty(ref _purchaseDate, value); }
        }

        public bool HasUnrepairedDamage
        {
            get { return _hasUnrepairedDamage; }
            set
            {
                if (_hasUnrepairedDamage != value)
                {
                    _hasUnrepairedDamage = value;
                    OnPropertyChanged(nameof(HasUnrepairedDamage));
                }
            }
        }

        public bool HasDamamge
        {
            get { return _hasDamamge; }
            set { SetProperty(ref _hasDamamge, value); }
        }

        public bool IsElectric
        {
            get { return _isElectric; }
            set { SetProperty(ref _isElectric, value); }
        }

        public bool IsGlasses
        {
            get { return _isGlasses; }
            set { SetProperty(ref _isGlasses, value); }
        }

        public bool BrandError
        {
            get { return _brandError; }
            set
            {
                if (_brandError != value)
                {
                    _brandError = value;
                    OnPropertyChanged(nameof(BrandError));
                }
            }
        }

        public bool ModelError
        {
            get { return _modelError; }
            set
            {
                if (_modelError != value)
                {
                    _modelError = value;
                    OnPropertyChanged(nameof(ModelError));
                }
            }
        }

        public bool ColorError
        {
            get { return _colorError; }
            set
            {
                if (_colorError != value)
                {
                    _colorError = value;
                    OnPropertyChanged(nameof(ColorError));
                }
            }
        }

        public bool SumError
        {
            get { return _sumError; }
            set
            {
                if (_sumError != value)
                {
                    _sumError = value;
                    OnPropertyChanged(nameof(SumError));
                }
            }
        }

        public bool SizeError
        {
            get { return _sizeError; }
            set
            {
                if (_sizeError != value)
                {
                    _sizeError = value;
                    OnPropertyChanged(nameof(SizeError));
                }
            }
        }

        public bool IsError
        {
            get { return _isError; }
            set
            {
                if (_isError != value)
                {
                    _isError = value;
                    OnPropertyChanged(nameof(IsError));
                }
            }
        }

        public bool UnrepairedInfoError
        {
            get { return _unrepairedInfoError; }
            set
            {
                if (_unrepairedInfoError != value)
                {
                    _unrepairedInfoError = value;
                    OnPropertyChanged(nameof(UnrepairedInfoError));
                }
            }
        }

        public bool IsBycicle
        {
            get { return _isBycicle; }
            set { SetProperty(ref _isBycicle, value); }
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

                var valResult = this.ValidateInput();

                if (!valResult.IsValid)
                {
                    throw new Exception(valResult.Message);
                }

                var questionnaire = this.PopulateQuestionnaire();

                this._myThingsInfo.Questionnaire = questionnaire;

                string formattedSum = this.Sum.Replace(',', '.');
                decimal sum = decimal.Parse(formattedSum);

                this._myThingsInfo.Brand = this.Brand;
                this._myThingsInfo.Model = this.Model;

                if (sum <= 0)
                {
                    this.SumError = true;
                    this.IsError = true;

                    throw new Exception(MessageConstants.SumGreaterThanZeroError);
                }

                if (sum > this._maxInsuranceSum)
                {
                    this.SumError = true;
                    this.IsError = true;

                    throw new Exception(MessageConstants.SumNotBiggerThan + $"{this._maxInsuranceSum} лв.");
                }

                this._myThingsInfo.InsuranceSum = sum;

                var parameters = new Dictionary<string, object>
                    {
                        {"myThingsInfo", this._myThingsInfo}
                    };

                App.InsuranceType = (int)InsuranceTypeEnum.MyThings;
                await Navigation.PushAsync<MyThingsInsuranceCalendarPage>(parameters);
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

        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (this.PurchaseDate > DateTime.Now)
            {
                res.IsValid = false;
                res.Message = MessageConstants.PurchaseDateFutureDateError;
            }
            if (this.PurchaseDate < this.MinPickerDate)
            {
                res.IsValid = false;
                res.Message = string.Format(MessageConstants.PurchaseDateOldDateError, this.MinPickerDate.ToString("MM/yyyy", CultureInfo.InvariantCulture));
            }

            if (string.IsNullOrEmpty(this.Brand))
            {
                this.BrandError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.BrandRequiredError;
            }


            if (string.IsNullOrEmpty(this.Model))
            {
                this.ModelError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.ModelRequiredError;
            }


            if (string.IsNullOrEmpty(this.Color))
            {
                this.ColorError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.ColorRequiredError;
            }

            if (string.IsNullOrEmpty(this.Sum))
            {
                this.SumError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.SumRequiredError;
            }
            else
            {
                string formattedSum = this.Sum.Replace(',', '.');
                decimal sum = decimal.Parse(formattedSum);
                if (sum < 100)
                {
                    this.SumError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.SumMoreThan100Error;
                }
            }

            if (this.IsGlasses)
            {
                if (string.IsNullOrEmpty(this.Size))
                {
                    this.SizeError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.SizeRequiredError;
                }
                else
                {
                    if (this.Size.Length < 3)
                    {
                        this.SizeError = true;
                        this.IsError = true;
                        res.IsValid = false;
                        res.Message = MessageConstants.SizeLengthError;
                    }
                }
            }

            if (this.HasUnrepairedDamage && string.IsNullOrEmpty(this.UnrepairedInfo))
            {
                this.ColorError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.UnrepairedDamageRequiredError;
            }

            return res;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._myThingsInfo = query.FirstOrDefault(x => x.Key == "myThingsInfo").Value as MyThingsInsuranceOfferModel;

            if (this._myThingsInfo.PropertyTypeId == (int)MyThingsPropertyTypeEnum.Bicycle)
            {
                this.IsBycicle = true;
                this.IsGlasses = false;
            }
            else
            {
                this.IsBycicle = false;
                this.IsGlasses = true;

                this.MaxInsuranceSum = 2000;

                DateTime now = DateTime.Now;
                DateTime targetDate = now.AddYears(-1).AddMonths(-6);
                DateTime firstDayOfTargetMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

                firstDayOfTargetMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

                this.MaxPickerDate = now;
                this.MinPickerDate = firstDayOfTargetMonth;
            }
            
        }

        private QuestionnaireModel PopulateQuestionnaire()
        {
            var questionnaire = new QuestionnaireModel();

            var question1 = new QuestionModel
            {
                QuestionId = 1,
                Answer = this.PurchaseDate.ToString("MM/yyyy", CultureInfo.InvariantCulture)
            };

            var question2 = new QuestionModel
            {
                QuestionId = 2,
                Answer = this.Brand
            };

            var question3 = new QuestionModel
            {
                QuestionId = 3,
                Answer = this.Model
            };

            var question4 = new QuestionModel
            {
                QuestionId = 4,
                Answer = this.Color
            };

            questionnaire.Questionnaire.Add(question1);
            questionnaire.Questionnaire.Add(question2);
            questionnaire.Questionnaire.Add(question3);
            questionnaire.Questionnaire.Add(question4);

            if (this.IsBycicle)
            {
                var bicicleQuestion5 = new QuestionModel
                {
                    QuestionId = 5,
                    Answer = this.IsElectric ? "да" : "не"
                };


                var bicicleQuestion6 = new QuestionModel
                {
                    QuestionId = 6,
                    Answer = !string.IsNullOrEmpty(this.AdditionalEquipment) ? this.AdditionalEquipment : null
                };


                var bicicleQuestion7 = new QuestionModel
                {
                    QuestionId = 7,
                    Answer = !string.IsNullOrEmpty(this.AdditionalClarification) ? this.AdditionalClarification : null
                };

                var bicicleQuestion8 = new QuestionModel
                {
                    QuestionId = 8,
                    Answer = this.HasDamamge ? "да" : "не"
                };


                var bicicleQuestion9 = new QuestionModel
                {
                    QuestionId = 9,
                    Answer = this.HasUnrepairedDamage ? "да" : "не"
                };

                var bicicleQuestion10 = new QuestionModel
                {
                    QuestionId = 10,
                    Answer = this.HasUnrepairedDamage ? this.UnrepairedInfo : null
                };

                questionnaire.Questionnaire.Add(bicicleQuestion5);
                questionnaire.Questionnaire.Add(bicicleQuestion6);
                questionnaire.Questionnaire.Add(bicicleQuestion7);
                questionnaire.Questionnaire.Add(bicicleQuestion8);
                questionnaire.Questionnaire.Add(bicicleQuestion9);
                questionnaire.Questionnaire.Add(bicicleQuestion10);
            }
            else
            {
                var glassesQuestion5 = new QuestionModel
                {
                    QuestionId = 5,
                    Answer = this.Size
                };


                var glassesQuestion6 = new QuestionModel
                {
                    QuestionId = 6,
                    Answer = !string.IsNullOrEmpty(this.AdditionalClarification) ? this.AdditionalClarification : null
                };

                questionnaire.Questionnaire.Add(glassesQuestion5);
                questionnaire.Questionnaire.Add(glassesQuestion6);
            }

            return questionnaire;
        }

        #endregion
    }
}
