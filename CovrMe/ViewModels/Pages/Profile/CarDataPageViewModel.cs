using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Request;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
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

namespace CovrMe.ViewModels.Pages.Profile
{
    public partial class CarDataPageViewModel : BaseViewModel
    {
        #region Fields
        private string _registrationCertificateNumber;
        private DateTime _firstRegistrationDate;
        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;
        private string _vehiclePlateNumber;
        private string _vin;
        private string _engineVolume;
        private string _places;
        private string _netWeight;
        private string _grossWeight;
        private string _vehicleKilowatts;
        private bool _showWeightFields;
        private UserVehiclesPickerModel _selecteUserVehicle;
        private BaseDataModel _selectedVehicleBrand;
        private BaseDataModel _selectedVehicleModel;
        private BaseDataModel _selectedVehicleType;
        private BaseDataModel _selectedVehicleUsage;

        private BaseDataModel _selectedBodyType;
        private BaseDataModel _selectedColor;
        private BaseDataModel _selectedEngineType;
        private BaseDataModel _selectedSteeringWheel;

        private bool isError = false;

        private bool _vinError;
        private bool _engineVolumeError;
        private bool _placesError;
        private bool _netWeightError;
        private bool _grossWeightError;
        private bool _vehicleKilowattsError;
        private bool _registrationCertificateNumberError;
        private bool _firstRegistrationDateError;
        private bool _vehiclePlateNumberError;
        private bool _isUpdatingCollection = false;
        private bool _isUpdatingOnLoading = false;

        //services
        private IVehicleService _vehicleService;

        //collections
        private ObservableCollection<UserVehiclesPickerModel> _userVehiclesCollection;
        private ObservableCollection<BaseDataModel> _vehicleBrandsCollection;
        private ObservableCollection<BaseDataModel> _vehiclesModelsCollection;
        private ObservableCollection<BaseDataModel> _vehiclesTypesCollection;
        private ObservableCollection<BaseDataModel> _vehicleUsagesCollection;
        private ObservableCollection<BaseDataModel> _bodyTypesCollection;
        private ObservableCollection<BaseDataModel> _colorsCollection;
        private ObservableCollection<BaseDataModel> _engineTypesCollection;
        private ObservableCollection<BaseDataModel> _steeringWheelCollection;
        #endregion

        public CarDataPageViewModel(IVehicleService vehicleService)
        {
            this._vehicleService = vehicleService;
            this.FirstRegistrationDate = DateTime.Now;
            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1960");
            this.FirstRegistrationDate = DateTime.Now;

            this.UserVehiclesCollection = new ObservableCollection<UserVehiclesPickerModel>();
            this.VehicleBrandsCollection = new ObservableCollection<BaseDataModel>();
            this.VehiclesModelsCollection = new ObservableCollection<BaseDataModel>();
            this.VehiclesTypesCollection = new ObservableCollection<BaseDataModel>();
            this.VehicleUsagesCollection = new ObservableCollection<BaseDataModel>();
            this.BodyTypesCollection = new ObservableCollection<BaseDataModel>();
            this.ColorsCollection = new ObservableCollection<BaseDataModel>();
            this.EngineTypesCollection = new ObservableCollection<BaseDataModel>();
            this.SteeringWheelCollection = new ObservableCollection<BaseDataModel>();

            Task.Run(async () => { await GetColors(); }).Wait();
            Task.Run(async () => { await GetBodyTypes(); }).Wait();
            Task.Run(async () => { await GetEngineTypes(); }).Wait();
            Task.Run(async () => { await PopulateSteeringWheelCollection(); }).Wait();
            Task.Run(async () => { await GetVehicleBrands(); }).Wait();
            Task.Run(async () => { await GetVehicleTypes(); }).Wait();
            Task.Run(async () => { await GetVehicleUsages(); }).Wait();
            Task.Run(async () => { await GetUserVehicles(); }).Wait();
        }

        #region Collections

        public ObservableCollection<UserVehiclesPickerModel> UserVehiclesCollection
        {
            get => _userVehiclesCollection;
            set => SetProperty(ref _userVehiclesCollection, value);
        }

        public ObservableCollection<BaseDataModel> VehicleBrandsCollection
        {
            get => _vehicleBrandsCollection;
            set => SetProperty(ref _vehicleBrandsCollection, value);
        }

        public ObservableCollection<BaseDataModel> VehiclesModelsCollection
        {
            get => _vehiclesModelsCollection;
            set => SetProperty(ref _vehiclesModelsCollection, value);
        }

        public ObservableCollection<BaseDataModel> VehiclesTypesCollection
        {
            get => _vehiclesTypesCollection;
            set => SetProperty(ref _vehiclesTypesCollection, value);
        }

        public ObservableCollection<BaseDataModel> VehicleUsagesCollection
        {
            get => _vehicleUsagesCollection;
            set => SetProperty(ref _vehicleUsagesCollection, value);
        }

        public ObservableCollection<BaseDataModel> BodyTypesCollection
        {
            get => _bodyTypesCollection;
            set => SetProperty(ref _bodyTypesCollection, value);
        }

        public ObservableCollection<BaseDataModel> ColorsCollection
        {
            get => _colorsCollection;
            set => SetProperty(ref _colorsCollection, value);
        }
        public ObservableCollection<BaseDataModel> EngineTypesCollection
        {
            get => _engineTypesCollection;
            set => SetProperty(ref _engineTypesCollection, value);
        }

        public ObservableCollection<BaseDataModel> SteeringWheelCollection
        {
            get => _steeringWheelCollection;
            set => SetProperty(ref _steeringWheelCollection, value);
        }

        #endregion

        #region Props

        public BaseDataModel SelectedBodyType
        {
            get { return _selectedBodyType; }
            set { SetProperty(ref _selectedBodyType, value); }
        }

        public BaseDataModel SelectedColor
        {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }
        }

        public BaseDataModel SelectedEngineType
        {
            get { return _selectedEngineType; }
            set { SetProperty(ref _selectedEngineType, value); }
        }

        public BaseDataModel SelectedSteeringWheel
        {
            get { return _selectedSteeringWheel; }
            set { SetProperty(ref _selectedSteeringWheel, value); }
        }

        public UserVehiclesPickerModel SelecteUserVehicle
        {
            get { return _selecteUserVehicle; }
            set { SetProperty(ref _selecteUserVehicle, value); }
        }

        public BaseDataModel SelectedVehicleBrand
        {
            get { return _selectedVehicleBrand; }
            set { SetProperty(ref _selectedVehicleBrand, value); }
        }

        public BaseDataModel SelectedVehicleModel
        {
            get { return _selectedVehicleModel; }
            set { SetProperty(ref _selectedVehicleModel, value); }
        }

        public BaseDataModel SelectedVehicleType
        {
            get { return _selectedVehicleType; }
            set { SetProperty(ref _selectedVehicleType, value); }
        }

        public BaseDataModel SelectedVehicleUsage
        {
            get { return _selectedVehicleUsage; }
            set { SetProperty(ref _selectedVehicleUsage, value); }
        }

        public string RegistrationCertificateNumber
        {
            get { return _registrationCertificateNumber; }
            set { SetProperty(ref _registrationCertificateNumber, value); }
        }

        public string VehiclePlateNumber
        {
            get { return _vehiclePlateNumber; }
            set { SetProperty(ref _vehiclePlateNumber, value); }
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

        public DateTime FirstRegistrationDate
        {
            get { return _firstRegistrationDate; }
            set { SetProperty(ref _firstRegistrationDate, value); }
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

        public bool ShowWeightFields
        {
            get { return _showWeightFields; }
            set { SetProperty(ref _showWeightFields, value); }
        }

        public bool IsUpdatingCollection
        {
            get { return _isUpdatingCollection; }
            set { SetProperty(ref _isUpdatingCollection, value); }
        }

        public bool IsUpdatingOnLoading
        {
            get { return _isUpdatingOnLoading; }
            set { SetProperty(ref _isUpdatingOnLoading, value); }
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

        public bool RegistrationCertificateNumberError
        {
            get { return _registrationCertificateNumberError; }
            set
            {
                if (_registrationCertificateNumberError != value)
                {
                    _registrationCertificateNumberError = value;
                    OnPropertyChanged(nameof(RegistrationCertificateNumberError));
                }
            }
        }

        public bool FirstRegistrationDateError
        {
            get { return _firstRegistrationDateError; }
            set
            {
                if (_firstRegistrationDateError != value)
                {
                    _firstRegistrationDateError = value;
                    OnPropertyChanged(nameof(FirstRegistrationDateError));
                }
            }
        }

        public bool VehiclePlateNumberError
        {
            get { return _vehiclePlateNumberError; }
            set
            {
                if (_vehiclePlateNumberError != value)
                {
                    _vehiclePlateNumberError = value;
                    OnPropertyChanged(nameof(VehiclePlateNumberError));
                }
            }
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

        #endregion

        #region Commands
        [RelayCommand]
        private async void Save()
        {
            var result = new VehicleResultModel();
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

                if (this.SelectedVehicleType == null)
                {
                    throw new Exception(MessageConstants.VehicleTypeNotSelected);
                }

                if (this.SelectedVehicleModel == null)
                {
                    throw new Exception(MessageConstants.VehicleModelNotSelected);
                }

                if (this.SelectedVehicleUsage == null)
                {
                    throw new Exception(MessageConstants.VehicleUsageNotSelected);
                }


                string message = string.Empty;

                if (this.SelecteUserVehicle == null || this.SelecteUserVehicle.VehicleId == GlobalConstants.New)
                {
                    result = await this.AddVehicle();
                    message = MessageConstants.AddVehicleSuccess;
                }
                else
                {
                    result = await this.EditVehicle();
                    message = MessageConstants.EditVehicleSuccess;
                }

                if (!string.IsNullOrEmpty(result.Id))
                {
                    this.UpdateVehicleCollection(result);
                    await this.ShowSuccessToast(message);
                }
                else if (result.ExceptionType == (int)ExceptionsTypeEnum.SqlUniqueErrorCode)
                {
                    throw new Exception(result.Message != null ? result.Message : MessageConstants.GeneralError);
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
                this.IsUpdatingCollection = false;
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

            if (string.IsNullOrEmpty(this.RegistrationCertificateNumber) || !RegistrationCertificateNumberValidation())
            {
                this.RegistrationCertificateNumberError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.InvalidRegistrationCertificateNumber;
            }

            if (string.IsNullOrEmpty(this.VehiclePlateNumber) || !ValidateCarNumber(this.VehiclePlateNumber))
            {
                this.VehiclePlateNumberError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.InvalidVehiclePlateNumber;
            }

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
        private bool ValidateCarNumber(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.CarNumberRegex))
            {
                return true;
            }

            return false;
        }
        private bool RegistrationCertificateNumberValidation()
        {
            if (Regex.IsMatch(this.RegistrationCertificateNumber.TrimEnd(), GlobalConstants.OnlyNumbersRegex))
            {
                return true;
            }

            return false;
        }

        private async Task GetVehicleBrands()
        {
            var vehiclesBrands = new ObservableCollection<BaseDataModel>();
            var brandsColl = new List<BaseDataModel>();

            var vehicleBrandsJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleBrands);
            var brands = JsonConvert.DeserializeObject<GetVehicleBrandsResultModel>(vehicleBrandsJson);
            brandsColl = brands.Brands;

            foreach (var brand in brandsColl.OrderBy(x => x.Name))
            {
                vehiclesBrands.Add(brand);
            }

            this.VehicleBrandsCollection = vehiclesBrands;

        }
        public async Task GetVehicleModels(BaseDataModel selectedVehicleBrand)
        {
            try
            {
                var vehiclesModels = new ObservableCollection<BaseDataModel>();

                var httpClient = HttpClientFactory.Create();
                string jwt = App.JwtToken;
                var modelsJson = await this._vehicleService.GetVehicleModels(selectedVehicleBrand.Name, httpClient, jwt);

                var models = JsonConvert.DeserializeObject<GetVehicleModelsResultModel>(modelsJson);
                this.VehiclesModelsCollection.Clear();

                foreach (var model in models.Models)
                {
                    vehiclesModels.Add(model);
                }

                this.VehiclesModelsCollection = vehiclesModels;

            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
        }
        private async Task GetVehicleTypes()
        {
            var vehicleTypes = new ObservableCollection<BaseDataModel>();
            var typesColl = new List<BaseDataModel>();

            var vehicleTypesJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleTypes);
            var types = JsonConvert.DeserializeObject<GetVehicleTypesResultModel>(vehicleTypesJson);
            typesColl = types.Types;

            this.VehiclesTypesCollection.Clear();

            foreach (var type in typesColl)
            {
                vehicleTypes.Add(type);
            }

            this.VehiclesTypesCollection = vehicleTypes;
        }
        private async Task GetVehicleUsages()
        {
            var vehicleUsages = new ObservableCollection<BaseDataModel>();
            var usagesColl = new List<BaseDataModel>();

            var vehicleUsagesJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleUsages);
            var usages = JsonConvert.DeserializeObject<GetVehicleUsagesResultModel>(vehicleUsagesJson);
            usagesColl = usages.Usages;

            this.VehicleUsagesCollection.Clear();

            foreach (var type in usages.Usages.OrderBy(x => x.Id))
            {
                vehicleUsages.Add(type);
            }

            this.VehicleUsagesCollection = vehicleUsages;
        }
        private async Task GetUserVehicles()
        {
            if (!string.IsNullOrEmpty(App.UserId))
            {
                var userVehicles = new ObservableCollection<UserVehiclesPickerModel>();

                var httpClient = HttpClientFactory.Create();
                var userId = App.UserId;
                var jwt = App.JwtToken;
                var vehicles = await this._vehicleService.GetUserVehicles(userId, httpClient, jwt);

                this.UserVehiclesCollection.Clear();

                var newVehicle = new UserVehiclesPickerModel
                {
                    VehicleId = GlobalConstants.New,
                    VehicleBrandAndModel = GlobalConstants.New,
                    FirstRegistrationDate = DateTime.Now
                };

                userVehicles.Add(newVehicle);

                foreach (var vehicle in vehicles.Vehicles)
                {
                    string vehicleId = vehicle.Id;
                    string brandModel = vehicle.Brand + " " + vehicle.Model;

                    var current = new UserVehiclesPickerModel
                    {
                        VehicleId = vehicleId,
                        VehicleBrandAndModel = brandModel,
                        RegistrationCertificateNumber = vehicle.RegistrationCertificateNumber,
                        VehiclePlateNumber = vehicle.PlateNumber,
                        VehicleModelId = vehicle.ModelId,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        VehicleUsageId = vehicle.VehicleUsageId,
                        VehicleBrandId = vehicle.BrandId,
                        FirstRegistrationDate = vehicle.FirstRegistrationDate,
                        Vin = vehicle.Vin,
                        EngineVolume = vehicle.EngineVolume,
                        BatteryCapacity = vehicle.BatteryCapacity,
                        BodyTypeId = vehicle.BodyType,
                        Places = vehicle.Places,
                        ColorId = vehicle.Color,
                        EngineTypeId = vehicle.EngineType,
                        NetWeight = vehicle.NetWeight,
                        GrossWeight = vehicle.GrossWeight,
                        SteeringWheelId = vehicle.SteeringWheel,
                        VehicleKilowatts = vehicle.VehicleKilowatts
                    };

                    userVehicles.Add(current);
                }

                UserVehiclesCollection = userVehicles;

                if (this.UserVehiclesCollection.Count > 1)
                {
                    this.IsUpdatingOnLoading = true;
                    this.SelecteUserVehicle = UserVehiclesCollection.FirstOrDefault(x => x.VehicleId != GlobalConstants.New);
                    this.RegistrationCertificateNumber = this.SelecteUserVehicle.RegistrationCertificateNumber;
                    this.VehiclePlateNumber = this.SelecteUserVehicle.VehiclePlateNumber;
                    this.FirstRegistrationDate = this.SelecteUserVehicle.FirstRegistrationDate.HasValue ? this.SelecteUserVehicle.FirstRegistrationDate.Value : DateTime.Now;
                    this.Vin = this.SelecteUserVehicle.Vin;
                    this.EngineVolume = this.SelecteUserVehicle.EngineVolume != 0 ? this.SelecteUserVehicle.EngineVolume.ToString() : string.Empty;
                    this.Places = this.SelecteUserVehicle.Places != 0 ? this.SelecteUserVehicle.Places.ToString() : string.Empty;
                    this.VehicleKilowatts = this.SelecteUserVehicle.VehicleKilowatts != 0 ? this.SelecteUserVehicle.VehicleKilowatts.ToString() : string.Empty;

                    var vehicleType = this.VehiclesTypesCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleTypeId);

                    if (vehicleType != null)
                    {
                        if (vehicleType.Id == 7 || vehicleType.Id == 4 || vehicleType.Id == 11 ||
                            vehicleType.Id == 6 || vehicleType.Id == 10)
                        {
                            this.ShowWeightFields = true;

                            this.GrossWeight = this.SelecteUserVehicle.GrossWeight != 0 ? this.SelecteUserVehicle.GrossWeight.ToString() : string.Empty;
                            this.NetWeight = this.SelecteUserVehicle.NetWeight != 0 ? this.SelecteUserVehicle.NetWeight.ToString() : string.Empty;
                        }
                    }

                    var bodyType = this.BodyTypesCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.BodyTypeId);
                    var color = this.ColorsCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.ColorId);
                    var engineType = this.EngineTypesCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.EngineTypeId);
                    var steeringWheel = this.SteeringWheelCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.SteeringWheelId);

                    var vehicleUsage = this.VehicleUsagesCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleUsageId);
                    var vehicleBrand = this.VehicleBrandsCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleBrandId);
                    await GetVehicleModels(vehicleBrand);
                    var vehicleModel = this.VehiclesModelsCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleModelId);

                    this.SelectedVehicleBrand = vehicleBrand;
                    this.SelectedVehicleType = vehicleType;
                    this.SelectedVehicleUsage = vehicleUsage;
                    this.SelectedVehicleModel = vehicleModel;

                    this.SelectedBodyType = bodyType;
                    this.SelectedColor = color;
                    this.SelectedEngineType = engineType;
                    this.SelectedSteeringWheel = steeringWheel;
                }
            }
        }
        public async Task ChangeUserVehicle(UserVehiclesPickerModel vehicle)
        {
            try
            {
                ShowLoading();

                this.RegistrationCertificateNumber = vehicle.RegistrationCertificateNumber;
                this.VehiclePlateNumber = vehicle.VehiclePlateNumber;
                this.FirstRegistrationDate = vehicle.FirstRegistrationDate.HasValue ? vehicle.FirstRegistrationDate.Value : DateTime.Now;
                this.Vin = vehicle.Vin;
                this.EngineVolume = vehicle.EngineVolume != 0 ? vehicle.EngineVolume.ToString() : string.Empty;
                this.Places = vehicle.Places != 0 ? vehicle.Places.ToString() : string.Empty;
                this.VehicleKilowatts = vehicle.VehicleKilowatts != 0 ? vehicle.VehicleKilowatts.ToString() : string.Empty;

                var vehicleType = this.VehiclesTypesCollection.FirstOrDefault(x => x.Id == vehicle.VehicleTypeId);
                var vehicleUsage = this.VehicleUsagesCollection.FirstOrDefault(x => x.Id == vehicle.VehicleUsageId);
                var vehicleBrand = this.VehicleBrandsCollection.FirstOrDefault(x => x.Id == vehicle.VehicleBrandId);

                if (vehicleType != null)
                {
                    if (vehicleType.Id == 7 || vehicleType.Id == 4 || vehicleType.Id == 11 ||
                        vehicleType.Id == 6 || vehicleType.Id == 10)
                    {
                        this.ShowWeightFields = true;

                        this.GrossWeight = vehicle.GrossWeight != 0 ? vehicle.GrossWeight.ToString() : string.Empty;
                        this.NetWeight = vehicle.NetWeight != 0 ? vehicle.NetWeight.ToString() : string.Empty;
                    }
                }

                var bodyType = this.BodyTypesCollection.FirstOrDefault(x => x.Id == vehicle.BodyTypeId);
                var color = this.ColorsCollection.FirstOrDefault(x => x.Id == vehicle.ColorId);
                var engineType = this.EngineTypesCollection.FirstOrDefault(x => x.Id == vehicle.EngineTypeId);
                var steeringWheel = this.SteeringWheelCollection.FirstOrDefault(x => x.Id == vehicle.SteeringWheelId);

                await GetVehicleModels(vehicleBrand);
                var vehicleModel = this.VehiclesModelsCollection.FirstOrDefault(x => x.Id == vehicle.VehicleModelId);


                this.SelectedVehicleBrand = vehicleBrand;
                this.SelectedVehicleType = vehicleType;
                this.SelectedVehicleUsage = vehicleUsage;
                this.SelectedVehicleModel = vehicleModel;

                this.SelectedBodyType = bodyType;
                this.SelectedColor = color;
                this.SelectedEngineType = engineType;
                this.SelectedSteeringWheel = steeringWheel;

                this.SelecteUserVehicle = vehicle;
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
            }
        }
        private async Task<VehicleResultModel> AddVehicle()
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new AddVehicleInput
            {
                RegistrationCertificateNumber = this.RegistrationCertificateNumber.TrimEnd(),
                VehiclePlateNumber = this.VehiclePlateNumber.TrimEnd(),
                UserId = App.UserId,
                ModelId = this.SelectedVehicleModel.Id,
                Model = this.SelectedVehicleModel.Name,
                BrandId = this.SelectedVehicleBrand.Id,
                Brand = this.SelectedVehicleBrand.Name,
                VehicleTypeId = this.SelectedVehicleType.Id,
                VehicleType = this.SelectedVehicleType.Name,
                VehicleUsageId = this.SelectedVehicleUsage.Id,
                VehicleUsage = this.SelectedVehicleUsage.Name,
                FirstRegistrationDate = this.FirstRegistrationDate,
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

            var result = await this._vehicleService.AddVehicle(req, httpClient, jwt);

            return result;
        }
        private async Task<VehicleResultModel> EditVehicle()
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new EditVehicleInput
            {
                RegistrationCertificateNumber = this.RegistrationCertificateNumber.TrimEnd(),
                VehiclePlateNumber = this.VehiclePlateNumber.TrimEnd(),
                UserId = App.UserId,
                ModelId = this.SelectedVehicleModel.Id,
                Model = this.SelectedVehicleModel.Name,
                BrandId = this.SelectedVehicleBrand.Id,
                Brand = this.SelectedVehicleBrand.Name,
                VehicleTypeId = this.SelectedVehicleType.Id,
                VehicleType = this.SelectedVehicleType.Name,
                VehicleUsageId = this.SelectedVehicleUsage.Id,
                VehicleUsage = this.SelectedVehicleUsage.Name,
                FirstRegistrationDate = this.FirstRegistrationDate,
                VehicleId = this.SelecteUserVehicle.VehicleId,
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
        private async Task ShowSuccessToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string clipBoardText = message;

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(clipBoardText, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
        private async void UpdateVehicleCollection(VehicleResultModel vehicle)
        {
            this.IsUpdatingCollection = true;
            var current = this.UserVehiclesCollection.FirstOrDefault(x => x.VehicleId.ToLower() == vehicle.Id.ToLower());

            if (current != null)
            {
                this.UserVehiclesCollection.Remove(current);
            }

            string vehicleId = vehicle.Id;
            string brandModel = vehicle.Brand + " " + vehicle.Model;

            current = new UserVehiclesPickerModel
            {
                VehicleId = vehicleId,
                VehicleBrandAndModel = brandModel,
                RegistrationCertificateNumber = vehicle.RegistrationCertificateNumber,
                VehiclePlateNumber = vehicle.PlateNumber,
                VehicleModelId = vehicle.ModelId,
                VehicleTypeId = vehicle.VehicleTypeId,
                VehicleUsageId = vehicle.VehicleUsageId,
                VehicleBrandId = vehicle.BrandId,
                FirstRegistrationDate = vehicle.FirstRegistrationDate.HasValue ? vehicle.FirstRegistrationDate.Value : DateTime.Now,
                Vin = vehicle.Vin,
                EngineVolume = vehicle.EngineVolume,
                BatteryCapacity = vehicle.BatteryCapacity,
                BodyTypeId = vehicle.BodyType,
                Places = vehicle.Places,
                ColorId = vehicle.Color,
                EngineTypeId = vehicle.EngineType,
                NetWeight = vehicle.NetWeight,
                GrossWeight = vehicle.GrossWeight,
                SteeringWheelId = vehicle.SteeringWheel,
                VehicleKilowatts = vehicle.VehicleKilowatts
            };

            this.UserVehiclesCollection.Add(current);
            await this.ChangeUserVehicle(current);
        }
        private async Task GetColors()
        {
            var colorsCollection = new ObservableCollection<BaseDataModel>();
            var colorsColl = new List<BaseDataModel>();

            var colorsJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleColors);
            var colors = JsonConvert.DeserializeObject<ColorsResultModel>(colorsJson);
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
            var engineTypesCollection = new ObservableCollection<BaseDataModel>();
            var engineTypesColl = new List<BaseDataModel>();

            var engineTypesJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleEngineTypes);
            var engineTypes = JsonConvert.DeserializeObject<EngineTypesResultModel>(engineTypesJson);
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

            var left = new BaseDataModel { Id = 1, Name = "Ляв" };
            var right = new BaseDataModel { Id = 2, Name = "Десен" };

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

        #endregion
    }
}
