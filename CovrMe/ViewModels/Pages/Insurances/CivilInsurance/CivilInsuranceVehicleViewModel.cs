using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Pickers;
using CovrMe.Models.Vehicles.Request;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceVehicleViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private string _registrationCertificateNumber;
        private DateTime _firstRegistrationDate;
        private DateTime _minPickerDate;
        private DateTime _maxPickerDate;
        private string _vehiclePlateNumber;
        private UserVehiclesPickerModel _selecteUserVehicle;
        private BaseDataModel _selectedVehicleBrand;
        private BaseDataModel _selectedVehicleModel;
        private BaseDataModel _selectedVehicleType;
        private BaseDataModel _selectedVehicleUsage;

        private bool isError = false;

        private bool _registrationCertificateNumberError;
        private bool _firstRegistrationDateError;
        private bool _vehiclePlateNumberError;

        private bool _isUpdatingCollection = false;
        private bool _save;
        private bool _isUpdatingOnLoading = false;
        private RegCertificateResultModel _regCertificateModel;

        //services
        private IVehicleService _vehicleService;
        private IInsuranceService _insuranceService;

        //collections
        private ObservableCollection<UserVehiclesPickerModel> _userVehiclesCollection;
        private ObservableCollection<BaseDataModel> _vehicleBrandsCollection;
        private ObservableCollection<BaseDataModel> _vehiclesModelsCollection;
        private ObservableCollection<BaseOcrDataModel> _vehiclesTypesCollection;
        private ObservableCollection<BaseDataModel> _vehicleUsagesCollection;

        #endregion
        public CivilInsuranceVehicleViewModel(IVehicleService vehicleService, IInsuranceService insuranceService)
        {
            this._vehicleService = vehicleService;
            this._insuranceService = insuranceService;

            this.FirstRegistrationDate = DateTime.Now;
            this.MaxPickerDate = DateTime.Now;
            this.MinPickerDate = DateTime.Parse("01/01/1960");

            this.UserVehiclesCollection = new ObservableCollection<UserVehiclesPickerModel>();
            this.VehicleBrandsCollection = new ObservableCollection<BaseDataModel>();
            this.VehiclesModelsCollection = new ObservableCollection<BaseDataModel>();
            this.VehiclesTypesCollection = new ObservableCollection<BaseOcrDataModel>();
            this.VehicleUsagesCollection = new ObservableCollection<BaseDataModel>();

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
        public ObservableCollection<BaseOcrDataModel> VehiclesTypesCollection
        {
            get => _vehiclesTypesCollection;
            set => SetProperty(ref _vehiclesTypesCollection, value);
        }
        public ObservableCollection<BaseDataModel> VehicleUsagesCollection
        {
            get => _vehicleUsagesCollection;
            set => SetProperty(ref _vehicleUsagesCollection, value);
        }

        #endregion

        #region Props
        public RegCertificateResultModel RegCertificateModel
        {
            get { return _regCertificateModel; }
            set { SetProperty(ref _regCertificateModel, value); }
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
        public bool Save
        {
            get { return this._save; }
            set { SetProperty(ref this._save, value); }
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

                var insuranceAllowed = await this.IsInsuranceAllowed();

                if (insuranceAllowed != null && insuranceAllowed.IsForbidden)
                {
                    var ExistingCivilInsuranceParameters = new Dictionary<string, object>
                    {
                        {"endDate", insuranceAllowed.EndDate.HasValue ? insuranceAllowed.EndDate.Value.ToString("dd.MM.yyyy") : string.Empty },
                    };

                    await Navigation.PushAsync<ExistingCivilInsurancePage>(ExistingCivilInsuranceParameters);
                }
                else
                {
                    if (this.Save)
                    {
                        var saveResult = new VehicleResultModel();

                        if (this.SelecteUserVehicle == null || this.SelecteUserVehicle.VehicleId == GlobalConstants.New)
                        {
                            saveResult = await this.AddVehicle();
                        }
                        else
                        {
                            saveResult = await this.EditVehicle();
                        }

                        if (saveResult.ExceptionType == (int)ExceptionsTypeEnum.SqlUniqueErrorCode)
                        {
                            throw new Exception(saveResult.Message != null ? saveResult.Message : MessageConstants.GeneralError);
                        }

                        if (saveResult.ExceptionType == (int)ExceptionsTypeEnum.SqlUniqueErrorCode)
                        {
                            throw new Exception(saveResult.Message != null ? saveResult.Message : MessageConstants.GeneralError);
                        }

                        if (string.IsNullOrEmpty(saveResult.Id))
                        {
                            throw new Exception(MessageConstants.GeneralError);
                        }

                        await this.UpdateVehicleCollection(saveResult);
                    }

                    var vehicleInfo = new InsuranceVehicleInfo
                    {
                        VehicleId = this.SelecteUserVehicle != null ? this.SelecteUserVehicle.VehicleId : GlobalConstants.New,
                        PlateNumber = this._vehiclePlateNumber.TrimEnd(),
                        RegistrationCertificateNumber = this._registrationCertificateNumber.TrimEnd(),
                        VehicleBrand = this.SelectedVehicleBrand.Name,
                        VehicleBrandId = this.SelectedVehicleBrand.Id,
                        VehicleModel = this.SelectedVehicleModel.Name,
                        VehicleModelId = this.SelectedVehicleModel.Id,
                        FirstRegistrationDate = this.FirstRegistrationDate.ToString("dd.MM.yyyy"),
                        VehicleType = this.SelectedVehicleType.Name,
                        VehicleTypeId = this.SelectedVehicleType.Id,
                        VehicleUsage = this.SelectedVehicleUsage.Name,
                        VehicleUsageId = this.SelectedVehicleUsage.Id,
                        FirstRegistrationDateAsDateTime = this.FirstRegistrationDate
                    };

                    var parameters = new Dictionary<string, object>
                    {
                        {"userVehicleInfo", vehicleInfo }
                    };

                    if (RegCertificateModel != null)
                    {
                        parameters.Add("ocrRegCertificateModel", RegCertificateModel);
                    }

                    await Navigation.PushAsync<CivilInsuranceOwnerPage>(parameters);
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
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            RegCertificateModel = query.FirstOrDefault(x => x.Key == "ocrRegCertificateModel").Value as RegCertificateResultModel;
            if (RegCertificateModel != null)
            {
                ParseOcrData();
            }
        }

        private async void ParseOcrData()
        {
            this.RegistrationCertificateNumber = String.Empty;
            this._firstRegistrationDate = DateTime.Now;
            this.VehiclePlateNumber = String.Empty;
            this.SelecteUserVehicle = new UserVehiclesPickerModel();
            this.SelectedVehicleBrand = new BaseDataModel();
            this.SelectedVehicleModel = new BaseDataModel();
            this.SelectedVehicleType = new BaseDataModel();
            this.SelectedVehicleUsage = new BaseDataModel();
            this.SelectedVehicleUsage = null;
            this.SelecteUserVehicle = null;

            var brand = VehicleBrandsCollection.FirstOrDefault(b => b.Name == RegCertificateModel.Brand);
            if (brand != null)
            {
                this.SelectedVehicleBrand = brand;
                await GetVehicleModels(brand);
            }

            if (RegCertificateModel.Model != null)
            {
                var model = VehiclesModelsCollection.FirstOrDefault(m => m.Name == RegCertificateModel.Model);
                if (model != null)
                {
                    this.SelectedVehicleModel = model;
                }
            }

            if (RegCertificateModel.VehicleType != null)
            {
                var type = VehiclesTypesCollection.FirstOrDefault(t => t.OcrName == RegCertificateModel.VehicleType);
                if (type != null)
                {
                    this.SelectedVehicleType = type;

                }
            }

            if (RegCertificateModel.RegistrationCertificateNumber != null)
            {
                this.RegistrationCertificateNumber = RegCertificateModel.RegistrationCertificateNumber;
            }

            if (RegCertificateModel.PlateNumber != null)
            {
                this.VehiclePlateNumber = RegCertificateModel.PlateNumber;
            }

            if (RegCertificateModel.FirstRegistrationDate != null)
            {
                this.FirstRegistrationDate = (DateTime)RegCertificateModel.FirstRegistrationDate;
            }
        }

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

            var str = "";
            foreach (var brand in brandsColl.OrderBy(x => x.Name))
            {
                str += brand.Name + ", ";
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
                if (ex != null)
                {
                    await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
                }
            }
        }
        private async Task GetVehicleTypes()
        {
            var vehicleTypes = new ObservableCollection<BaseOcrDataModel>();
            var typesColl = new List<BaseOcrDataModel>();

            var vehicleTypesJson = await AssetsHelper.LoadAssets(GlobalConstants.VehicleTypes);
            var types = JsonConvert.DeserializeObject<GetVehicleOcrTypesResultModel>(vehicleTypesJson);
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
                        FirstRegistrationDate = vehicle.FirstRegistrationDate
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

                    var vehicleType = this.VehiclesTypesCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleTypeId);
                    var vehicleUsage = this.VehicleUsagesCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleUsageId);
                    var vehicleBrand = this.VehicleBrandsCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleBrandId);
                    await GetVehicleModels(vehicleBrand);
                    var vehicleModel = this.VehiclesModelsCollection.FirstOrDefault(x => x.Id == this.SelecteUserVehicle.VehicleModelId);

                    this.SelectedVehicleBrand = vehicleBrand;
                    this.SelectedVehicleType = vehicleType;
                    this.SelectedVehicleUsage = vehicleUsage;
                    this.SelectedVehicleModel = vehicleModel;
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

                var vehicleType = this.VehiclesTypesCollection.FirstOrDefault(x => x.Id == vehicle.VehicleTypeId);
                var vehicleUsage = this.VehicleUsagesCollection.FirstOrDefault(x => x.Id == vehicle.VehicleUsageId);
                var vehicleBrand = this.VehicleBrandsCollection.FirstOrDefault(x => x.Id == vehicle.VehicleBrandId);
                await GetVehicleModels(vehicleBrand);
                var vehicleModel = this.VehiclesModelsCollection.FirstOrDefault(x => x.Id == vehicle.VehicleModelId);

                this.SelectedVehicleBrand = vehicleBrand;
                this.SelectedVehicleType = vehicleType;
                this.SelectedVehicleUsage = vehicleUsage;
                this.SelectedVehicleModel = vehicleModel;
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
                FirstRegistrationDate = this.FirstRegistrationDate
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
                VehicleId = this.SelecteUserVehicle.VehicleId,
                FirstRegistrationDate = this.FirstRegistrationDate
            };

            var result = await this._vehicleService.EditVehicle(req, httpClient, jwt);

            return result;
        }
        private async Task UpdateVehicleCollection(VehicleResultModel vehicle)
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
                FirstRegistrationDate = vehicle.FirstRegistrationDate.HasValue ? vehicle.FirstRegistrationDate.Value : DateTime.Now
            };

            this.UserVehiclesCollection.Add(current);
            this.SelecteUserVehicle = current;
            await this.ChangeUserVehicle(current);
        }
        private async Task<CheckVehicleCivilInsuranceAllowedResultModel> IsInsuranceAllowed()
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var result = await this._insuranceService.CheckVehicleCivilInsuranceAllowed(this.VehiclePlateNumber, jwt, httpClient);

            return result;
        }

        #endregion
    }
}
