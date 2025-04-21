using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Request;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceVehicleAdditionalInfoViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;
        private RegCertificateResultModel _regCertificateModel;
        private List<string> _availableOffersCompanyNames;

        private string _vin;
        private string _engineVolume;
        private string _places;
        private string _netWeight;
        private string _grossWeight;
        private string _vehicleKilowatts;
        private bool _showWeightFields;
        private bool _save;

        private BaseDataModel _selectedBodyType;
        private BaseOcrDataModel _selectedColor;
        private BaseOcrDataModel _selectedEngineType;
        private BaseDataModel _selectedSteeringWheel;

        private bool isError = false;
        private bool _vinError;
        private bool _engineVolumeError;
        private bool _placesError;
        private bool _netWeightError;
        private bool _grossWeightError;
        private bool _vehicleKilowattsError;

        private IVehicleService _vehicleService;

        private ObservableCollection<BaseDataModel> _bodyTypesCollection;
        private ObservableCollection<BaseOcrDataModel> _colorsCollection;
        private ObservableCollection<BaseOcrDataModel> _engineTypesCollection;
        private ObservableCollection<BaseDataModel> _steeringWheelCollection;
        private ObservableCollection<CivilInsuranceSearchModel> _offersOneInstallmentCollection;

        #endregion
        public CivilInsuranceVehicleAdditionalInfoViewModel(IVehicleService vehicleService)
        {
            this.BodyTypesCollection = new ObservableCollection<BaseDataModel>();
            this.ColorsCollection = new ObservableCollection<BaseOcrDataModel>();
            this.EngineTypesCollection = new ObservableCollection<BaseOcrDataModel>();
            this.SteeringWheelCollection = new ObservableCollection<BaseDataModel>();
            this.OffersOneInstallmentCollection = new ObservableCollection<CivilInsuranceSearchModel>();

            this._vehicleService = vehicleService;

            Task.Run(async () => { await GetColors(); }).Wait();
            Task.Run(async () => { await GetBodyTypes(); }).Wait();
            Task.Run(async () => { await GetEngineTypes(); }).Wait();
            Task.Run(async () => { await PopulateSteeringWheelCollection(); }).Wait();
        }

        #region Collections

        public ObservableCollection<BaseDataModel> BodyTypesCollection
        {
            get => _bodyTypesCollection;
            set => SetProperty(ref _bodyTypesCollection, value);
        }

        public ObservableCollection<BaseOcrDataModel> ColorsCollection
        {
            get => _colorsCollection;
            set => SetProperty(ref _colorsCollection, value);
        }
        public ObservableCollection<BaseOcrDataModel> EngineTypesCollection
        {
            get => _engineTypesCollection;
            set => SetProperty(ref _engineTypesCollection, value);
        }

        public ObservableCollection<BaseDataModel> SteeringWheelCollection
        {
            get => _steeringWheelCollection;
            set => SetProperty(ref _steeringWheelCollection, value);
        }
        public ObservableCollection<CivilInsuranceSearchModel> OffersOneInstallmentCollection
        {
            get => _offersOneInstallmentCollection;
            set => SetProperty(ref _offersOneInstallmentCollection, value);
        }
        #endregion

        #region Props
        public RegCertificateResultModel RegCertificateModel
        {
            get { return _regCertificateModel; }
            set { SetProperty(ref _regCertificateModel, value); }
        }
        public BaseDataModel SelectedBodyType
        {
            get { return _selectedBodyType; }
            set { SetProperty(ref _selectedBodyType, value); }
        }
        public BaseOcrDataModel SelectedColor
        {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }
        }
        public BaseOcrDataModel SelectedEngineType
        {
            get { return _selectedEngineType; }
            set { SetProperty(ref _selectedEngineType, value); }
        }
        public BaseDataModel SelectedSteeringWheel
        {
            get { return _selectedSteeringWheel; }
            set { SetProperty(ref _selectedSteeringWheel, value); }
        }
        public string Vin
        {
            get { return _vin; }
            set { SetProperty(ref _vin, value); }
        }
        public string EngineVolume
        {
            get { return _engineVolume; }
            set { SetProperty(ref _engineVolume, value); }
        }
        public string Places
        {
            get { return _places; }
            set { SetProperty(ref _places, value); }
        }
        public string NetWeight
        {
            get { return _netWeight; }
            set { SetProperty(ref _netWeight, value); }
        }
        public string GrossWeight
        {
            get { return _grossWeight; }
            set { SetProperty(ref _grossWeight, value); }
        }
        public string VehicleKilowatts
        {
            get { return _vehicleKilowatts; }
            set { SetProperty(ref _vehicleKilowatts, value); }
        }
        public bool ShowWeightFields
        {
            get { return _showWeightFields; }
            set { SetProperty(ref _showWeightFields, value); }
        }
        public bool VinError
        {
            get { return _vinError; }
            set
            {
                if (_vinError != value)
                {
                    _vinError = value;
                    OnPropertyChanged(nameof(VinError));
                }
            }
        }
        public bool EngineVolumeError
        {
            get { return _engineVolumeError; }
            set
            {
                if (_engineVolumeError != value)
                {
                    _engineVolumeError = value;
                    OnPropertyChanged(nameof(EngineVolumeError));
                }
            }
        }
        public bool PlacesError
        {
            get { return _placesError; }
            set
            {
                if (_placesError != value)
                {
                    _placesError = value;
                    OnPropertyChanged(nameof(PlacesError));
                }
            }
        }
        public bool NetWeightError
        {
            get { return _netWeightError; }
            set
            {
                if (_netWeightError != value)
                {
                    _netWeightError = value;
                    OnPropertyChanged(nameof(NetWeightError));
                }
            }
        }
        public bool GrossWeightError
        {
            get { return _grossWeightError; }
            set
            {
                if (_grossWeightError != value)
                {
                    _grossWeightError = value;
                    OnPropertyChanged(nameof(GrossWeightError));
                }
            }
        }
        public bool VehicleKilowattsError
        {
            get { return _vehicleKilowattsError; }
            set
            {
                if (_vehicleKilowattsError != value)
                {
                    _vehicleKilowattsError = value;
                    OnPropertyChanged(nameof(VehicleKilowattsError));
                }
            }
        }
        public bool IsError
        {
            get { return isError; }
            set
            {
                if (isError != value)
                {
                    isError = value;
                    OnPropertyChanged(nameof(IsError));
                }
            }
        }
        public bool Save
        {
            get { return this._save; }
            set { SetProperty(ref this._save, value); }
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

                if (this.SelectedBodyType == null)
                {
                    throw new Exception(MessageConstants.BodyTypeNotSelected);
                }

                if (this.SelectedColor == null)
                {
                    throw new Exception(MessageConstants.ColorNotSelected);
                }

                if (this.SelectedEngineType == null)
                {
                    throw new Exception(MessageConstants.EngineTypeNotSelected);
                }

                if (this.SelectedSteeringWheel == null)
                {
                    throw new Exception(MessageConstants.SteeringWheelNotSelected);
                }


                if (this.Save)
                {
                    if (!string.IsNullOrEmpty(this._vehicleInfo.VehicleId) || this._vehicleInfo.VehicleId == GlobalConstants.New)
                    {
                        var saveResult = await this.EditVehicle();
                    }
                }

                var vehicleInfo = new InsuranceVehicleInfo
                {
                    VehicleId = this._vehicleInfo.VehicleId,
                    PlateNumber = this._vehicleInfo.PlateNumber.TrimEnd(),
                    RegistrationCertificateNumber = this._vehicleInfo.RegistrationCertificateNumber.TrimEnd(),
                    VehicleBrand = this._vehicleInfo.VehicleBrand,
                    VehicleModel = this._vehicleInfo.VehicleModel,
                    VehicleModelId = this._vehicleInfo.VehicleModelId,
                    FirstRegistrationDate = this._vehicleInfo.FirstRegistrationDate,
                    VehicleType = this._vehicleInfo.VehicleType,
                    VehicleTypeId = this._vehicleInfo.VehicleTypeId,
                    VehicleUsage = this._vehicleInfo.VehicleUsage,
                    VehicleUsageId = this._vehicleInfo.VehicleUsageId,
                    FirstRegistrationDateAsDateTime = this._vehicleInfo.FirstRegistrationDateAsDateTime,
                    Vin = !string.IsNullOrEmpty(this.Vin) ? this.Vin.TrimEnd() : null,
                    EngineType = this.SelectedEngineType != null ? (byte)this.SelectedEngineType.Id : (byte)0,
                    EngineTypeText = this.SelectedEngineType != null ? this.SelectedEngineType.Name : string.Empty,
                    BodyType = this.SelectedBodyType != null ? (byte)this.SelectedBodyType.Id : (byte)0,
                    Places = !string.IsNullOrEmpty(this.Places) ? byte.Parse(this.Places.TrimEnd()) : (byte)0,
                    Color = this.SelectedColor != null ? (byte)this.SelectedColor.Id : (byte)0,
                    EngineVolume = !string.IsNullOrEmpty(this.EngineVolume) ? int.Parse(this.EngineVolume.TrimEnd()) : 0,
                    NetWeight = !string.IsNullOrEmpty(this.NetWeight) ? int.Parse(this.NetWeight.TrimEnd()) : 0,
                    GrossWeight = !string.IsNullOrEmpty(this.GrossWeight) ? int.Parse(this.GrossWeight.TrimEnd()) : 0,
                    VehicleKilowatts = !string.IsNullOrEmpty(this.VehicleKilowatts) ? int.Parse(this.VehicleKilowatts.TrimEnd()) : 0,
                    SteeringWheel = this.SelectedSteeringWheel != null ? (byte)this.SelectedSteeringWheel.Id : (byte)0,
                    ColorText = this.SelectedColor != null ? this.SelectedColor.Name : null,
                    BodyTypeText = this.SelectedBodyType != null ? this.SelectedBodyType.Name : null
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"userVehicleInfo", vehicleInfo },
                        {"usualUserInfo", this._usualUserInfo},
                        {"ownerUserInfo", this._ownerUserInfo},
                        {"availableOffersCompanyNames", this._availableOffersCompanyNames},
                        {"oneInstallmentOffersCollection", this._offersOneInstallmentCollection},
                    };

                await Navigation.PushAsync<CivilInsuranceLongOffersPage>(parameters);

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

        private async Task GetUserVehicleInfo()
        {
            var httpClient = HttpClientFactory.Create();
            var userId = App.UserId;
            var jwt = App.JwtToken;
            var vehicleId = this._vehicleInfo.VehicleId;

            var vehicle = new VehicleResultModel();

            if (!string.IsNullOrEmpty(vehicleId) && vehicleId != GlobalConstants.New)
            {
                vehicle = await this._vehicleService.GetVehicleById(vehicleId, httpClient, jwt);
            }

            this.Vin = vehicle.Vin;
            this.EngineVolume = vehicle.EngineVolume != 0 ? vehicle.EngineVolume.ToString() : string.Empty;
            this.Places = vehicle.Places != 0 ? vehicle.Places.ToString() : string.Empty;
            this.VehicleKilowatts = vehicle.VehicleKilowatts != 0 ? vehicle.VehicleKilowatts.ToString() : string.Empty;

            int vehicleType = this._vehicleInfo.VehicleTypeId;

            if (vehicleType == 7 || vehicleType == 4 || vehicleType == 11 ||
                                    vehicleType == 6 || vehicleType == 10)
            {
                this.ShowWeightFields = true;

                this.GrossWeight = vehicle.GrossWeight != 0 ? vehicle.GrossWeight.ToString() : string.Empty;
                this.NetWeight = vehicle.NetWeight != 0 ? vehicle.NetWeight.ToString() : string.Empty;
            }

            var bodyType = this.BodyTypesCollection.FirstOrDefault(x => x.Id == vehicle.BodyType);
            var color = this.ColorsCollection.FirstOrDefault(x => x.Id == vehicle.Color);
            var engineType = this.EngineTypesCollection.FirstOrDefault(x => x.Id == vehicle.EngineType);
            var steeringWheel = this.SteeringWheelCollection.FirstOrDefault(x => x.Id == vehicle.SteeringWheel);

            this.SelectedBodyType = bodyType;
            this.SelectedColor = color;
            this.SelectedEngineType = engineType;
            this.SelectedSteeringWheel = steeringWheel;
        }
        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (!string.IsNullOrEmpty(this.EngineVolume))
            {
                bool successfullyParsed = int.TryParse(this.EngineVolume.TrimEnd(), out int ignoreMe);

                if (!successfullyParsed)
                {
                    this.EngineVolumeError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.OnlyDigits;
                }
            }

            if (!string.IsNullOrEmpty(this.NetWeight))
            {
                bool successfullyParsed = int.TryParse(this.NetWeight.TrimEnd(), out int ignoreMe);

                if (!successfullyParsed)
                {
                    this.NetWeightError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.OnlyDigits;
                }
            }

            if (!string.IsNullOrEmpty(this.GrossWeight))
            {
                bool successfullyParsed = int.TryParse(this.GrossWeight.TrimEnd(), out int ignoreMe);

                if (!successfullyParsed)
                {
                    this.GrossWeightError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.OnlyDigits;
                }
            }

            if (!string.IsNullOrEmpty(this.Places))
            {
                bool successfullyParsed = int.TryParse(this.Places.TrimEnd(), out int ignoreMe);

                if (!successfullyParsed)
                {
                    this.PlacesError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.OnlyDigits;
                }
            }

            if (!string.IsNullOrEmpty(this.VehicleKilowatts))
            {
                bool successfullyParsed = int.TryParse(this.VehicleKilowatts.TrimEnd(), out int ignoreMe);

                if (!successfullyParsed)
                {
                    this.VehicleKilowattsError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.OnlyDigits;
                }
            }

            if (!string.IsNullOrEmpty(this.Vin))
            {
                bool isValid = this.VinValidation();

                if (!isValid)
                {
                    this.VinError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidVin;
                }

            }

            return res;
        }
        private async Task GetColors()
        {
            var colorsCollection = new ObservableCollection<BaseOcrDataModel>();
            var colorsColl = new List<BaseOcrDataModel>();

            var colorsJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleColors);
            var colors = JsonConvert.DeserializeObject<ColorsOcrResultModel>(colorsJson);
            colorsColl = colors.Colors;

            this.ColorsCollection.Clear();

            foreach (var color in colorsColl)
            {
                colorsCollection.Add(color);
            }

            this.ColorsCollection = colorsCollection;
        }
        private async Task GetBodyTypes()
        {
            var bodyTypesCollection = new ObservableCollection<BaseDataModel>();
            var bodyTypesColl = new List<BaseDataModel>();

            var bodyTypesJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleBodyTypes);
            var bodyTypes = JsonConvert.DeserializeObject<BodyTypesResultModel>(bodyTypesJson);
            bodyTypesColl = bodyTypes.BodyTypes;

            this.BodyTypesCollection.Clear();

            foreach (var bodyType in bodyTypesColl)
            {
                bodyTypesCollection.Add(bodyType);
            }

            this.BodyTypesCollection = bodyTypesCollection;
        }
        private async Task GetEngineTypes()
        {
            var engineTypesCollection = new ObservableCollection<BaseOcrDataModel>();
            var engineTypesColl = new List<BaseOcrDataModel>();

            var engineTypesJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleEngineTypes);
            var engineTypes = JsonConvert.DeserializeObject<EngineOcrTypesResultModel>(engineTypesJson);
            engineTypesColl = engineTypes.EngineTypes;

            this.EngineTypesCollection.Clear();

            foreach (var engineType in engineTypesColl)
            {
                engineTypesCollection.Add(engineType);
            }

            this.EngineTypesCollection = engineTypesCollection;
        }
        private async Task PopulateSteeringWheelCollection()
        {
            var steeringWheelCollection = new ObservableCollection<BaseDataModel>();

            var left = new BaseDataModel { Id = 0, Name = "Ляв" };
            var right = new BaseDataModel { Id = 1, Name = "Десен" };

            this.SteeringWheelCollection.Clear();
            this.SteeringWheelCollection.Add(left);
            this.SteeringWheelCollection.Add(right);
        }
        private bool VinValidation()
        {
            if (Regex.IsMatch(this.Vin.TrimEnd(), GlobalConstants.VinRegex))
            {
                return true;
            }

            return false;
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._availableOffersCompanyNames = query.FirstOrDefault(x => x.Key == "availableOffersCompanyNames").Value as List<string>;
            this._offersOneInstallmentCollection = query.FirstOrDefault(x => x.Key == "oneInstallmentOffersCollection").Value as ObservableCollection<CivilInsuranceSearchModel>;
            this.RegCertificateModel = query.FirstOrDefault(x => x.Key == "ocrRegCertificateModel").Value as RegCertificateResultModel;
            
            if (RegCertificateModel != null)
            {
                ParseOcrData();
            }
            else
            {
                Task.Run(async () => { await GetUserVehicleInfo(); }).Wait();
            }
        }

        public void ParseOcrData()
        {
            this.Vin = String.Empty;
            this.EngineVolume = String.Empty;
            this.Places = String.Empty;
            this.NetWeight = String.Empty;
            this.GrossWeight = String.Empty;
            this.VehicleKilowatts = String.Empty;
            this.SelectedBodyType = new BaseDataModel();
            this.SelectedColor = new BaseOcrDataModel();
            this.SelectedEngineType = new BaseOcrDataModel();
            this.SelectedSteeringWheel = new BaseDataModel();

            if (RegCertificateModel.Vin != null)
            {
                this.Vin = RegCertificateModel.Vin;
            }

            if (RegCertificateModel.EngineVolume != null)
            {
                this.EngineVolume = RegCertificateModel.EngineVolume;
            }

            if (RegCertificateModel.Places != null)
            {
                this.Places = RegCertificateModel.Places;
            }

            if (RegCertificateModel.NetWeight != null)
            {
                this.NetWeight = RegCertificateModel.NetWeight;
            }

            if (RegCertificateModel.GrossWeight != null)
            {
                this.GrossWeight = RegCertificateModel.GrossWeight;
            }

            if (RegCertificateModel.VehicleKilowatts != null)
            {
                this.VehicleKilowatts = RegCertificateModel.VehicleKilowatts;
            }

            if (RegCertificateModel.Color != null)
            {
                var color = ColorsCollection.FirstOrDefault(m => m.OcrName == RegCertificateModel.Color);

                if (color != null)
                {
                    this.SelectedColor = color;
                }
            }

            if (RegCertificateModel.EngineType != null)
            {
                var engineType = EngineTypesCollection.FirstOrDefault(m => m.OcrName == RegCertificateModel.EngineType);

                if (engineType != null)
                {
                    this.SelectedEngineType = engineType;
                }
            }
        }

        private async Task<VehicleResultModel> EditVehicle()
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new EditVehicleInput
            {
                RegistrationCertificateNumber = this._vehicleInfo.RegistrationCertificateNumber.TrimEnd(),
                VehiclePlateNumber = this._vehicleInfo.PlateNumber.TrimEnd(),
                UserId = App.UserId,
                ModelId = this._vehicleInfo.VehicleModelId,
                Model = this._vehicleInfo.VehicleModel,
                BrandId = this._vehicleInfo.VehicleBrandId,
                Brand = this._vehicleInfo.VehicleBrand,
                VehicleTypeId = this._vehicleInfo.VehicleTypeId,
                VehicleType = this._vehicleInfo.VehicleType,
                VehicleUsageId = this._vehicleInfo.VehicleUsageId,
                VehicleUsage = this._vehicleInfo.VehicleUsage,
                FirstRegistrationDate = this._vehicleInfo.FirstRegistrationDateAsDateTime,
                VehicleId = this._vehicleInfo.VehicleId,
                Vin = !string.IsNullOrEmpty(this.Vin) ? this.Vin.TrimEnd() : null,
                EngineType = this.SelectedEngineType != null ? (byte)this.SelectedEngineType.Id : (byte)0,
                BodyType = this.SelectedBodyType != null ? (byte)this.SelectedBodyType.Id : (byte)0,
                Places = !string.IsNullOrEmpty(this.Places) ? byte.Parse(this.Places.TrimEnd()) : (byte)0,
                Color = this.SelectedColor != null ? (byte)this.SelectedColor.Id : (byte)0,
                EngineVolume = !string.IsNullOrEmpty(this.EngineVolume) ? int.Parse(this.EngineVolume.TrimEnd()) : 0,
                NetWeight = !string.IsNullOrEmpty(this.NetWeight) ? int.Parse(this.NetWeight.TrimEnd()) : 0,
                GrossWeight = !string.IsNullOrEmpty(this.GrossWeight) ? int.Parse(this.GrossWeight.TrimEnd()) : 0,
                VehicleKilowatts = !string.IsNullOrEmpty(this.VehicleKilowatts) ? int.Parse(this.VehicleKilowatts.TrimEnd()) : 0,
                SteeringWheel = this.SelectedSteeringWheel != null ? (byte)this.SelectedSteeringWheel.Id : (byte)0
            };

            var result = await this._vehicleService.EditVehicle(req, httpClient, jwt);

            return result;
        }

        #endregion
    }
}
